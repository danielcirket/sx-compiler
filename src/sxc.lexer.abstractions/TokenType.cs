namespace Sxc.Lexer.Abstractions
{
    public enum TokenType
    {
        EOF,
        Error,
        Whitespace,
        NewLine,
        LineComment,
        BlockComment,
        IntegerLiteral,
        StringLiteral,
        RealLiteral,
        Identifier,
        Keyword,
        LeftBracket, // {
        RightBracket, // }
        RightBrace, // ]
        LeftBrace, // [
        LeftParenthesis, // (
        RightParenthesis, // )
        GreaterThanOrEqual, // >=
        GreaterThan, // >
        LessThan, // <
        LessThanOrEqual, // <=
        PlusEqual, // +=
        PlusPlus, // ++
        Plus, // +
        MinusEqual, // -=
        MinusMinus, // --
        Minus, // -
        Assignment, // =
        Not, // !
        NotEqual, // !=
        Mul, // *
        MulEqual, // *=
        Div, // /
        DivEqual, // /=
        BooleanAnd, // &&
        BooleanOr, // ||
        BitwiseAnd, // &
        BitwiseOr, // |
        BitwiseAndEqual, // &=
        BitwiseOrEqual, // |=
        ModEqual, // %=
        Mod, // %
        BitwiseXorEqual, // ^=
        BitwiseXor, // ^
        DoubleQuestion, // ??
        Question, // ?
        Equal, // ==
        BitShiftLeft, // <<
        BitShiftRight, // >>
        Dot,
        Comma,
        Semicolon,
        Colon,
        Arrow, // ->
        FatArrow, // =>
    }
}
