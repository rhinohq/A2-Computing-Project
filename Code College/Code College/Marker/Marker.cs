using Language.Lua;
using System.Collections.Generic;

namespace Marker
{
    public static class Marker
    {
        public static ExMarkScheme MarkScheme { get; set; }

        public static bool MarkOutput()
        {
            if (MarkScheme.Output == LuaInterpreter.CodeReport.Output)
                return true;
            else
                return false;
        }

        public static bool MarkVars()
        {
            if (MarkScheme.AssignedVariables.Contains(new ExMarkScheme.Variable { VarName = null, VarValue = null }))
                return true;

            bool Contains, DNM = false;
            List<bool> MarkList = new List<bool>();
            bool[] Marks;

            foreach (UserCode.Variable Var in LuaInterpreter.CodeReport.AssignedVariables)
            {
                Contains = MarkScheme.AssignedVariables.Contains(new ExMarkScheme.Variable { VarName = Var.VarName, VarValue = Var.VarValue });

                if (MarkScheme.AssignedVariables.Contains(new ExMarkScheme.Variable { VarName = Var.VarName, VarValue = null }))
                    DNM = true;
                else if (MarkScheme.AssignedVariables.Contains(new ExMarkScheme.Variable { VarName = null, VarValue = Var.VarValue }))
                    DNM = true;

                if (Contains || DNM)
                    MarkList.Add(true);
                else
                    MarkList.Add(false);
            }

            Marks = MarkList.ToArray();

            foreach (bool Mark in Marks)
            {
                if (!Mark)
                    return false;
            }

            return true;
        }

        public static bool MarkExprs()
        {
            if (MarkScheme.Expressions.Contains(new ExMarkScheme.Expression { ExpressionStr = null, ExpressionType = null }))
                return true;

            bool Contains, DNM = false;
            List<bool> MarkList = new List<bool>();
            bool[] Marks;

            foreach (UserCode.Expression Expr in LuaInterpreter.CodeReport.Expressions)
            {
                Contains = MarkScheme.Expressions.Contains(new ExMarkScheme.Expression { ExpressionStr = Expr.ExpressionStr, ExpressionType = Expr.ExpressionType });

                if (MarkScheme.Expressions.Contains(new ExMarkScheme.Expression { ExpressionStr = Expr.ExpressionStr, ExpressionType = null }))
                    DNM = true;
                else if (MarkScheme.Expressions.Contains(new ExMarkScheme.Expression { ExpressionStr = null, ExpressionType = Expr.ExpressionType }))
                    DNM = true;

                if (Contains || DNM)
                    MarkList.Add(true);
                else
                    MarkList.Add(false);
            }

            Marks = MarkList.ToArray();

            foreach (bool Mark in Marks)
            {
                if (!Mark)
                    return false;
            }

            return true;
        }

        public static bool MarkControlStructs()
        {
            if (MarkScheme.ControlStructures.Contains(new ExMarkScheme.ControlStructure { StructureType = null, StructureCondition = null }))
                return true;

            bool Contains, DNM = false;
            List<bool> MarkList = new List<bool>();
            bool[] Marks;

            foreach (UserCode.ControlStructure ConStruct in LuaInterpreter.CodeReport.ControlStructures)
            {
                Contains = MarkScheme.ControlStructures.Contains(new ExMarkScheme.ControlStructure { StructureType = ConStruct.StructureType, StructureCondition = ConStruct.StructureCondition });

                if (MarkScheme.ControlStructures.Contains(new ExMarkScheme.ControlStructure { StructureType = ConStruct.StructureType, StructureCondition = null }))
                    DNM = true;
                else if (MarkScheme.ControlStructures.Contains(new ExMarkScheme.ControlStructure { StructureType = null, StructureCondition = ConStruct.StructureCondition }))
                    DNM = true;

                if (Contains || DNM)
                    MarkList.Add(true);
                else
                    MarkList.Add(false);
            }

            Marks = MarkList.ToArray();

            foreach (bool Mark in Marks)
            {
                if (!Mark)
                    return false;
            }

            return true;
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