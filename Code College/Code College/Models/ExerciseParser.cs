using System;
using System.IO;
using System.Xml;

using Marker;

namespace Code_College.Models
{
    public static class ExerciseParser
    {
        private static ExDBEntities ExDB = new ExDBEntities();
        private static Exercise NewExercise = new Exercise();
        private static ExMarkScheme NewMarkScheme = new ExMarkScheme();

        public static void ParseExFile(string Filename)
        {
            StreamReader File = new StreamReader(Filename);

            NewExercise.ExID = ExParsing.GetExID(File);
            NewExercise.ExTitle = ExParsing.GetExTitle(File);
            NewExercise.ExDescription = ExParsing.GetExDescription(File);
            NewExercise.ExCodeTemplate = ExParsing.GetExCodeTemplate(File);
            NewExercise.ExAppendCode = ExParsing.GetExAppendCode(File);

            XMLParsing.ParseXML(ExParsing.GetExMarkSchemeXML(File));

            NewExercise.ExMarkScheme = NewMarkScheme;

            ExDB.SaveChangesAsync();
        }

        private static class ExParsing
        {
            public static int GetExID(StreamReader File)
            {
                while (!File.EndOfStream)
                {
                    string Line = File.ReadLineAsync().ToString();

                    if (Line != "[ExTitle]" && Line != null)
                        return Convert.ToInt32(Line);
                }

                return 0;
            }

            public static string GetExTitle(StreamReader File)
            {
                while (!File.EndOfStream)
                {
                    string Line = File.ReadLineAsync().ToString();
                    string Entry = "";

                    if (Line == "[ExDescription]" && Line == null)
                        return Entry;

                    Entry += Line;
                }

                return null;
            }

            public static string GetExDescription(StreamReader File)
            {
                while (!File.EndOfStream)
                {
                    string Line = File.ReadLineAsync().ToString();
                    string Entry = "";

                    if (Line == "[ExCodeTemplate]")
                        return Entry;

                    Entry += Line;
                }

                return null;
            }

            public static string GetExCodeTemplate(StreamReader File)
            {
                while (!File.EndOfStream)
                {
                    string Line = File.ReadLineAsync().ToString();
                    string Entry = "";

                    if (Line == "[ExAppendCode]")
                        return Entry;

                    Entry += Line;
                }

                return null;
            }

            public static string GetExAppendCode(StreamReader File)
            {
                while (!File.EndOfStream)
                {
                    string Line = File.ReadLineAsync().ToString();
                    string Entry = "";

                    if (Line == "[ExMarkScheme]")
                        return Entry;

                    Entry += Line;
                }

                return null;
            }

            public static XmlDocument GetExMarkSchemeXML(StreamReader File)
            {
                XmlDocument XML = new XmlDocument();

                while (!File.EndOfStream)
                {
                    string Line = File.ReadLineAsync().ToString();
                    string Entry = "";

                    if (Line == "[ExAppendCode]")
                    {
                        XML.LoadXml(Entry);

                        return XML;
                    }

                    Entry += Line;
                }

                return null;
            }
        }

        private static class XMLParsing
        {
            public static void ParseXML(XmlDocument XML)
            {
                NewMarkScheme.Output = GetExOutput(XML);
                GetExVars(XML);
                GetExExprs(XML);
                GetExConStructs(XML);
            }

            private static string GetExOutput(XmlDocument XML)
            {
                return XML.SelectSingleNode("/MarkScheme/Output").InnerText;
            }

            private static void GetExVars(XmlDocument XML)
            {
                XmlNodeList VariablesNode = XML.SelectNodes("/MarkScheme/Variables");

                foreach (XmlNode Node in VariablesNode)
                {
                    ExMarkScheme.Variable NewVar = new ExMarkScheme.Variable();

                    NewVar.VarName = Node.InnerText;
                    NewVar.VarValue = Node.Attributes.GetNamedItem("VarValue").Value;

                    if (NewVar.VarName == "[DNM]")
                        NewVar.VarName = null;
                    else if (NewVar.VarValue == "[DNM]")
                        NewVar.VarValue = null;

                    NewMarkScheme.AssignedVariables.Add(NewVar);
                }
            }

            private static void GetExExprs(XmlDocument XML)
            {
                XmlNodeList ExprsNode = XML.SelectNodes("/MarkScheme/Expressions");

                foreach (XmlNode Node in ExprsNode)
                {
                    ExMarkScheme.Expression NewExpr = new ExMarkScheme.Expression();

                    NewExpr.ExpressionStr = Node.InnerText;
                    NewExpr.ExpressionType = Node.Attributes.GetNamedItem("ExpressionType").Value;

                    if (NewExpr.ExpressionStr == "[DNM]")
                        NewExpr.ExpressionStr = null;
                    else if (NewExpr.ExpressionType == "[DNM]")
                        NewExpr.ExpressionType = null;

                    NewMarkScheme.Expressions.Add(NewExpr);
                }
            }

            private static void GetExConStructs(XmlDocument XML)
            {
                XmlNodeList ConStructsNode = XML.SelectNodes("/MarkScheme/ControlStructures");

                foreach (XmlNode Node in ConStructsNode)
                {
                    ExMarkScheme.ControlStructure NewConStruct = new ExMarkScheme.ControlStructure();

                    NewConStruct.StructureCondition = Node.InnerText;
                    NewConStruct.StructureType = Node.Attributes.GetNamedItem("StructType").Value;

                    if (NewConStruct.StructureCondition == "[DNM]")
                        NewConStruct.StructureCondition = null;
                    else if (NewConStruct.StructureType == "[DNM]")
                        NewConStruct.StructureType = null;

                    NewMarkScheme.ControlStructures.Add(NewConStruct);
                }
            }
        }
    }
}