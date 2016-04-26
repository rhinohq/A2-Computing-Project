using Language.Lua;
using System.Collections.Generic;

namespace Marker
{
    public static class Marker
    {
        public static ExMarkScheme MarkScheme { get; set; }
        
        public static bool FullMark()
        {
            List<bool> Marks = new List<bool>();

            if (MarkScheme.CheckOutput)
                Marks.Add(MarkOutput());
            else if (MarkScheme.CheckVars)
                Marks.Add(MarkVars());
            else if (MarkScheme.CheckConStruct)
                Marks.Add(MarkControlStructs());
            else
                return true;

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
            List<bool> Marks = new List<bool>();

            foreach (UserCode.ControlStructure ConStruct in LuaInterpreter.CodeReport.ControlStructures)
            {
                Contains = MarkScheme.ControlStructures.Contains(new ExMarkScheme.ControlStructure { StructureType = ConStruct.StructureType, StructureCondition = ConStruct.StructureCondition });

                if (MarkScheme.ControlStructures.Contains(new ExMarkScheme.ControlStructure { StructureType = ConStruct.StructureType, StructureCondition = null }))
                    DNM = true;
                else if (MarkScheme.ControlStructures.Contains(new ExMarkScheme.ControlStructure { StructureType = null, StructureCondition = ConStruct.StructureCondition }))
                    DNM = true;

                if (Contains || DNM)
                    Marks.Add(true);
                else
                    Marks.Add(false);
            }

            foreach (bool Mark in Marks)
            {
                if (!Mark)
                    return false;
            }

            return true;
        }

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
            List<bool> Marks = new List<bool>();

            foreach (UserCode.Variable Var in LuaInterpreter.CodeReport.AssignedVariables)
            {
                Contains = MarkScheme.AssignedVariables.Contains(new ExMarkScheme.Variable { VarName = Var.VarName, VarValue = Var.VarValue });

                if (MarkScheme.AssignedVariables.Contains(new ExMarkScheme.Variable { VarName = Var.VarName, VarValue = null }))
                    DNM = true;
                else if (MarkScheme.AssignedVariables.Contains(new ExMarkScheme.Variable { VarName = null, VarValue = Var.VarValue }))
                    DNM = true;

                if (Contains || DNM)
                    Marks.Add(true);
                else
                    Marks.Add(false);
            }

            foreach (bool Mark in Marks)
            {
                if (!Mark)
                    return false;
            }

            return true;
        }
    }
}