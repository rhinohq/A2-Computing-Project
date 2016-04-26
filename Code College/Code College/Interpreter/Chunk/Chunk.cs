using Marker;
using System;

namespace Language.Lua
{
    public partial class Assignment : Statement
    {
        public override LuaValue Execute(LuaTable environment, out bool isBreak)
        {
            LuaValue[] values = ExprList.ConvertAll(expr => expr.Evaluate(environment)).ToArray();
            LuaValue[] neatValues = LuaMultiValue.UnWrapLuaValues(values);

            for (int i = 0; i < Math.Min(VarList.Count, neatValues.Length); i++)
            {
                Var var = VarList[i];

                if (var.Accesses.Count == 0)
                {
                    VarName varName = var.Base as VarName;

                    if (varName != null)
                    {
                        SetKeyValue(environment, new LuaString(varName.Name), values[i]);

                        LuaInterpreter.CodeReport.AssignedVariables.Add(new UserCode.Variable { VarName = varName.Name, VarValue = values[i].Value.ToString() });

                        continue;
                    }
                }
                else
                {
                    LuaValue baseValue = var.Base.Evaluate(environment);

                    for (int j = 0; j < var.Accesses.Count - 1; j++)
                    {
                        Access access = var.Accesses[j];

                        baseValue = access.Evaluate(baseValue, environment);
                    }

                    Access lastAccess = var.Accesses[var.Accesses.Count - 1];

                    NameAccess nameAccess = lastAccess as NameAccess;
                    if (nameAccess != null)
                    {
                        SetKeyValue(baseValue, new LuaString(nameAccess.Name), values[i]);
                        continue;
                    }

                    KeyAccess keyAccess = lastAccess as KeyAccess;
                    if (lastAccess != null)
                    {
                        SetKeyValue(baseValue, keyAccess.Key.Evaluate(environment), values[i]);
                    }
                }
            }

            isBreak = false;
            return null;
        }

        private static void SetKeyValue(LuaValue baseValue, LuaValue key, LuaValue value)
        {
            LuaValue newIndex = LuaNil.Nil;
            LuaTable table = baseValue as LuaTable;
            if (table != null)
            {
                if (table.ContainsKey(key))
                {
                    table.SetKeyValue(key, value);
                    return;
                }
                else
                {
                    if (table.MetaTable != null)
                    {
                        newIndex = table.MetaTable.GetValue("__newindex");
                    }

                    if (newIndex == LuaNil.Nil)
                    {
                        table.SetKeyValue(key, value);
                        return;
                    }
                }
            }
            else
            {
                LuaUserdata userdata = baseValue as LuaUserdata;
                if (userdata != null)
                {
                    if (userdata.MetaTable != null)
                    {
                        newIndex = userdata.MetaTable.GetValue("__newindex");
                    }

                    if (newIndex == LuaNil.Nil)
                    {
                        throw new Exception("Interpreter: Assign field of userdata without __newindex defined.");
                    }
                }
            }

            LuaFunction func = newIndex as LuaFunction;
            if (func != null)
            {
                func.Invoke(new LuaValue[] { baseValue, key, value });
            }
            else
            {
                SetKeyValue(newIndex, key, value);
            }
        }
    }

    public partial class BreakStmt : Statement
    {
        public override LuaValue Execute(LuaTable environment, out bool isBreak)
        {
            throw new NotImplementedException();
        }
    }

    public partial class Chunk
    {
        public LuaTable Environment;

        public LuaValue Execute()
        {
            bool isBreak;
            return Execute(out isBreak);
        }

        public LuaValue Execute(LuaTable environment, out bool isBreak)
        {
            Environment = new LuaTable(environment);
            return Execute(out isBreak);
        }

        public LuaValue Execute(out bool isBreak)
        {
            foreach (Statement statement in Statements)
            {
                ReturnStmt returnStmt = statement as ReturnStmt;
                if (returnStmt != null)
                {
                    isBreak = false;
                    return LuaMultiValue.WrapLuaValues(returnStmt.ExprList.ConvertAll(expr => expr.Evaluate(Environment)).ToArray());
                }
                else if (statement is BreakStmt)
                {
                    isBreak = true;
                    return null;
                }
                else
                {
                    var returnValue = statement.Execute(Environment, out isBreak);
                    if (returnValue != null || isBreak == true)
                    {
                        return returnValue;
                    }
                }
            }

            isBreak = false;
            return null;
        }
    }

    public partial class DoStmt : Statement
    {
        public override LuaValue Execute(LuaTable environment, out bool isBreak)
        {
            return Body.Execute(environment, out isBreak);
        }
    }

    public partial class ExprStmt : Statement
    {
        public override LuaValue Execute(LuaTable environment, out bool isBreak)
        {
            Expr.Evaluate(environment);
            isBreak = false;
            return null;
        }
    }

    public partial class ForInStmt : Statement
    {
        public override LuaValue Execute(LuaTable environment, out bool isBreak)
        {
            LuaValue[] values = ExprList.ConvertAll(expr => expr.Evaluate(environment)).ToArray();
            LuaValue[] neatValues = LuaMultiValue.UnWrapLuaValues(values);

            LuaFunction func = neatValues[0] as LuaFunction;
            LuaValue state = neatValues[1];
            LuaValue loopVar = neatValues[2];

            var table = new LuaTable(environment);
            Body.Environment = table;

            while (true)
            {
                LuaValue result = func.Invoke(new LuaValue[] { state, loopVar });
                LuaMultiValue multiValue = result as LuaMultiValue;

                if (multiValue != null)
                {
                    neatValues = LuaMultiValue.UnWrapLuaValues(multiValue.Values);
                    loopVar = neatValues[0];

                    for (int i = 0; i < Math.Min(NameList.Count, neatValues.Length); i++)
                    {
                        table.SetNameValue(NameList[i], neatValues[i]);
                    }
                }
                else
                {
                    loopVar = result;
                    table.SetNameValue(NameList[0], result);
                }

                if (loopVar == LuaNil.Nil)
                {
                    break;
                }

                var returnValue = Body.Execute(out isBreak);
                if (returnValue != null || isBreak == true)
                {
                    isBreak = false;
                    return returnValue;
                }
            }

            isBreak = false;
            return null;
        }
    }

    public partial class ForStmt : Statement
    {
        public override LuaValue Execute(LuaTable environment, out bool isBreak)
        {
            LuaNumber start = Start.Evaluate(environment) as LuaNumber;
            LuaNumber end = End.Evaluate(environment) as LuaNumber;

            double step = 1;
            if (Step != null)
            {
                step = (Step.Evaluate(environment) as LuaNumber).Number;
            }

            var table = new LuaTable(environment);
            table.SetNameValue(VarName, start);
            Body.Environment = table;

            while (step > 0 && start.Number <= end.Number ||
                   step <= 0 && start.Number >= end.Number)
            {
                var returnValue = Body.Execute(out isBreak);
                if (returnValue != null || isBreak == true)
                {
                    isBreak = false;
                    return returnValue;
                }
                start.Number += step;
            }

            isBreak = false;
            return null;
        }
    }

    public partial class Function : Statement
    {
        public override LuaValue Execute(LuaTable environment, out bool isBreak)
        {
            LuaTable table = environment;

            if (Name.MethodName == null)
            {
                for (int i = 0; i < Name.FullName.Count - 1; i++)
                {
                    LuaValue obj = environment.GetValue(Name.FullName[i]);
                    table = obj as LuaTable;

                    if (table == null)
                    {
                        throw new Exception("Interpreter: Not a table: " + Name.FullName[i]);
                    }
                }

                table.SetNameValue(
                    Name.FullName[Name.FullName.Count - 1],
                    Body.Evaluate(environment));
            }
            else
            {
                for (int i = 0; i < Name.FullName.Count; i++)
                {
                    LuaValue obj = environment.GetValue(Name.FullName[i]);
                    table = obj as LuaTable;

                    if (table == null)
                    {
                        throw new Exception("Interpreter: Not a table " + Name.FullName[i]);
                    }
                }

                Body.ParamList.NameList.Insert(0, "self");

                table.SetNameValue(
                    Name.MethodName,
                    Body.Evaluate(environment));
            }

            isBreak = false;
            return null;
        }
    }

    public partial class IfStmt : Statement
    {
        public override LuaValue Execute(LuaTable environment, out bool isBreak)
        {
            LuaValue condition = Condition.Evaluate(environment);

            if (condition.GetBooleanValue() == true)
            {
                return ThenBlock.Execute(environment, out isBreak);
            }
            else
            {
                foreach (ElseifBlock elseifBlock in ElseifBlocks)
                {
                    condition = elseifBlock.Condition.Evaluate(environment);

                    if (condition.GetBooleanValue() == true)
                    {
                        return elseifBlock.ThenBlock.Execute(environment, out isBreak);
                    }
                }

                if (ElseBlock != null)
                {
                    return ElseBlock.Execute(environment, out isBreak);
                }
            }

            isBreak = false;
            return null;
        }
    }

    public partial class LocalFunc : Statement
    {
        public override LuaValue Execute(LuaTable environment, out bool isBreak)
        {
            environment.SetNameValue(Name, Body.Evaluate(environment));
            isBreak = false;
            return null;
        }
    }

    public partial class LocalVar : Statement
    {
        public override LuaValue Execute(LuaTable environment, out bool isBreak)
        {
            LuaValue[] values = ExprList.ConvertAll(expr => expr.Evaluate(environment)).ToArray();
            LuaValue[] neatValues = LuaMultiValue.UnWrapLuaValues(values);

            for (int i = 0; i < Math.Min(NameList.Count, neatValues.Length); i++)
            {
                environment.RawSetValue(NameList[i], neatValues[i]);
            }

            if (neatValues.Length < NameList.Count)
            {
                for (int i = neatValues.Length; i < NameList.Count - neatValues.Length; i++)
                {
                    environment.RawSetValue(NameList[i], LuaNil.Nil);
                }
            }

            isBreak = false;
            return null;
        }
    }

    public partial class RepeatStmt : Statement
    {
        public override LuaValue Execute(LuaTable environment, out bool isBreak)
        {
            while (true)
            {
                var returnValue = Body.Execute(environment, out isBreak);
                if (returnValue != null || isBreak == true)
                {
                    isBreak = false;
                    return returnValue;
                }

                LuaValue condition = Condition.Evaluate(environment);

                if (condition.GetBooleanValue() == true)
                {
                    break;
                }
            }

            return null;
        }
    }

    public partial class ReturnStmt : Statement
    {
        public override LuaValue Execute(LuaTable environment, out bool isBreak)
        {
            throw new NotImplementedException();
        }
    }

    public abstract partial class Statement
    {
        public abstract LuaValue Execute(LuaTable environment, out bool isBreak);
    }

    public partial class WhileStmt : Statement
    {
        public override LuaValue Execute(LuaTable environment, out bool isBreak)
        {
            while (true)
            {
                LuaValue condition = Condition.Evaluate(environment);

                if (condition.GetBooleanValue() == false)
                {
                    break;
                }

                var returnValue = Body.Execute(environment, out isBreak);
                if (returnValue != null || isBreak == true)
                {
                    isBreak = false;
                    return returnValue;
                }
            }

            isBreak = false;
            return null;
        }
    }
}