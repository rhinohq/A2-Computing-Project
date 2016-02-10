namespace Code_College.Interpreter
{
    public class Interpreter
    {
        public void Interpret(string Code)
        {
            Lua.LuaLexer Lexer = new Lua.LuaLexer();
            Lua.LuaParser Parser = new Lua.LuaParser();

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