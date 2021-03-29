namespace Strukt.Parse.ParseNodeTypes
{
    public class ModuleNode : BodyNodes
    {
        public ModuleNode() : base(AstNodeKind.Module)
        {
        }

        public string Name { get; set; }
    }
}