using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DslTranslator
{
    public class SymbolTable
    {
        readonly Dictionary<string, SymbolTableEntry> _entries = new Dictionary<string, SymbolTableEntry>();

        public void AddOrUpdate(object variables)
        {
            var kvp = variables
                .GetType()
                .GetProperties()
                .Select(x => (x.Name, Convert.ToDouble(x.GetValue(variables))));

            foreach (var (id, val) in kvp)
            {
                AddOrUpdate(id, val);
            }
        }

        public void AddOrUpdate(string identifier, double value)
        {
            var key = identifier.ToLower();
            if (!_entries.ContainsKey(key))
            {
                _entries.Add(key, new VariableSymbolTableEntry(value, identifier));
            }
            else
            {
                var entry = _entries[key];

                if (entry.Type != EntryType.Variable)
                    throw new Exception($"Identifier {identifier} type mismatch");

                (entry as VariableSymbolTableEntry).Value = value;
            }
        }

        public void AddFunctions(Type type)
        {
            var methods = type
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(mi => typeof(double).IsAssignableFrom(mi.ReturnType))
                .Where(mi => !mi.GetParameters().Any(p =>
                    !p.ParameterType.IsAssignableFrom(typeof(double))));

            foreach (var mi in methods)
            {
                _entries.TryAdd(mi.Name.ToLower(), new FunctionSymbolTableEntry(mi));
            }
        }

        public SymbolTableEntry Get(string identifier)
        {
            var key = identifier.ToLower();
            return _entries.ContainsKey(key) ? _entries[key] : null;
        }
    }
}
