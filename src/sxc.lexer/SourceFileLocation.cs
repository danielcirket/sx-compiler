using Sxc.Lexer.Abstractions;

namespace Sxc.Lexer
{
    internal class SourceFileLocation : ISourceFileLocation
    {
        private int _column;
        private int _index;
        private int _lineNo;
        
        public int Column => _column;
        public int Index => _index;
        public int Line => _lineNo;

        public override bool Equals(object obj)
        {
            if (obj is ISourceFileLocation)
                return Equals((ISourceFileLocation)obj);

            return base.Equals(obj);
        }
        public bool Equals(ISourceFileLocation other)
        {
            return other.GetHashCode() == GetHashCode();
        }
        public override int GetHashCode()
        {
            // NOTE(Dan): Unashamedly stolen from System.Tuple
            return (((_index << 5) + _index) ^ _lineNo) ^ _column;
        }

        public SourceFileLocation(int column, int index, int lineNo)
        {
            _column = column;
            _index = index;
            _lineNo = lineNo;
        }
    }
}
