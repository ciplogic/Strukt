using Strukt.Parse.ParseNodeTypes;

namespace Strukt.Parse
{
    internal class ClassNode : AstNode
    {
        public ClassNode() : base(AstNodeKind.Class)
        {
        }

        public string Name { get; set; }
    }
}