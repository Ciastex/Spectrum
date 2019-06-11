namespace Spectrum.Prism.Enums
{
    public enum TerminationReason
    {
        InvalidSyntax = -1,
        Default = 0,
        TargetDllNotProvided = 1,
        PatchNameNotProvided = 2,
        SourceDllNotProvided = 3,
        TargetDllNonexistant = 4,
        SourceDllNonexistant = 5,
        TargetModuleLoadFailed = 6,
        BootstrapModuleLoadFailed = 7,
        RequiredDependenciesMissing = 8,
        AssemblySaveFailed = 9,
        PatchFailure = 10,
        PatchNotFound = 11
    }
}
