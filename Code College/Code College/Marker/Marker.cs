using System.Collections.Generic;

using Language.Lua;

namespace Marker
{
    public static class Marker
    {
        public static ExMarkScheme MarkScheme { get; set; }

        public static bool MarkOutput()
        {
            if (MarkScheme.Output == LuaInterpreter.CodeReport.Output)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool MarkVars()
        {
            foreach (UserCode.Variable Var in LuaInterpreter.CodeReport.AssignedVariables)
            {
                if (MarkScheme.AssignedVariables.Contains(new ExMarkScheme.Variable { VarName = Var.VarName, VarValue = Var.VarValue }))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool MarkExprs()
        {
            foreach (UserCode.Expression Expr in LuaInterpreter.CodeReport.Expressions)
            {
                if (MarkScheme.Expressions.Contains(new ExMarkScheme.Expression { ExpressionStr = Expr.ExpressionStr, ExpressionType = Expr.ExpressionType }))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool MarkControlStructs()
        {
            foreach (UserCode.ControlStructure ConStruct in LuaInterpreter.CodeReport.ControlStructures)
            {
                if (MarkScheme.ControlStructures.Contains(new ExMarkScheme.ControlStructure { StructureType = ConStruct.StructureType, StructureCondition = ConStruct.StructureCondition }))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool FullMark()
        {
            List<bool> MarkList = new List<bool>();

            if (MarkScheme.CheckOutput)
                MarkList.Add(MarkOutput());
            else if (MarkScheme.CheckVars)
                MarkList.Add(MarkVars());
            else if (MarkScheme.CheckExprs)
                MarkList.Add(MarkExprs());
            else if (MarkScheme.CheckConStruct)
                MarkList.Add(MarkControlStructs());
            else
                return false;

            bool[] Marks = MarkList.ToArray();

            foreach (bool Mark in Marks)
            {
                if (!Mark)
                    return false;
            }

            return true;
        }
    }
}