namespace Language.Lua.Library
{
    public static class StringLib
    {
        // Lua function for formatting strings
        public static LuaValue format(LuaValue[] values)
        {
            LuaString format = values[0] as LuaString;
            object[] args = new object[values.Length - 1];
            for (int i = 0; i < args.Length; i++)
            {
                args[i] = values[i + 1].Value;
            }
            return new LuaString(string.Format(format.Text, args));
        }

        // Lua function for finding length of string
        public static LuaValue len(LuaValue[] values)
        {
            LuaString str = values[0] as LuaString;
            return new LuaNumber(str.Text.Length);
        }

        // Registers functions in library
        public static void RegisterFunctions(LuaTable module)
        {
            module.Register("format", format);
            module.Register("len", len);
        }

        // Registers library as module in the environment
        public static void RegisterModule(LuaTable environment)
        {
            LuaTable module = new LuaTable();
            RegisterFunctions(module);
            environment.SetNameValue("string", module);
        }
    }
}