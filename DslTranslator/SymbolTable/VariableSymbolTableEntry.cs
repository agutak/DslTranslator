namespace DslTranslator
{
    public class VariableSymbolTableEntry : SymbolTableEntry
    {
        public VariableSymbolTableEntry(double value, string name)
            : base(name, EntryType.Variable)
        {
            Value = value;
        }

        public double Value { get; set; }
    }
}
