namespace DslTranslator.AstNodes
{
    public abstract class IdentifierAstNode : AstNode
    {
        public IdentifierAstNode(Token token) : base(token)
        {
            Name = token.Value;
        }

        public string Name { get; }
    }
}
