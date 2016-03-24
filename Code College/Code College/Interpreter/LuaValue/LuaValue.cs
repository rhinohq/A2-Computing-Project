using System;

namespace Language.Lua
{
    public abstract class LuaValue : IEquatable<LuaValue>
    {
        public abstract object Value { get; }

        public static LuaValue GetKeyValue(LuaValue baseValue, LuaValue key)
        {
            LuaTable table = baseValue as LuaTable;

            if (table != null)
                return table.GetValue(key);
            else
            {
                LuaUserdata userdata = baseValue as LuaUserdata;

                if (userdata != null)
                {
                    if (userdata.MetaTable != null)
                    {
                        LuaValue index = userdata.MetaTable.GetValue("__index");

                        if (index != null)
                        {
                            LuaFunction func = index as LuaFunction;

                            if (func != null)
                                return func.Invoke(new LuaValue[] { baseValue, key });
                            else
                                return GetKeyValue(index, key);
                        }
                    }
                }

                throw new Exception(string.Format("Interpreter: Access field '{0}' from not a table.", key.Value));
            }
        }

        public bool Equals(LuaValue other)
        {
            if (other == null)
                return false;

            if (this is LuaNil)
                return other is LuaNil;

            if (this is LuaTable && other is LuaTable)
                return ReferenceEquals(this, other);

            return Value.Equals(other.Value);
        }

        public virtual bool GetBooleanValue()
        {
            return true;
        }

        public override int GetHashCode()
        {
            if (this is LuaNumber || this is LuaString)
                return Value.GetHashCode();

            return base.GetHashCode();
        }

        public abstract string GetTypeCode();
    }
}