namespace Language.Lua
{
    public class LuaString : LuaValue
    {
        public static readonly LuaString Empty = new LuaString(string.Empty);

        public LuaString(string text)
        {
            Text = text;
        }

        public string Text { get; set; }

        public override object Value
        {
            get { return Text; }
        }

        public override string GetTypeCode()
        {
            return "string";
        }

        public override string ToString()
        {
            return Text;
        }
    }
}