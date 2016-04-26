using Language.Lua.Library;
using Marker;
using System;

namespace Language.Lua
{
    public static class LuaInterpreter
    {
        public static UserCode CodeReport { get; set; }

        // Used for easily adding Constructs to the code report
        public static void AddConStructToCodeReport(string Type, string Condition)
        {
            CodeReport.ControlStructures.Add(new UserCode.ControlStructure { StructureType = Type, StructureCondition = Condition });
        }

        // Creates global environment for the program and registers libraries
        public static LuaTable CreateGlobalEnviroment()
        {
            LuaTable global = new LuaTable();

            BaseLib.RegisterFunctions(global);
            StringLib.RegisterModule(global);
            MathLib.RegisterModule(global);

            global.SetNameValue("_G", global);

            return global;
        }

        // Parses and executes code
        public static LuaValue Interpreter(string UserCode, LuaTable Environment)
        {
            Chunk Chunk = Parse(UserCode);

            Chunk.Environment = Environment;

            return Chunk.Execute();
        }

        // Parses code
        public static Chunk Parse(string UserCode)
        {
            bool Success;
            Parser Parser = new Parser();

            Chunk Chunk = Parser.ParseChunk(new TextInput(UserCode), out Success);

            if (Success)
                return Chunk;
            else
                throw new ArgumentException("Interpreter: Code has syntax errors:\r\n" + Parser.GetErrorMessages());
        }

        // Entry point to interpreter
        // User's code is passed as UserCode string
        public static LuaValue RunCode(string UserCode)
        {
            CodeReport = new UserCode();

            return Interpreter(UserCode, CreateGlobalEnviroment());
        }
    }
}