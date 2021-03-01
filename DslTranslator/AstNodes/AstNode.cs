namespace DslTranslator.AstNodes
{
    public abstract class AstNode
    {
        public AstNode(Token token)
        {
            Token = token;
        }

        public Token Token { get; private set; }
    }
}
