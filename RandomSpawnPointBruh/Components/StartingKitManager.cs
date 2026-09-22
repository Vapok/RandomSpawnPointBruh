using System.Collections;
using Jotunn.Managers;
using RandomSpawnPointBruh.Configuration;
using UnityEngine;

namespace RandomSpawnPointBruh.Components;

public static class StartingKitManager
{
    private const float MaxGroundedWaitSeconds = 5.0f;
    private static Coroutine _activeCoroutine;
    private static bool _hasPendingKit;

    public static bool HasPendingKit => _hasPendingKit;

    public static void QueueKitAward()
    {
        if (GUIManager.IsHeadless())
        {
            return;
        }

        _hasPendingKit = true;
        RandomSpawnPointBruh.Log.Debug("Starting kit queued for award on touchdown.");
    }

    public static void StartAwardCoroutine()
    {
        if (GUIManager.IsHeadless() || !_hasPendingKit)
        {
            return;
        }

        if (RandomSpawnPointBruh.Main == null)
        {
            return;
        }

        if (_activeCoroutine != null)
        {
            RandomSpawnPointBruh.Main.StopCoroutine(_activeCoroutine);
            _activeCoroutine = null;
        }

        RandomSpawnPointBruh.Log.Debug("Starting touchdown coroutine for kit award.");
        _activeCoroutine = RandomSpawnPointBruh.Main.StartCoroutine(AwardKitWhenReadyCoroutine());
    }

    private static IEnumerator AwardKitWhenReadyCoroutine()
    {
        if (GUIManager.IsHeadless())
        {
            _activeCoroutine = null;
            yield break;
        }

        while (true)
        {
            if (!_hasPendingKit || !ConfigRegistry.EnableStartingKits.Value)
            {
                _activeCoroutine = null;
                yield break;
            }

            if (Game.instance == null || Game.instance.IsShuttingDown())
            {
                _activeCoroutine = null;
                yield break;
            }

            Player localPlayer = Player.m_localPlayer;
            if (localPlayer == null || localPlayer.IsDead())
            {
                yield return null;
                continue;
            }

            if (Game.instance.WaitingForRespawn())
            {
                yield return null;
                continue;
            }

            if (localPlayer.InIntro() || localPlayer.InCutscene() || localPlayer.IsTeleporting())
            {
                yield return null;
                continue;
            }

            if (Hud.instance == null || Hud.instance.m_loadingScreen == null)
            {
                yield return null;
                continue;
            }

            if (Hud.instance.m_loadingScreen.gameObject.activeInHierarchy && Hud.instance.m_loadingScreen.alpha > 0.05f)
            {
                yield return null;
                continue;
            }

            if (WorldGenerator.instance == null)
            {
                yield return null;
                continue;
            }

            break;
        }

        float groundedTimer = 0f;
        while (groundedTimer < MaxGroundedWaitSeconds)
        {
            Player localPlayer = Player.m_localPlayer;
            if (localPlayer == null || localPlayer.IsDead())
            {
                _activeCoroutine = null;
                yield break;
            }

            if (localPlayer.IsOnGround() || localPlayer.InWater() || localPlayer.IsSwimming())
            {
                break;
            }

            groundedTimer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        if (_hasPendingKit)
        {
            Player finalPlayer = Player.m_localPlayer;
            if (finalPlayer != null && !finalPlayer.IsDead())
            {
                AwardKit(finalPlayer);
                _hasPendingKit = false;
            }
        }

        _activeCoroutine = null;
    }

    private static void AwardKit(Player player)
    {
        if (WorldGenerator.instance == null)
        {
            return;
        }

        Vector3 position = player.transform.position;
        Heightmap.Biome biome = WorldGenerator.instance.GetBiome(position.x, position.z);
        BiomeStartingKit kit = ConfigRegistry.GetKit(biome);

        if (kit == null)
        {
            if (biome == Heightmap.Biome.Ocean || player.InWater() || player.IsSwimming())
            {
                SpawnOceanKarve(player);
                return;
            }

            RandomSpawnPointBruh.Log.Warning($"No starting kit registered for biome '{biome}'.");
            return;
        }

        bool success = kit.TryGiveKit(player);
        if (success)
        {
            RandomSpawnPointBruh.Log.Debug($"Awarded {biome} Starting Kit to '{player.GetPlayerName()}'.");
        }
    }

    private static void SpawnOceanKarve(Player player)
    {
        if (ZNetScene.instance == null)
        {
            return;
        }

        GameObject karvePrefab = ZNetScene.instance.GetPrefab("Karve");
        if (karvePrefab == null)
        {
            return;
        }

        float waterLevel = ZoneSystem.instance != null ? ZoneSystem.instance.m_waterLevel : 30.0f;
        Vector3 forward = player.transform.forward;
        forward.y = 0f;
        if (forward.sqrMagnitude < 0.01f)
        {
            forward = Vector3.forward;
        }
        forward.Normalize();

        Vector3 boatPos = player.transform.position + forward * 6.0f;
        boatPos.y = waterLevel;
        Quaternion rotation = Quaternion.LookRotation(forward);

        UnityEngine.Object.Instantiate(karvePrefab, boatPos, rotation);
        RandomSpawnPointBruh.Log.Debug($"Ocean Easter Egg: Spawned Karve at {boatPos} for '{player.GetPlayerName()}'.");

        if (ConfigRegistry.ClearVanillaStartingItems.Value)
        {
            GiveEmergencyItem(player, "ArmorRagsChest", 1);
            GiveEmergencyItem(player, "ArmorRagsLegs", 1);
        }
        GiveEmergencyItem(player, "Bread", 2);
    }

    private static void GiveEmergencyItem(Player player, string prefabName, int count)
    {
        if (ObjectDB.instance == null)
        {
            return;
        }

        GameObject prefab = ObjectDB.instance.GetItemPrefab(prefabName);
        if (prefab == null)
        {
            return;
        }

        ItemDrop itemDrop = prefab.GetComponent<ItemDrop>();
        if (itemDrop == null)
        {
            return;
        }

        ItemDrop.ItemData itemData = itemDrop.m_itemData.Clone();
        itemData.m_stack = count;
        if (player.GetInventory().AddItem(itemData))
        {
            ItemDrop.ItemData addedItem = player.GetInventory().GetItem(prefabName, itemData.m_quality, isPrefabName: true);
            if (addedItem != null)
            {
                player.AddKnownItem(addedItem);
                if (addedItem.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Chest || addedItem.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Legs)
                {
                    player.EquipItem(addedItem, triggerEquipEffects: false);
                }
            }
        }
    }

    public static void ResetKitQueue()
    {
        if (GUIManager.IsHeadless())
        {
            return;
        }

        QueueKitAward();
        StartAwardCoroutine();
        RandomSpawnPointBruh.Log.Debug("Queued starting kit for local player via console reset.");
    }

    public static void Reset()
    {
        _hasPendingKit = false;
        if (_activeCoroutine != null && RandomSpawnPointBruh.Main != null)
        {
            RandomSpawnPointBruh.Main.StopCoroutine(_activeCoroutine);
        }
        _activeCoroutine = null;
    }
}
