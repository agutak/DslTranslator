using System.Collections.Generic;

namespace DslTranslator
{
    public class SourceScanner
    {
        private readonly Stack<int> _positionStack;
        private readonly string _buffer;

        public SourceScanner(string expression)
        {
            _positionStack = new Stack<int>();

            _buffer = expression;
        }

        public int Position { get; private set; }
        public bool EndOfSource => Position >= _buffer.Length;

        public char? Read() => EndOfSource ? (char?)null : _buffer[Position++];

        public char? Peek()
        {
            Push();
            var next = Read();
            Pop();
            return next;
        }

        public void Push() => _positionStack.Push(Position);

        public void Pop() => Position = _positionStack.Pop();
    }
}
