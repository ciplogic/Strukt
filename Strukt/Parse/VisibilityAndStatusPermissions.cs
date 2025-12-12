using System;
using System.Collections.Generic;

namespace Strukt.Parse;

class VisibilityAndStatusPermissions
{
    private List<string> Permissions { get; } = [];
    private static string[] VisiblityKeywords = ["public", "private", "internal", "static", "readonly", "override"];

    public bool AddPermission(string permission)
    {
        if (VisiblityKeywords.Contains(permission))
        {
            Permissions.Add(permission);
            return true;
        }

        return false;
    }

    public void Clear() => Permissions.Clear();

    public string[] ToArray() => Permissions.ToArray();

    public override string ToString() => string.Join(',', Permissions);
}