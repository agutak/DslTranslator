namespace DslTranslator.AstNodes
{
    public abstract class BinaryOperatorAstNode : OperatorAstNode
    {
        public BinaryOperatorAstNode(Token token, AstNode left, AstNode right) : base(token)
        {
            Left = left;
            Right = right;
        }

        public AstNode Left { get; }
        public AstNode Right { get; }
    }
}
