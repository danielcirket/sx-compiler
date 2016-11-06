using System;
using Sx.Lexer.Abstractions;

namespace Sx.Lexer
{
    public class Token : IToken
    {
        private TokenCategory _category;
        private TokenType _type;
        private ISourceFilePart _sourceFilePart;
        private string _value;

        public TokenCategory Category => GetCategory();
        public ISourceFilePart SourceFilePart => _sourceFilePart;
        public TokenType TokenType => _type;
        public string Value => _value;

        public TokenCategory GetCategory()
        {
            return TokenCategory.Unknown;
        }
        public bool Equals(IToken other)
        {
            throw new NotImplementedException();
        }
        public override string ToString()
        {
            return $@"Start: (Line){_sourceFilePart.Start.Line} (Col){_sourceFilePart.Start.Column}, Type: {TokenType}, Value: {Value}";
        }

        public Token(TokenType tokenType, string content, ISourceFileLocation startSourceLocation, ISourceFileLocation endSourceLocation)
        {
            _type = tokenType;
            _sourceFilePart = new SourceFilePart(startSourceLocation, endSourceLocation, content.Split('\n'));
            _value = content;
        }
    }
}
