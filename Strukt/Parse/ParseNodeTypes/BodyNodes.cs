using System.Collections.Generic;

namespace Strukt.Parse.ParseNodeTypes
{
    public abstract class BodyNodes : AstNode
    {
        protected BodyNodes(AstNodeKind kind) : base(kind)
        {
        }

        public List<AstNode> Children { get; } = new();
    }
}