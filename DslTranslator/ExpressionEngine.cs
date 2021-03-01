using DslTranslator.AstNodes;
using System;
using System.Linq;

namespace DslTranslator
{
    public class ExpressionEngine
    {
        private readonly SymbolTable _symbolTable = new SymbolTable();

        public void AddFunctions(Type type) => _symbolTable.AddFunctions(type);

        public double Evaluate(string expression, object variables)
        {
            _symbolTable.AddOrUpdate(variables);

            return Evaluate(expression);
        }

        public double Evaluate(string expression)
        {
            var astRoot = new Parser(new Lexer(new SourceScanner(expression)), _symbolTable).Parse();

            return Evaluate(astRoot as dynamic);
        }

        private double Evaluate(NumberAstNode node) => node.Value;

        private double Evaluate(AdditionBinaryOperatorAstNode node) =>
            Evaluate(node.Left as dynamic) + Evaluate(node.Right as dynamic);

        private double Evaluate(SubtractionBinaryOperatorAstNode node) =>
            Evaluate(node.Left as dynamic) - Evaluate(node.Right as dynamic);

        private double Evaluate(MultiplicationBinaryOperatorAstNode node) =>
            Evaluate(node.Left as dynamic) * Evaluate(node.Right as dynamic);

        private double Evaluate(DivisionBinaryOperatorAstNode node) =>
            Evaluate(node.Left as dynamic) / Evaluate(node.Right as dynamic);

        private double Evaluate(NegationUnaryOperatorAstNode node) =>
            -1 * Evaluate(node.Target as dynamic);

        private double Evaluate(FactorialUnaryOperatorAstNode node)
        {
            int Fact(int x) => x == 1 ? 1 : x * Fact(x - 1);
            var value = (int)Evaluate(node.Target as dynamic);
            if (value < 0)
                throw new Exception("Only positive integers allowed.");
            return Fact(value);
        }

        private double Evaluate(ExponentBinaryOperatorAstNode node) =>
            Math.Pow(Evaluate(node.Left as dynamic), Evaluate(node.Right as dynamic));

        private double Evaluate(VariableIdentifierAstNode node)
        {
            var entry = _symbolTable.Get(node.Name);

            if (entry is null || entry.Type != EntryType.Variable)
                throw new Exception($"Error evaluating expression. Variable {node.Name}");

            return (entry as VariableSymbolTableEntry).Value;
        }

        private double Evaluate(FunctionIdentifierAstNode node)
        {
            var entry = _symbolTable.Get(node.Name);

            if (entry is null || entry.Type != EntryType.Function)
                throw new Exception($"Error evaluating expression. Function {node.Name}");

            return 
                (double)
                (entry as FunctionSymbolTableEntry)
                .MethodInfo
                .Invoke(null, node.ArgumentNodes.Select(arg => Evaluate(arg as dynamic)).ToArray());
        }
    }
}
