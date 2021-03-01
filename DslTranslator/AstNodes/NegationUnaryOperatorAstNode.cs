namespace DslTranslator.AstNodes
{
    public class NegationUnaryOperatorAstNode : UnaryOperatorAstNode
    {
        public NegationUnaryOperatorAstNode(Token token, AstNode target)
            : base(token, target)
        {

        }
    }
}
