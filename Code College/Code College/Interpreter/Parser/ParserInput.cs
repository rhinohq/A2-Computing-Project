namespace Language.Lua
{
    public interface ParserInput<T>
    {
        int Length { get; }

        string FormErrorMessage(int position, string message);

        T GetInputSymbol(int pos);

        T[] GetSubSection(int position, int length);

        bool HasInput(int pos);
    }
}