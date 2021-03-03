namespace Strukt.Parse.ParseNodeTypes
{
    public abstract class AstNode
    {
        public AstNodeKind Kind;

        protected AstNode(AstNodeKind kind)
        {
            Kind = kind;
        }
    }
}