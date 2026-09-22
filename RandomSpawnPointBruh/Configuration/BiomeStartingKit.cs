using System;
using System.Collections.Generic;
using BepInEx.Configuration;
using UnityEngine;
using Vapok.Common.Managers.Configuration;
using Vapok.Common.Shared;

namespace RandomSpawnPointBruh.Configuration;

public class BiomeStartingKit
{
    public Heightmap.Biome Biome { get; }
    public string SectionName { get; }

    public ConfigEntry<bool> Enabled;
    public ConfigEntry<string> KitItems;

    public BiomeStartingKit(Heightmap.Biome biome, string sectionName)
    {
        Biome = biome;
        SectionName = sectionName;
    }

    public void Register(ConfigRegistry registry, string defaultItems)
    {
        ConfigSyncBase.SyncedConfig(SectionName, "Enabled", true,
            new ConfigDescription($"Enables the starting kit for {Biome}. If false, vanilla starting items are kept.",
                null,
                new ConfigurationManagerAttributes { Category = SectionName, Order = 2 }), ref Enabled);

        ConfigSyncBase.SyncedConfig(SectionName, "Kit Items", defaultItems,
            new ConfigDescription("Items awarded on first spawn. Format: PrefabName:Count[:Quality], separated by commas.",
                null,
                new ConfigurationManagerAttributes { Category = SectionName, Order = 1 }), ref KitItems);
    }

    public bool TryGiveKit(Player player)
    {
        if (player == null || player.IsDead() || !Enabled.Value || ObjectDB.instance == null)
        {
            return false;
        }

        if (ConfigRegistry.ClearVanillaStartingItems.Value)
        {
            RemoveVanillaStartingItem(player, "ArmorRagsChest");
            RemoveVanillaStartingItem(player, "ArmorRagsLegs");
            RemoveVanillaStartingItem(player, "Torch");
        }

        List<string> consumablePrefabs = new List<string>();
        string rawItems = KitItems.Value;
        if (!string.IsNullOrEmpty(rawItems))
        {
            string[] tokens = rawItems.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < tokens.Length; i++)
            {
                string token = tokens[i].Trim();
                if (string.IsNullOrEmpty(token))
                {
                    continue;
                }

                string[] parts = token.Split(':');
                string prefabName = parts[0].Trim();
                int count = 1;
                int quality = 1;

                if (parts.Length > 1 && !int.TryParse(parts[1].Trim(), out count))
                {
                    count = 1;
                }

                if (parts.Length > 2 && !int.TryParse(parts[2].Trim(), out quality))
                {
                    quality = 1;
                }

                GameObject prefab = ObjectDB.instance.GetItemPrefab(prefabName);
                if (prefab == null)
                {
                    RandomSpawnPointBruh.Log.Warning($"Starting Kit ({Biome}): Item prefab '{prefabName}' not found in ObjectDB!");
                    continue;
                }

                ItemDrop itemDrop = prefab.GetComponent<ItemDrop>();
                if (itemDrop == null)
                {
                    RandomSpawnPointBruh.Log.Warning($"Starting Kit ({Biome}): Prefab '{prefabName}' does not have an ItemDrop component!");
                    continue;
                }

                bool isConsumable = itemDrop.m_itemData.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Consumable;
                if (isConsumable && !consumablePrefabs.Contains(prefabName))
                {
                    consumablePrefabs.Add(prefabName);
                }

                int remaining = count;
                while (remaining > 0)
                {
                    int toAdd = Mathf.Min(remaining, itemDrop.m_itemData.m_shared.m_maxStackSize);
                    ItemDrop.ItemData itemData = itemDrop.m_itemData.Clone();
                    itemData.m_stack = toAdd;
                    itemData.m_quality = Mathf.Clamp(quality, 1, itemData.m_shared.m_maxQuality);
                    itemData.m_durability = itemData.GetMaxDurability();

                    if (player.GetInventory().AddItem(itemData))
                    {
                        ItemDrop.ItemData addedItem = player.GetInventory().GetItem(prefabName, itemData.m_quality, isPrefabName: true);
                        if (addedItem != null)
                        {
                            player.AddKnownItem(addedItem);

                            if (ConfigRegistry.AutoEquipGear.Value && IsEquippable(addedItem))
                            {
                                player.EquipItem(addedItem, triggerEquipEffects: false);
                            }
                        }
                    }
                    else
                    {
                        RandomSpawnPointBruh.Log.Warning($"Starting Kit ({Biome}): Inventory full, could not add '{prefabName}'!");
                        break;
                    }

                    remaining -= toAdd;
                }
            }
        }

        if (ConfigRegistry.AutoConsumeConsumables.Value)
        {
            for (int i = 0; i < consumablePrefabs.Count; i++)
            {
                string consumablePrefab = consumablePrefabs[i];
                ItemDrop.ItemData item = player.GetInventory().GetItem(consumablePrefab, -1, isPrefabName: true);
                if (item != null)
                {
                    bool consumed = player.ConsumeItem(player.GetInventory(), item);
                    if (!consumed)
                    {
                        if (item.m_shared.m_food > 0f)
                        {
                            if (player.EatFood(item))
                            {
                                player.GetInventory().RemoveOneItem(item);
                                RandomSpawnPointBruh.Log.Debug($"Starting Kit ({Biome}): Auto-consumed 1x food '{consumablePrefab}'.");
                            }
                        }
                        else if (item.m_shared.m_consumeStatusEffect != null)
                        {
                            player.GetSEMan().AddStatusEffect(item.m_shared.m_consumeStatusEffect, resetTime: true);
                            player.GetInventory().RemoveOneItem(item);
                            RandomSpawnPointBruh.Log.Debug($"Starting Kit ({Biome}): Applied status effect and deducted 1x '{consumablePrefab}'.");
                        }
                    }
                    else
                    {
                        RandomSpawnPointBruh.Log.Debug($"Starting Kit ({Biome}): Auto-consumed 1x '{consumablePrefab}'.");
                    }
                }
            }
        }

        return true;
    }

    private static bool IsEquippable(ItemDrop.ItemData itemData)
    {
        if (itemData.IsWeapon())
        {
            return true;
        }

        ItemDrop.ItemData.ItemType itemType = itemData.m_shared.m_itemType;
        return itemType == ItemDrop.ItemData.ItemType.Helmet
            || itemType == ItemDrop.ItemData.ItemType.Chest
            || itemType == ItemDrop.ItemData.ItemType.Legs
            || itemType == ItemDrop.ItemData.ItemType.Shoulder
            || itemType == ItemDrop.ItemData.ItemType.Shield
            || itemType == ItemDrop.ItemData.ItemType.Utility;
    }

    private static void RemoveVanillaStartingItem(Player player, string prefabName)
    {
        ItemDrop.ItemData item = player.GetInventory().GetItem(prefabName, -1, isPrefabName: true);
        if (item != null)
        {
            if (player.IsItemEquiped(item))
            {
                player.UnequipItem(item, triggerEquipEffects: false);
            }
            player.GetInventory().RemoveItem(item);
        }
    }
}
