namespace Language.Lua
{
    public class LuaUserdata : LuaValue
    {
        private object Object;

        public LuaUserdata(object obj)
        {
            Object = obj;
        }

        public LuaUserdata(object obj, LuaTable metatable)
        {
            Object = obj;
            MetaTable = metatable;
        }

        public LuaTable MetaTable { get; set; }

        public override object Value
        {
            get { return Object; }
        }

        public override string GetTypeCode()
        {
            return "userdata";
        }

        public override string ToString()
        {
            return "userdata";
        }
    }
}