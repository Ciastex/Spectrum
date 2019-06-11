using System;
using Spectrum.Prism.Enums;
using Spectrum.Prism.IO;

namespace Spectrum.Prism
{
    internal class ErrorHandler
    {
        public static void TerminateWithError(string message, TerminationReason reason = 0)
        {
            ColoredOutput.WriteError(message);
            Environment.Exit((int)reason);
        }
    }
}
