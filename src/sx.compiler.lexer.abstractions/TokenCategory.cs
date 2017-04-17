namespace Sx.Lexer.Abstractions
{
    public enum TokenCategory
    {
        Unknown,
        Whitespace,
        Comment,
        Constant,
        Grouping,
        Punctuation,
        Operator,
        Invalid,
        Identifier
    }
}
