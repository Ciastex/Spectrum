using System;

namespace Spectrum.API.Exceptions
{
    public class MetadataReadException : Exception
    {
        internal bool StreamFailed { get; }
        internal string Json { get; }

        public MetadataReadException(string message, bool streamFailed, string json = "", Exception innerException = null) : base(message, innerException)
        {
            StreamFailed = streamFailed;
            Json = json;
        }
    }
}
