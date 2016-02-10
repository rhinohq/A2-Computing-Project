// This is a combinator. A combinator is a function which produces a parser as its output, usually after taking one or more parsers as input.

namespace Code_College.Interpreter
{
    internal class Result
    {
        public string Token { get; set; }
        public int Pos { get; set; }
        
        public string Repr()
        {
            return "Result(" + Token + ", " + Pos.ToString() + ")";
        }
    }

    // The class that defines the parsers. Parsers are used to create the AST.
    internal class Parser
    {
        public string Call(string[,] Tokens, int Pos)
        {
            return null;
        }

        public Concat Add()
        {
            Concat concat = new Concat();

            return concat;
        }

        public Exp Mul()
        {
            Exp exp = new Exp();

            return exp;
        }

        public Alternate Or()
        {
            Alternate alternate = new Alternate();

            return alternate;
        }

        public Process Xor()
        {
            Process process = new Process();

            return process;
        }
    }

    internal class Tag : Parser
    {
        public string TokenTag { get; set; }

        public string Call(string[,] Tokens, int Pos)
        {
            if (Pos < Tokens.Length && Tokens[Pos, 0] == TokenTag)
            {
                Result result = new Result();

                result.Token = Tokens[Pos, 0];
                result.Pos = Pos++;

                return result.Repr();
            }
            else
                return null;
        }
    }

    // Reserved will be used to parse reserved words and operators; it will accept tokens with a specific value and tag.
    internal class Reserved : Parser
    {
        public string Value { get; set; }
        public string Tag { get; set; }

        public string Call(string[,] Tokens, int Pos)
        {
            if (Pos < Tokens.Length && Tokens[Pos, 0] == Value && Tokens[Pos, 1] == Tag)
            {
                Result result = new Result();

                result.Token = Tokens[Pos, 0];
                result.Pos = Pos++;

                return result.Repr();
            }
            else
                return null;
        }
    }

    internal class Concat : Parser
    {
        public Tag Left { get; set; }
        public Reserved Right { get; set; }
    }

    internal class Exp : Parser
    {
        public Alternate Parser { get; set; }
        public Process Separator { get; set; }
    }

    internal class Alternate : Parser
    {
        public Reserved Left { get; set; }
        public Reserved Right { get; set; }
    }

    internal class Opt : Parser
    {
        public Concat Parser { get; set; }
    }

    internal class Rep : Parser
    {
        public Concat Parser { get; set; }
    }

    internal class Process : Parser
    {
        public Concat Parser { get; set; }
    }

    internal class Lazy : Parser
    {
    }

    internal class Phrase : Parser
    {
        public Exp Parser { get; set; }
    }
}