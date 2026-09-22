/* RandomSpawnPointBruh by Vapok */
using System;
using System.Reflection;
using BepInEx;
using BepInEx.Bootstrap;
using HarmonyLib;
using JetBrains.Annotations;
using Jotunn.Managers;
using Jotunn.Utils;
using RandomSpawnPointBruh.Components;
using RandomSpawnPointBruh.Configuration;
using Vapok.Common.Abstractions;
using Vapok.Common.Managers;
using Vapok.Common.Managers.Configuration;
using Vapok.Common.Managers.LocalizationManager;
using Vapok.Common.Managers.Splash;
using Vapok.Common.Tools;

namespace RandomSpawnPointBruh
{
    [BepInDependency(Jotunn.Main.ModGuid)]
    [BepInDependency("com.ValheimModding.YamlDotNetDetector")]
    [BepInDependency("org.bepinex.plugins.servercharacters", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.orianaventure.mod.MultiplayerTweaks", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin(_pluginId, _displayName, _version)]
    [SynchronizationMode(AdminOnlyStrictness.IfOnServer)]
    
    public class RandomSpawnPointBruh : BaseUnityPlugin, IPluginInfo
    {
        //Module Constants
        private const string _pluginId = "vapok.mods.RandomSpawnPointBruh";
        private const string _displayName = "RandomSpawnPointBruh";
        private const string _version = "2.1.0";
        
        //Interface Properties
        public string PluginId => _pluginId;
        public string DisplayName => _displayName;
        public string Version => _version;
        public BaseUnityPlugin Instance => _instance;
        public static RandomSpawnPointBruh Main => _instance;
        
        //Class Properties
        public static ILogIt Log => _log;
        public static bool ValheimAwake = false;
        public static Waiting Waiter;
        public bool HasCompetingMods;
        
        //Class Privates
        private static RandomSpawnPointBruh _instance;
        private static ConfigSyncBase _config;
        private static ILogIt _log;
        private Harmony _harmony;
        
        [UsedImplicitly]
        // This the main function of the mod. BepInEx will call this.
        private void Awake()
        {
            //I'm awake!
            _instance = this;
            
            HasCompetingMods = Chainloader.PluginInfos.ContainsKey("org.bepinex.plugins.servercharacters") || Chainloader.PluginInfos.ContainsKey("com.orianaventure.mod.MultiplayerTweaks");
            
            //Waiting For Startup
            Waiter = new Waiting();
            
            //Jotunn Localization
            Jotunn.Entities.CustomLocalization localization = LocalizationManager.Instance.GetLocalization();

            //Register Logger
            LogManager.Init(PluginId,out _log);
            
            //Initialize Managers
            Initializer.LoadManagers(localization);

            //Register Configuration Settings
            _config = new ConfigRegistry(_instance);

            ModSplashManager.Register(new ModSplashDossier(_instance)
            {
                Tagline = "Randomized and static player respawn and initial spawn point mechanics.",
                ShowOnStartup = ConfigRegistry.ShowSplashOnStartup,
                EnableTelemetry = ConfigRegistry.EnableTelemetry,
            });

            Log.Debug($"HasCompetingMods: {HasCompetingMods}");
            Localizer.Waiter.StatusChanged += InitializeModule;
            
            //Patch Harmony
            _harmony = new Harmony(Info.Metadata.GUID);
            _harmony.PatchAll(Assembly.GetExecutingAssembly());

            //???

            //Profit
        }
        
        private void Update()
        {
            if (!Player.m_localPlayer || !ZNetScene.instance)
                return;
        }

        public void InitializeModule(object send, EventArgs args)
        {
            if (ValheimAwake)
                return;
            
            //Register Assets
            ConfigRegistry.Waiter.ConfigurationComplete(true);

            ValheimAwake = true;
        }
        
        private void OnDestroy()
        {
            SpawnPointGenerator.Reset();
            StartingKitManager.Reset();
            _instance = null;
        }
    }
}