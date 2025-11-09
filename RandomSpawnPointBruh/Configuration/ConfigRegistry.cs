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
        internal static ConfigEntry<int> MaxRangeIncreases;
        internal static ConfigEntry<int> RangeIncrement;
        internal static ConfigEntry<int> MaxPointsInRange;
        internal static ConfigEntry<float> MinSearchRange;
        internal static ConfigEntry<float> MaxSearchRange;
        internal static ConfigEntry<float> RangeSeparationFactor;
        internal static ConfigEntry<float> AshlandsStart;
        internal static ConfigEntry<float> DeepNorthStart;
        internal static ConfigEntry<UseableBiomes> SpawnBiome;
        
        //Static Spawn Point
        internal static ConfigEntry<Vector3> CustomSpawnPoint;
        
        
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
            SyncedConfig("General Settings (Synced)", "Enable Random Spawn Bruh!", true,
                new ConfigDescription("If true, will randomize new player spawn points within the area parameters. Requires Game Restart.",
                    null, 
                    new ConfigurationManagerAttributes { Category = "General Settings (Synced)", Order = 1 }),ref Enabled);

            SyncedConfig("General Settings (Synced)", "Spawn Point Method", SpawnFunction.RandomSpawnPoint,
                new ConfigDescription("Spawn method to use. Only affects NEW players who join the world.",
                    null, 
                    new ConfigurationManagerAttributes { Category = "General Settings (Synced)", Order = 2 }),ref SpawnMethod);

            SyncedConfig("Static Spawn Settings (Synced)", "Spawn Point", Vector3.zero, 
                new ConfigDescription("Vector Coordinates to use for all players spawns.",
                    null, 
                    new ConfigurationManagerAttributes { Category = "Static Spawn Settings (Synced)", Order = 1 }),ref CustomSpawnPoint);

            SyncedConfig("Random Spawn Settings (Synced)", "Max Range Increments", 10, 
                new ConfigDescription("Max number of range increments in order to find a spawn point.",
                    null, 
                    new ConfigurationManagerAttributes { Category = "Random Spawn Settings (Synced)", Order = 1 }),ref MaxRangeIncreases);

            SyncedConfig("Random Spawn Settings (Synced)", "Range Increment", 50, 
                new ConfigDescription("Amount to increase radius each time the search range is incremented.",
                    null, 
                    new ConfigurationManagerAttributes { Category = "Random Spawn Settings (Synced)", Order = 2 }),ref RangeIncrement);

            SyncedConfig("Random Spawn Settings (Synced)", "Max Points In Range", 25, 
                new ConfigDescription("Max number of random points to check within a zone before increasing search range.",
                    null, 
                    new ConfigurationManagerAttributes { Category = "Random Spawn Settings (Synced)", Order = 3 }),ref MaxPointsInRange);

            SyncedConfig("Random Spawn Settings (Synced)", "Min Search Range", 500f, 
                new ConfigDescription("Inner most distance from center point to begin looking for a spawn point.",
                    new AcceptableValueRange<float>(100f, 9000f), 
                    new ConfigurationManagerAttributes { Category = "Random Spawn Settings (Synced)", Order = 4 }),ref MinSearchRange);

            SyncedConfig("Random Spawn Settings (Synced)", "Max Search Range", 5000f, 
                new ConfigDescription("Outer most distance from center point to stop looking for a spawn point.",
                    new AcceptableValueRange<float>(100f, 9000f), 
                    new ConfigurationManagerAttributes { Category = "Random Spawn Settings (Synced)", Order = 5 }),ref MaxSearchRange);
            
            SyncedConfig("Random Spawn Settings (Synced)", "Separation Factor", 400f, 
                new ConfigDescription("Factor to increase randomizer between min and max randomizations.",
                    new AcceptableValueRange<float>(0f, 1000f), 
                    new ConfigurationManagerAttributes { Category = "Random Spawn Settings (Synced)", Order = 6 }),ref RangeSeparationFactor);
            
            SyncedConfig("Random Spawn Settings (Synced)", "Biome for Spawn Points", UseableBiomes.Meadows, 
                new ConfigDescription("Defines which biome to look for spawn points.",
                    null, 
                    new ConfigurationManagerAttributes { Category = "Random Spawn Settings (Synced)", Order = 7 }),ref SpawnBiome);

            SyncedConfig("Random Spawn Settings (Synced)", "Ashlands Starting Point", -7500.0f, 
                new ConfigDescription("Defines Starting Y point for Ashlands",
                    null, 
                    new ConfigurationManagerAttributes { Category = "Random Spawn Settings (Synced)", Order = 8 }),ref AshlandsStart);
            
            SyncedConfig("Random Spawn Settings (Synced)", "Deep North Starting Point", 7500.0f, 
                new ConfigDescription("Defines Starting Y point for Deep North",
                    null, 
                    new ConfigurationManagerAttributes { Category = "Random Spawn Settings (Synced)", Order = 8 }),ref DeepNorthStart);
            
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