using System.Collections.Generic;

namespace Marker
{
    public class Marker
    {
        public ExMarkScheme MarkScheme { get; set; }
        public UserCode Code { get; set; }

        public bool MarkOutput()
        {
            if (MarkScheme.Output == Code.Output)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool MarkVars()
        {
            foreach (UserCode.Variable Var in Code.AssignedVariables)
            {
                if (MarkScheme.AssignedVariables.Contains(new ExMarkScheme.Variable { VarName = Var.VarName, VarType = Var.VarType, VarValue = Var.VarValue }))
                {
                    return true;
                }
            }

            return false;
        }

        public bool MarkExprs()
        {
            foreach (UserCode.Expression Expr in Code.Expressions)
            {
                if (MarkScheme.Expressions.Contains(new ExMarkScheme.Expression { ExpressionStr = Expr.ExpressionStr, ExpressionType = Expr.ExpressionType }))
                {
                    return true;
                }
            }

            return false;
        }

        public bool MarkControlStructs()
        {
            foreach (UserCode.ControlStructure ConStruct in Code.ControlStructures)
            {
                if (MarkScheme.ControlStructures.Contains(new ExMarkScheme.ControlStructure { StructureType = ConStruct.StructureType, StrutureCondition = ConStruct.StrutureCondition }))
                {
                    return true;
                }
            }

            return false;
        }

        public bool FullMark()
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