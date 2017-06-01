namespace Sx.Compiler.Parser.Types.Primitives
{
    public class Pointer<T> : Type
    {
        public override string Name => "Pointer";
        public override string FullName => Name;
        public Type Type { get; }
    }
}
