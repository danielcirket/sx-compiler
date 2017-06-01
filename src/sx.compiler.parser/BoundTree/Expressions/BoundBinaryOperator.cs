using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sx.Compiler.Parser.BoundTree.Expressions
{
    public enum BoundBinaryOperator
    {
        #region Assignment

        Assign,
        AddAssign,
        SubAssign,
        MulAssign,
        DivAssign,
        ModAssign,
        AndAssign,
        XorAssign,
        OrAssign,

        #endregion Assignment

        #region Logical

        LogicalOr,
        LogicalAnd,

        #endregion Logical

        #region Equality

        Equal,
        NotEqual,

        #endregion Equality

        #region Relational

        GreaterThan,
        LessThan,
        GreaterThanOrEqual,
        LessThanOrEqual,

        #endregion Relational

        #region Bitwise

        BitwiseAnd,
        BitwiseOr,
        BitwiseXor,

        #endregion Bitwise

        #region Shift

        LeftShift,
        RightShift,

        #endregion Shift

        #region Additive

        Add,
        Sub,

        #endregion Additive

        #region Multiplicative

        Mul,
        Div,
        Mod,

        #endregion Multiplicative
    }
}
