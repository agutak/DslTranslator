using System.Reflection;

namespace DslTranslator
{
    public class FunctionSymbolTableEntry : SymbolTableEntry
    {
        public FunctionSymbolTableEntry(MethodInfo methodInfo)
            : base(methodInfo.Name, EntryType.Function)
        {
            MethodInfo = methodInfo;
        }

        public MethodInfo MethodInfo { get; set; }
    }
}
