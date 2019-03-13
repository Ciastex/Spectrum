using System;

namespace Spectrum.API.Exceptions
{
    public class SettingsException : Exception
    {
        public string Key { get; }
        public bool IsJsonFailure { get; }
        
        public SettingsException(string message, string key, bool isJsonFailure, Exception innerException) : base(message, innerException)
        {
            Key = key;
            IsJsonFailure = isJsonFailure;
        }
        public SettingsException(string message, string key, bool isJsonFailure) : this(message, key, isJsonFailure, null) { }
    }
}
