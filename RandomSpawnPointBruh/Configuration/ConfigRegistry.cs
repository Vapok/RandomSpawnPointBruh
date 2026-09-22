using System;
using BepInEx.Configuration;
using RandomSpawnPointBruh.Components;
using UnityEngine;
using Vapok.Common.Abstractions;
using Vapok.Common.Managers.Configuration;
using Vapok.Common.Shared;

namespace RandomSpawnPointBruh.Configuration
{
    public class ConfigRegistry : ConfigSyncBase
    {
        //Configuration Entry Privates
        //General Settings
        internal static ConfigEntry<bool> Enabled;
        internal static ConfigEntry<SpawnFunction> SpawnMethod;

        //Random Spawn
        internal static ConfigEntry<UseableBiomes> SpawnBiome;
        internal static ConfigEntry<float> MinSearchRange;
        internal static ConfigEntry<float> BiomePaddingDistance;
        internal static ConfigEntry<float> MinAltitudeAboveWater;
        internal static ConfigEntry<float> PlayerSeparationDistance;
        internal static ConfigEntry<float> SpecialPoiBufferDistance;
        internal static ConfigEntry<int> MaxSearchAttempts;
        
        //Static Spawn Point
        internal static ConfigEntry<Vector3> CustomSpawnPoint;

        //Starting Kits
        internal static ConfigEntry<bool> EnableStartingKits;
        internal static ConfigEntry<bool> ClearVanillaStartingItems;
        internal static ConfigEntry<bool> AutoEquipGear;
        internal static ConfigEntry<bool> AutoConsumeConsumables;

        private static readonly System.Collections.Generic.Dictionary<Heightmap.Biome, BiomeStartingKit> _kits = new System.Collections.Generic.Dictionary<Heightmap.Biome, BiomeStartingKit>();

        internal static ConfigEntry<bool> ShowSplashOnStartup;
        internal static ConfigEntry<bool> EnableTelemetry;
        
        public static Waiting Waiter;

        public ConfigRegistry(IPluginInfo mod): base(mod)
        {
            //Waiting For Startup
            Waiter = new Waiting();

            InitializeConfigurationSettings();
        }

        public sealed override void InitializeConfigurationSettings()
        {
            if (_config == null)
                return;
            
            //User Configs
            UnsyncedConfig("Local Config", "Show Splash on Startup", true,
                new ConfigDescription("If enabled, displays the mod overview and links splash screen on game startup.",
                    null, new ConfigurationManagerAttributes { Order = 4 }), ref ShowSplashOnStartup);

            UnsyncedConfig("Local Config", "Enable Anonymous Telemetry", true,
                new ConfigDescription("If enabled, sends anonymous mod launch and heartbeat telemetry to help improve mod stability and track active versions.",
                    null, new ConfigurationManagerAttributes { Order = 5 }), ref EnableTelemetry);

            SyncedConfig("General Settings (Synced)", "Enable Random Spawn Bruh!", true,
                new ConfigDescription("If true, will randomize new player spawn points within the area parameters. Requires Game Restart.",
                    null, 
                    new ConfigurationManagerAttributes { Category = "General Settings (Synced)", Order = 1 }), ref Enabled);

            SyncedConfig("General Settings (Synced)", "Spawn Point Method", SpawnFunction.RandomSpawnPoint,
                new ConfigDescription("Spawn method to use. Only affects NEW players who join the world.",
                    null, 
                    new ConfigurationManagerAttributes { Category = "General Settings (Synced)", Order = 2 }), ref SpawnMethod);

            SyncedConfig("Static Spawn Settings (Synced)", "Spawn Point", Vector3.zero, 
                new ConfigDescription("Vector Coordinates to use for all players spawns.",
                    null, 
                    new ConfigurationManagerAttributes { Category = "Static Spawn Settings (Synced)", Order = 1 }), ref CustomSpawnPoint);

            SyncedConfig("Random Spawn Settings (Synced)", "Biome for Spawn Points", UseableBiomes.Meadows, 
                new ConfigDescription("Defines which biome to look for spawn points.",
                    null, 
                    new ConfigurationManagerAttributes { Category = "Random Spawn Settings (Synced)", Order = 1 }), ref SpawnBiome);

            SyncedConfig("Random Spawn Settings (Synced)", "Min Search Range", 500f, 
                new ConfigDescription("Inner most distance from center point to begin looking for a spawn point.",
                    new AcceptableValueRange<float>(0f, 9000f), 
                    new ConfigurationManagerAttributes { Category = "Random Spawn Settings (Synced)", Order = 2 }), ref MinSearchRange);

            SyncedConfig("Random Spawn Settings (Synced)", "Biome Padding Distance", 100f, 
                new ConfigDescription("Minimum distance in meters to keep between spawn points and any other biome.",
                    new AcceptableValueRange<float>(0f, 500f), 
                    new ConfigurationManagerAttributes { Category = "Random Spawn Settings (Synced)", Order = 3 }), ref BiomePaddingDistance);

            SyncedConfig("Random Spawn Settings (Synced)", "Min Altitude Above Water", 4.0f, 
                new ConfigDescription("Minimum height in meters above sea level to place spawn points. Prevents spawning on soggy beaches, waterlines, or submerged ground.",
                    new AcceptableValueRange<float>(1.0f, 50.0f), 
                    new ConfigurationManagerAttributes { Category = "Random Spawn Settings (Synced)", Order = 4 }), ref MinAltitudeAboveWater);

            SyncedConfig("Random Spawn Settings (Synced)", "Player Separation Distance", 200f, 
                new ConfigDescription("Minimum distance in meters to keep between new spawn points and existing players, player bases, or wards.",
                    new AcceptableValueRange<float>(0f, 2000f), 
                    new ConfigurationManagerAttributes { Category = "Random Spawn Settings (Synced)", Order = 5 }), ref PlayerSeparationDistance);

            SyncedConfig("Random Spawn Settings (Synced)", "Special POI Buffer Distance", 100f, 
                new ConfigDescription("Buffer distance in meters beyond the POI reveal radius and Valkyrie flight path to prevent new spawns from revealing traders or unique locations.",
                    new AcceptableValueRange<float>(50f, 500f), 
                    new ConfigurationManagerAttributes { Category = "Random Spawn Settings (Synced)", Order = 6 }), ref SpecialPoiBufferDistance);

            SyncedConfig("Random Spawn Settings (Synced)", "Max Search Attempts", 250, 
                new ConfigDescription("Maximum number of random candidate points to test before falling back to the original start temple.",
                    new AcceptableValueRange<int>(10, 1000), 
                    new ConfigurationManagerAttributes { Category = "Random Spawn Settings (Synced)", Order = 7 }), ref MaxSearchAttempts);

            //Starting Kits
            SyncedConfig("Starting Kits (Synced)", "Enable Starting Kits", false,
                new ConfigDescription("If enabled, awards starting equipment and survival seeds tailored to the player's initial landing biome. Defaults to false to preserve vanilla behavior for existing installs.",
                    null,
                    new ConfigurationManagerAttributes { Category = "Starting Kits (Synced)", Order = 4 }), ref EnableStartingKits);

            SyncedConfig("Starting Kits (Synced)", "Clear Vanilla Starting Items", true,
                new ConfigDescription("If true, removes default rag clothes and torch before awarding the biome starting kit.",
                    null,
                    new ConfigurationManagerAttributes { Category = "Starting Kits (Synced)", Order = 3 }), ref ClearVanillaStartingItems);

            SyncedConfig("Starting Kits (Synced)", "Auto-Equip Gear", true,
                new ConfigDescription("If true, automatically equips weapons, shields, utility belts, and armor from the kit upon landing.",
                    null,
                    new ConfigurationManagerAttributes { Category = "Starting Kits (Synced)", Order = 2 }), ref AutoEquipGear);

            SyncedConfig("Starting Kits (Synced)", "Auto-Consume Foods and Meads", true,
                new ConfigDescription("If true, automatically consumes one of each food and mead item included in the starting kit upon landing.",
                    null,
                    new ConfigurationManagerAttributes { Category = "Starting Kits (Synced)", Order = 1 }), ref AutoConsumeConsumables);

            RegisterStartingKits();
        }

        private void RegisterStartingKits()
        {
            _kits.Clear();

            RegisterKit(Heightmap.Biome.Meadows, "Starting Kits: Meadows (Synced)",
                "ArmorRagsChest:1, ArmorRagsLegs:1, Club:1, Torch:1, Wood:10, Stone:6, Raspberry:5, CookedMeat:3");

            RegisterKit(Heightmap.Biome.BlackForest, "Starting Kits: Black Forest (Synced)",
                "ArmorLeatherChest:1, ArmorLeatherLegs:1, HelmetLeather:1, CapeDeerHide:1, Club:1, ShieldWood:1, Torch:1, HardAntler:1, Wood:10, Stone:6, CookedDeerMeat:3, Blueberries:5");

            RegisterKit(Heightmap.Biome.Swamp, "Starting Kits: Swamp (Synced)",
                "ArmorTrollLeatherChest:1, ArmorTrollLeatherLegs:1, HelmetTrollLeather:1, CapeTrollHide:1, MaceBronze:1:1, ShieldBronzeBuckler:1:1, PickaxeAntler:1, AxeFlint:1, Hoe:1, Hammer:1, MeadPoisonResist:4, CarrotSoup:3, CookedDeerMeat:3, QueensJam:3");

            RegisterKit(Heightmap.Biome.Mountain, "Starting Kits: Mountain (Synced)",
                "ArmorIronChest:1:1, ArmorIronLegs:1:1, HelmetIron:1:1, MaceIron:1:1, ShieldBanded:1:1, BowHuntsman:1:1, ArrowFire:30, PickaxeAntler:1, AxeBronze:1:1, Hammer:1, MeadFrostResist:4, Sausages:3, TurnipStew:3, CookedDeerMeat:3");

            RegisterKit(Heightmap.Biome.Plains, "Starting Kits: Plains (Synced)",
                "ArmorWolfChest:1:1, ArmorWolfLegs:1:1, HelmetDrake:1:1, CapeWolf:1:1, MaceBronze:1:4, ShieldBronzeBuckler:1:4, BowDraugrFang:1:1, ArrowObsidian:30, PickaxeAntler:1, AxeBronze:1:1, Hammer:1, WolfMeatSkewer:3, Eyescream:3, Sausages:3");

            RegisterKit(Heightmap.Biome.Mistlands, "Starting Kits: Mistlands (Synced)",
                "ArmorPaddedCuirass:1:1, ArmorPaddedGreaves:1:1, HelmetPadded:1:1, CapeLox:1:1, Demister:1, MaceSilver:1:1, ShieldBlackmetal:1:1, PickaxeAntler:1, AxeBlackMetal:1:1, Hammer:1, FineWood:30, DeerHide:10, Resin:20, BronzeNails:80, BarleyWine:4, LoxPie:3, BloodPudding:3, Bread:3");

            RegisterKit(Heightmap.Biome.AshLands, "Starting Kits: Ashlands (Synced)",
                "ArmorCarapaceChest:1:1, ArmorCarapaceLegs:1:1, HelmetCarapace:1:1, CapeLox:1:1, BeltStrength:1, SwordMistwalker:1:1, ShieldCarapace:1:1, PickaxeAntler:1, AxeBlackMetal:1:1, Hammer:1, BlackMarble:10, BlackCore:5, CeramicPlate:30, IronNails:100, FineWood:50, YggdrasilWood:35, BarleyWine:4, MisthareSupreme:3, MeatPlatter:3, Salad:3");

            RegisterKit(Heightmap.Biome.DeepNorth, "Starting Kits: Deep North (Synced)",
                "ArmorFlametalChest:1:1, ArmorFlametalLegs:1:1, HelmetFlametal:1:1, CapeLox:1:1, BowAshlands:1:1, ShieldFlametal:1:1, SwordMistwalker:1:1, ArrowFire:40, PickaxeAntler:1, AxeBlackMetal:1:1, Hammer:1, Wood:20, FineWood:30, DeerHide:10, Resin:20, BronzeNails:80, MeatPlatter:3, MisthareSupreme:3, Salad:3");
        }

        private void RegisterKit(Heightmap.Biome biome, string sectionName, string defaultItems)
        {
            BiomeStartingKit kit = new BiomeStartingKit(biome, sectionName);
            kit.Register(this, defaultItems);
            _kits[biome] = kit;
        }

        public static BiomeStartingKit GetKit(Heightmap.Biome biome)
        {
            if (_kits.TryGetValue(biome, out BiomeStartingKit kit))
            {
                return kit;
            }

            return null;
        }
    }
    
    public class Waiting
    {
        public void ConfigurationComplete(bool configDone)
        {
            if (configDone)
                StatusChanged?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler StatusChanged;            
    }
}