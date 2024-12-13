namespace AMDevIT.AI.Core.Modules
{
    /// <summary>
    /// Informs the provider that this module cannot be duplicated.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class UniqueModuleAttribute
        : Attribute
    {
    }
}
