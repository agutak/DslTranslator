namespace DslTranslator
{
    public abstract class SymbolTableEntry
    {
        protected SymbolTableEntry(string name, EntryType entryType)
        {
            IdentifierName = name;
            Type = entryType;
        }

        public string IdentifierName { get; }
        public EntryType Type { get; }
    }
}
