using System;
using System.Collections.Generic;

namespace Marker
{
    public abstract class Code
    {
        public class Variable : IEquatable<Variable>
        {
            public string VarName { get; set; }
            public string VarValue { get; set; }

            public bool Equals(Variable other)
            {
                throw new NotImplementedException();
            }
        }

        public class ControlStructure : IEquatable<ControlStructure>
        {
            public string StructureType { get; set; }
            public string StructureCondition { get; set; }

            public bool Equals(ControlStructure other)
            {
                throw new NotImplementedException();
            }
        }

        public string Output { get; set; }

        public List<Variable> AssignedVariables = new List<Variable>();
        public List<ControlStructure> ControlStructures = new List<ControlStructure>();
    }

    public class ExMarkScheme : Code
    {
        public bool CheckOutput { get; set; }
        public bool CheckVars { get; set; }
        public bool CheckConStruct { get; set; }
    }

    public class UserCode : Code
    {
        
    }
}