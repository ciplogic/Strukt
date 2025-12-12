namespace Strukt.Parse;

internal class ClassDefinition : NamedDefinition
{
    public ClassDefinition(string name, VisibilityAndStatusPermissions visibilityAndStatusPermissions)
    {
        Name = name;
        Permissions = visibilityAndStatusPermissions.ToArray();
    }

    public string[] Permissions { get; set; }
}