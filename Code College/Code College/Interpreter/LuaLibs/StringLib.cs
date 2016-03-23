using System;

namespace Language.Lua.Library
{
    public static class StringLib
    {
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

        public static LuaValue len(LuaValue[] values)
        {
            LuaString str = values[0] as LuaString;
            return new LuaNumber(str.Text.Length);
        }

        public static LuaValue lower(LuaValue[] values)
        {
            LuaString str = values[0] as LuaString;
            return new LuaString(str.Text.ToLower());
        }

        public static void RegisterFunctions(LuaTable module)
        {
            module.Register("format", format);
            module.Register("len", len);
            module.Register("lower", lower);
            module.Register("upper", upper);
            module.Register("reverse", reverse);
        }

        public static void RegisterModule(LuaTable enviroment)
        {
            LuaTable module = new LuaTable();
            RegisterFunctions(module);
            enviroment.SetNameValue("string", module);
        }

        public static LuaValue reverse(LuaValue[] values)
        {
            LuaString str = values[0] as LuaString;
            char[] chars = str.Text.ToCharArray();
            Array.Reverse(chars);
            return new LuaString(new string(chars));
        }

        public static LuaValue upper(LuaValue[] values)
        {
            LuaString str = values[0] as LuaString;
            return new LuaString(str.Text.ToUpper());
        }
    }
}