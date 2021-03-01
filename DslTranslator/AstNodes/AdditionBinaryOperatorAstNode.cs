namespace DslTranslator.AstNodes
{
    public class AdditionBinaryOperatorAstNode : BinaryOperatorAstNode
    {
        public AdditionBinaryOperatorAstNode(Token token, AstNode left, AstNode right)
            : base(token, left, right)
        {

        }
    }
}
