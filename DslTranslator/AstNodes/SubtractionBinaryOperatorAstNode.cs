namespace DslTranslator.AstNodes
{
    public class SubtractionBinaryOperatorAstNode : BinaryOperatorAstNode
    {
        public SubtractionBinaryOperatorAstNode(Token token, AstNode left, AstNode right)
            : base(token, left, right)
        {

        }
    }
}
