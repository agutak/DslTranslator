namespace DslTranslator.AstNodes
{
    public abstract class UnaryOperatorAstNode : AstNode
    {
        public UnaryOperatorAstNode(Token token, AstNode target) : base(token)
        {
            Target = target;
        }

        public AstNode Target { get; }
    }
}
