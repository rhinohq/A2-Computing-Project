﻿using System;

namespace Language.Lua.Library
{
    public static class MathLib
    {
        private static Random randomGenerator = new Random();

        // Lua function for finding power of two numbers
        public static LuaValue pow(LuaValue[] values)
        {
            var numbers = CheckArgs2(values);
            return new LuaNumber(Math.Pow(numbers.Item1, numbers.Item2));
        }

        // Lua function for creating random number within limits
        public static LuaValue random(LuaValue[] values)
        {
            if (values.Length == 0)
            {
                return new LuaNumber(randomGenerator.NextDouble());
            }
            else if (values.Length == 1)
            {
                LuaNumber number1 = values[0] as LuaNumber;
                return new LuaNumber(randomGenerator.Next((int)number1.Number));
            }
            else
            {
                var numbers = CheckArgs2(values);
                return new LuaNumber(randomGenerator.Next((int)numbers.Item1, (int)numbers.Item2));
            }
        }

        // Lua function for creating random seed
        public static LuaValue randomseed(LuaValue[] values)
        {
            LuaNumber number = CheckArgs(values);
            randomGenerator = new Random((int)number.Number);
            return number;
        }

        // Registers functions in library
        public static void RegisterFunctions(LuaTable module)
        {
            module.SetNameValue("huge", new LuaNumber(double.MaxValue));
            module.SetNameValue("pi", new LuaNumber(Math.PI));
            module.SetNameValue("e", new LuaNumber(Math.E));
            module.Register("pow", pow);
            module.Register("random", random);
            module.Register("randomseed", randomseed);
            module.Register("sqrt", sqrt);
        }

        // Registers library as module in the environment
        public static void RegisterModule(LuaTable environment)
        {
            LuaTable module = new LuaTable();
            RegisterFunctions(module);
            environment.SetNameValue("math", module);
        }

        // Lua function for finding the square root of a number
        public static LuaValue sqrt(LuaValue[] values)
        {
            LuaNumber number = CheckArgs(values);
            return new LuaNumber(Math.Sqrt(number.Number));
        }

        private static LuaNumber CheckArgs(LuaValue[] values)
        {
            if (values.Length >= 1)
            {
                LuaNumber number = values[0] as LuaNumber;
                if (number != null)
                {
                    return number;
                }
                else
                {
                    throw new LuaError("Interpreter: bad argument #1 to 'abs' (number expected, got {0})", values[0].GetTypeCode());
                }
            }
            else
            {
                throw new LuaError("Interpreter: bad argument #1 to 'abs' (number expected, got no value)");
            }
        }

        private static Tuple<double, double> CheckArgs2(LuaValue[] values)
        {
            if (values.Length >= 2)
            {
                LuaNumber number1 = values[0] as LuaNumber;
                if (number1 == null)
                {
                    throw new LuaError("Interpreter: bad argument #1 to 'abs' (number expected, got {0})", values[0].GetTypeCode());
                }

                LuaNumber number2 = values[1] as LuaNumber;
                if (number2 == null)
                {
                    throw new LuaError("Interpreter: bad argument #2 to 'abs' (number expected, got {0})", values[1].GetTypeCode());
                }

                return Tuple.Create(number1.Number, number2.Number);
            }
            else
            {
                throw new LuaError("Interpreter: bad argument #1 to 'abs' (number expected, got no value)");
            }
        }
    }
}