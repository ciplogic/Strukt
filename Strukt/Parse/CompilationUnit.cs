using System.Collections.Generic;

namespace Strukt.Parse;

internal class CompilationUnit
{
    public string Namespace { get; set; }
    public List<string> Usage { get; set; } = [];

    public Dictionary<string, NamedDefinition> Definitions { get; set; } = [];
}