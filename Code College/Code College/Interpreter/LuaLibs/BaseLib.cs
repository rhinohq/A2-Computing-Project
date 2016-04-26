using System;

namespace Language.Lua.Library
{
    internal class BaseLib
    {
        // Lua assert function
        public static LuaValue assert(LuaValue[] values)
        {
            bool condition = values[0].GetBooleanValue();
            LuaString message = values.Length > 1 ? values[1] as LuaString : null;

            if (message != null)
            {
                throw new LuaError("Interpreter: " + message.Text);
            }
            else
            {
                throw new LuaError("Interpreter: assertion failed!");
            }
        }

        // Lua error function
        public static LuaValue error(LuaValue[] values)
        {
            LuaString message = values[0] as LuaString;
            if (message != null)
            {
                throw new LuaError("Interpreter: " + message.Text);
            }
            else
            {
                throw new LuaError("Interpreter: error raised!");
            }
        }

        public static LuaValue getmetatable(LuaValue[] values)
        {
            LuaTable table = values[0] as LuaTable;

            return table.MetaTable;
        }

        // Lua function for printing to the console
        public static LuaValue print(LuaValue[] values)
        {
            LuaInterpreter.CodeReport.Output += string.Join<LuaValue>("    ", values);

            return null;
        }

        // Registers functions in library
        public static void RegisterFunctions(LuaTable module)
        {
            module.Register("print", print);
            module.Register("type", type);
            module.Register("getmetatable", getmetatable);
            module.Register("setmetatable", setmetatable);
            module.Register("tostring", tostring);
            module.Register("tonumber", tonumber);
            module.Register("assert", assert);
            module.Register("error", error);
        }

        public static LuaValue setmetatable(LuaValue[] values)
        {
            LuaTable table = values[0] as LuaTable;
            LuaTable metatable = values[1] as LuaTable;

            table.MetaTable = metatable;

            return null;
        }

        // Lua function for converting string to a number
        public static LuaValue tonumber(LuaValue[] values)
        {
            LuaString text = values[0] as LuaString;
            if (text != null)
            {
                return new LuaNumber(double.Parse(text.Text));
            }

            LuaString number = values[0] as LuaString;
            if (number != null)
            {
                return number;
            }

            return LuaNil.Nil;
        }

        // Lua function for converting number to a string
        public static LuaValue tostring(LuaValue[] values)
        {
            return new LuaString(values[0].ToString());
        }

        // Lua function that returns the data type of an object
        public static LuaValue type(LuaValue[] values)
        {
            if (values.Length > 0)
            {
                return new LuaString(values[0].GetTypeCode());
            }
            else
            {
                throw new Exception("Interpreter: bad argument #1 to 'type' (value expected)");
            }
        }
    }
}