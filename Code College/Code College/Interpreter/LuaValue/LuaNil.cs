namespace Language.Lua
{
    public class LuaNil : LuaValue
    {
        public static readonly LuaNil Nil = new LuaNil();

        private LuaNil()
        {
        }

        public override object Value
        {
            get { return null; }
        }

        public override bool GetBooleanValue()
        {
            return false;
        }

        public override string GetTypeCode()
        {
            return "nil";
        }

        public override string ToString()
        {
            return "nil";
        }
    }
}