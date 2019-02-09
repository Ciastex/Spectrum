﻿using Spectrum.API.Events.EventArgs;
using Spectrum.API.IPC;
using System;
using System.Collections.Generic;

namespace Spectrum.API.Interfaces.Systems
{
    public interface IManager
    {
        event EventHandler<PluginInitializationEventArgs> PluginInitialized;

        IHotkeyManager Hotkeys { get; }
        IEventRouter EventRouter { get; }
        ICheatSystem CheatSystem { get; }

        void SendIPC(string ipcIdentifier, IPCData data);
        bool IsAvailableForIPC(string ipcIdentifier);

        bool SetConfig<T>(string key, T value);
        T GetConfig<T>(string key);

        List<PluginInfo> QueryLoadedPlugins();
    }
}
