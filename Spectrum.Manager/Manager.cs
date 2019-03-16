using Events;
using Events.ClientToAllClients;
using Harmony;
using Spectrum.API;
using Spectrum.API.Configuration;
using Spectrum.API.Events.EventArgs;
using Spectrum.API.Interfaces.Plugins;
using Spectrum.API.Interfaces.Systems;
using Spectrum.API.IPC;
using Spectrum.API.Logging;
using Spectrum.API.Network;
using Spectrum.API.Network.Events;
using Spectrum.API.Security;
using Spectrum.Manager.GUI;
using Spectrum.Manager.Input;
using Spectrum.Manager.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Spectrum.Manager
{
    public class Manager : IManager
    {
        private PluginRegistry PluginRegistry { get; set; }
        private PluginLoader PluginLoader { get; set; }
        private HarmonyInstance HarmonyInstance { get; set; }
        private Logger Log { get; }

        public event EventHandler<PluginInitializationEventArgs> PluginInitialized;
        public IHotkeyManager Hotkeys { get; }
        public IEventRouter EventRouter { get; }
        public ICheatSystem CheatSystem { get; }
        public IMenuManager Menus { get; set; }

        public bool IsEnabled { get; set; } = true;
        public bool CanLoadPlugins => Directory.Exists(Defaults.ManagerPluginDirectory);

        public Manager()
        {
            Log = new Logger(Defaults.ManagerLogFileName) { WriteToConsole = true };
            Log.Info("Spectrum Plugin Manager started.");

            CheckPaths();
            InitializeSettings();

            HarmonyInstance = HarmonyInstance.Create("Spectrum.Manager");
            HarmonyInstance.PatchAll(AppDomain.CurrentDomain.GetAssemblies().First(asm => asm.GetName().Name == "Spectrum.API"));

            if (!Global.Settings.GetItem<bool>("Enabled"))
            {
                Log.Error("Spectrum is disabled. Set 'Enabled' entry to 'true' in settings to activate plugin functionality.");
                IsEnabled = false;

                return;
            }

            InitializeNetworkOverrides();

            EventRouter = new EventRouter();
            Hotkeys = new HotkeyManager();
            Menus = new MenuManager();

            CheatSystem = new CheatSystem(this);

            CheatSystem.CheatStateInfoReceived += (sender, args) =>
            {
                if (!G.Sys.NetworkingManager_.IsServer_) return;


                if (args.CheatStateInfo.AnyCheatsEnabled)
                {
                    if (!Global.Settings.GetItem<bool>("AllowCheatsOnline"))
                    {
#pragma warning disable 618
                        StaticTransceivedEvent<ChatMessage.Data>.Broadcast(new ChatMessage.Data("[FF44FF]Player disconnected due to cheats not being allowed in this game.[-]"));
#pragma warning restore 618
                        UnityEngine.Network.CloseConnection(args.Sender, true);
                    }
                }
            };

            CheatSystem.CheatStateInfoFailure += (sender, args) =>
            {
                if (!G.Sys.NetworkingManager_.IsServer_) return;

                if (Global.Settings.GetItem<bool>("KickCheatRequestFailures"))
                {
#pragma warning disable 618
                    StaticTransceivedEvent<ChatMessage.Data>.Broadcast(new ChatMessage.Data("[FF44FF]Player disconnected because of cheat state verification failure.[-]"));
#pragma warning restore 618
                    UnityEngine.Network.CloseConnection(args.Sender, true);
                }
            };

            LoadExtensions();
            StartExtensions();
        }

        private void InitializeNetworkOverrides()
        {
            Log.Info("Initializing network overrides...");

            NetworkOverrides.RegisterBroadcastAllEvent<BroadcastAll.Data>();
            NetworkOverrides.RegisterClientToServerEvent<ClientToServer.Data>();
            NetworkOverrides.RegisterServerToClientEvent<ServerToClient.Data>();
            NetworkOverrides.RegisterTargetedEvent<ServerToClient.Data>();
        }

        public void SendIPC(string ipcIdentifierTo, IPCData data)
        {
            var pluginHost = PluginRegistry.GetByIPCIdentifier(ipcIdentifierTo);

            if (pluginHost != null)
            {
                if (!pluginHost.IsIPCEnabled)
                {
                    Log.Error($"Plugin with IPC ID '{data.SourceIdentifier}' tried to send IPCData to '{ipcIdentifierTo}', but the target is not IPC enabled.");
                    return;
                }

                (pluginHost.Instance as IIPCEnabled).HandleIPCData(data);
            }
        }

        public bool IsAvailableForIPC(string ipcIdentifier)
        {
            return PluginRegistry.GetByIPCIdentifier(ipcIdentifier) != null;
        }

        public List<PluginInfo> QueryLoadedPlugins()
        {
            return PluginRegistry.QueryLoadedPlugins();
        }

        public bool SetConfig<T>(string key, T value)
        {
            if (!Global.Settings.ContainsKey<T>(key))
                return false;

            Global.Settings[key] = value;
            Global.Settings.Save();

            return true;
        }

        public T GetConfig<T>(string key)
        {
            try
            {
                return Global.Settings.GetItem<T>(key);
            }
            catch (Exception ex)
            {
                Log.Error("Manager exception occured while a plugin tried to get manager's configuration entry. Check the log file for details.");

                Log.ExceptionSilent(ex);
                return default(T);
            }
        }

        public void CheckPaths()
        {
            if (!Directory.Exists(Defaults.ManagerSettingsDirectory))
            {
                Log.Info("Settings directory does not exist. Creating...");
                Directory.CreateDirectory(Defaults.ManagerSettingsDirectory);
            }

            if (!Directory.Exists(Defaults.ManagerLogDirectory))
            {
                Log.Info("Log directory does not exist. Creating...");
                Directory.CreateDirectory(Defaults.ManagerLogDirectory);
            }

            if (!Directory.Exists(Defaults.ManagerPluginDirectory))
            {
                Log.Info("Plugin directory does not exist. Creating...");
                Directory.CreateDirectory(Defaults.ManagerPluginDirectory);
            }
        }

        public void UpdateExtensions()
        {
            if (!IsEnabled)
                return;

            ((HotkeyManager)Hotkeys).Update();

            if (PluginRegistry != null)
            {
                foreach (var pluginInfo in PluginRegistry)
                {
                    if (pluginInfo.Enabled && pluginInfo.UpdatesEveryFrame && pluginInfo.Instance is IUpdatable plugin)
                        plugin.Update();
                }
            }
        }

        private void InitializeSettings()
        {
            try
            {
                Global.Settings = new Settings("ManagerSettings");

                Global.Settings.GetOrCreate("LogToConsole", true);
                Global.Settings.GetOrCreate("Enabled", true);
                Global.Settings.GetOrCreate("AllowCheatsOnline", false);
                Global.Settings.GetOrCreate("KickCheatRequestFailures", true);

                if (Global.Settings.Dirty)
                    Global.Settings.Save();
            }
            catch (Exception ex)
            {
                Log.Error($"Couldn't load settings. Defaults loaded. Exception has been caught, see the log for details.");
                Log.ExceptionSilent(ex);
            }
        }

        private void LoadExtensions()
        {
            PluginRegistry = new PluginRegistry();

            PluginLoader = new PluginLoader(Defaults.ManagerPluginDirectory, PluginRegistry);
            PluginLoader.LoadPlugins();
        }

        private void StartExtensions()
        {
            if (PluginRegistry == null || PluginRegistry.Count == 0)
            {
                Log.Info("No plugins loaded, skipping initalization.");
                return;
            }

            Log.Info("Initializing extensions...");

            for (var i = 0; i < PluginRegistry.Count; i++)
            {
                var pluginHost = PluginRegistry[i];

                try
                {
                    var pluginInfo = PluginRegistry.GetPluginInfoByName(pluginHost.Manifest.FriendlyName);
                    var isLastPlugin = (i == PluginRegistry.Count - 1);

                    pluginHost.Instance.Initialize(this, pluginHost.Manifest.IPCIdentifier);
                    PluginInitialized?.Invoke(this, new PluginInitializationEventArgs(pluginInfo, isLastPlugin));

                    Log.Info($"Plugin {pluginHost.Manifest.FriendlyName} initialized");
                }
                catch (Exception ex)
                {
                    Log.Error($"Plugin {pluginHost.Manifest.FriendlyName} failed to initialize. Exception has been caught, see the log for details.");
                    Log.ExceptionSilent(ex);
                }
            }

            Log.Info("Extensions initialized.");
        }
    }
}
