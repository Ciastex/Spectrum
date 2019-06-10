using Events;
using Events.Scene;
using Spectrum.API.Interfaces.Systems;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Logger = Spectrum.API.Logging.Logger;

namespace Spectrum.API.Security
{
    internal class CheatRegistry : ICheatRegistry
    {
        private readonly Dictionary<Assembly, List<string>> _cheatStates;
        private readonly SubscriberList _subscriberList;

        private Logger Logger { get; }
        private IManager Manager { get; }

        public bool AnyCheatsEnabled => _cheatStates.Values.Any(x => x.Any());

        internal CheatRegistry(IManager manager)
        {
            _cheatStates = new Dictionary<Assembly, List<string>>();

            _subscriberList = new SubscriberList
            {
                new StaticEvent<LoadFinish.Data>.Subscriber(OnSceneLoadFinished),
            };
            _subscriberList.Subscribe();

            Logger = new Logger(Defaults.CheatSystemLogFileName) { WriteToConsole = true };
            Manager = manager;
        }

        public void Enable(string key)
        {
            var callingAssembly = Assembly.GetCallingAssembly();

            if (!_cheatStates.ContainsKey(callingAssembly))
                _cheatStates.Add(callingAssembly, new List<string>() { key });
            else
            {
                if (!_cheatStates[callingAssembly].Contains(key))
                    _cheatStates[callingAssembly].Add(key);
                else Logger.Warning($"Plugin assembly {Path.GetFileName(callingAssembly.Location)} tried to add cheat key '{key}' more than once.");
            }

            G.Sys.CheatsManager_.anyGameplayCheatsUsedThisLevel_ = AnyCheatsEnabled;
        }

        public void Disable(string key)
        {
            var callingAssembly = Assembly.GetCallingAssembly();

            if (_cheatStates.ContainsKey(callingAssembly))
            {
                if (_cheatStates[callingAssembly].Contains(key))
                    _cheatStates[callingAssembly].Remove(key);
                else Logger.Warning($"Plugin assembly {Path.GetFileName(callingAssembly.Location)} tried to remove non-existent cheat key '{key}'.");
            }
        }

        public bool IsCheatEnabled(string key)
        {
            var callingAssembly = Assembly.GetCallingAssembly();

            if (_cheatStates.ContainsKey(callingAssembly))
                return _cheatStates[callingAssembly].Contains(key);

            return false;
        }

        private void OnSceneLoadFinished(LoadFinish.Data data)
        {
            G.Sys.CheatsManager_.anyGameplayCheatsUsedThisLevel_ = AnyCheatsEnabled;
        }
    }
}
