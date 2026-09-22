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