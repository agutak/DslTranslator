using System.Collections.Generic;

namespace DslTranslator.AstNodes
{
    public class FunctionIdentifierAstNode : IdentifierAstNode
    {
        public FunctionIdentifierAstNode(Token token) : base(token)
        {
        }

        public List<AstNode> ArgumentNodes { get; } = new();
    }
}
