using Language.Lua.Library;
using Marker;
using System;

namespace Language.Lua
{
    public static class LuaInterpreter
    {
        private static Parser Parser = new Parser();
        public static UserCode CodeReport { get; set; }

        public static void AddConStructToCodeReport(string Type, string Condition)
        {
            CodeReport.ControlStructures.Add(new UserCode.ControlStructure { StructureType = Type, StructureCondition = Condition });
        }

        public static LuaTable CreateGlobalEnviroment()
        {
            LuaTable global = new LuaTable();

            BaseLib.RegisterFunctions(global);
            StringLib.RegisterModule(global);
            TableLib.RegisterModule(global);
            MathLib.RegisterModule(global);

            global.SetNameValue("_G", global);

            return global;
        }

        public static LuaValue Interpreter(string UserCode)
        {
            return Interpreter(UserCode, CreateGlobalEnviroment());
        }

        public static LuaValue Interpreter(string UserCode, LuaTable Enviroment)
        {
            Chunk Chunk = Parse(UserCode);

            Chunk.Enviroment = Enviroment;

            return Chunk.Execute();
        }

        public static Chunk Parse(string UserCode)
        {
            bool Success;

            Chunk Chunk = Parser.ParseChunk(new TextInput(UserCode), out Success);

            if (Success)
                return Chunk;
            else
                throw new ArgumentException("Code has syntax errors:\r\n" + Parser.GetErrorMessages());
        }

        public static LuaValue RunCode(string UserCode)
        {
            CodeReport = new UserCode();

            return Interpreter(UserCode);
        }

        public static LuaValue RunCode(string UserCode, LuaTable Enviroment)
        {
            return Interpreter(UserCode, Enviroment);
        }
    }
}