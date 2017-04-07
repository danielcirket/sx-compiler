using System;

namespace Sx.Compiler.Parser
{
    public static class GenerticExtensions
    {
        public static T Or<T>(this T input, Func<T> evaluator) where T : class
        {
            if (input != null)
                return input;

            return evaluator();
        }
    }
}
