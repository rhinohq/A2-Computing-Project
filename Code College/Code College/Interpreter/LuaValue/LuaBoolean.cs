﻿namespace Language.Lua
{
    public class LuaBoolean : LuaValue
    {
        public static readonly LuaBoolean False = new LuaBoolean { BoolValue = false };

        public static readonly LuaBoolean True = new LuaBoolean { BoolValue = true };

        private LuaBoolean()
        {
        }

        public bool BoolValue { get; set; }

        public override object Value
        {
            get { return BoolValue; }
        }

        public static LuaBoolean From(bool value)
        {
            if (value == true)
                return True;
            else
                return False;
        }

        public override bool GetBooleanValue()
        {
            return BoolValue;
        }

        public override string GetTypeCode()
        {
            return "boolean";
        }

        public override string ToString()
        {
            return BoolValue.ToString().ToLower();
        }
    }
}