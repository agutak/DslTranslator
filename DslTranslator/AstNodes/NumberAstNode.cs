using System.Globalization;

namespace DslTranslator.AstNodes
{
    public class NumberAstNode : AstNode
    {
        public NumberAstNode(Token token) : base(token) { }

        public double Value => double.Parse(Token.Value, CultureInfo.InvariantCulture);
    }
}
