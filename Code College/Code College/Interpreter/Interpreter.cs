using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Code_College.Interpreter
{
    public class Interpreter
    {
        public void Interpret(string Code)
        {
            Lua.LuaLexer Lexer = new Lua.LuaLexer();

            string[,] Tokens = Lexer.LuaLex(Code);
        }

        public bool Mark(UserCode UserCode, ExMarkScheme MarkScheme)
        {
            Marker Marker = new Marker();

            Marker.Code = UserCode;
            Marker.MarkScheme = MarkScheme;

            return Marker.FullMark();
        }
    }
}