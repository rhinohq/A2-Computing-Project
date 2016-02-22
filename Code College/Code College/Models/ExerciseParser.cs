using Marker;

using System;
using System.IO;
using System.Xml;

namespace Code_College.Models
{
    public static class ExerciseParser
    {
        private static ExDBEntities ExDB = new ExDBEntities();
        private static Exercise NewExercise = new Exercise();
        private static ExMarkScheme NewMarkScheme = new ExMarkScheme();
        private static StreamReader File;

        public static void ParseExFile(string Filename)
        {
            File = new StreamReader(Filename);

            NewExercise.ExID = ExParsing.GetExID();
            File.BaseStream.Position = 0;

            NewExercise.ExTitle = ExParsing.GetExTitle();
            File.BaseStream.Position = 0;

            NewExercise.ExDescription = ExParsing.GetExDescription();
            File.BaseStream.Position = 0;

            NewExercise.ExCodeTemplate = ExParsing.GetExCodeTemplate();
            File.BaseStream.Position = 0;

            NewExercise.ExAppendCode = ExParsing.GetExAppendCode();
            File.BaseStream.Position = 0;

            ExParsing.XMLParsing.ParseXML(ExParsing.GetExMarkSchemeXML());

            NewExercise.ExMarkScheme = NewMarkScheme;

            ExDB.Exercises.Add(NewExercise);
            ExDB.SaveChangesAsync();
        }

        private static class ExParsing
        {
            public static int GetExID()
            {
                while (!File.EndOfStream)
                {
                    string Line = File.ReadLine();

                    if (!Line.StartsWith("[") && Line != "")
                        return Convert.ToInt32(Line);
                }

                return 0;
            }

            public static string GetExTitle()
            {
                while (!File.EndOfStream)
                {
                    string Line = File.ReadLine();

                    if (Line.StartsWith("[ExTitle]") && Line != "")
                        return File.ReadLine();
                }

                return null;
            }

            public static string GetExDescription()
            {
                string Entry = "";

                while (!File.EndOfStream)
                {
                    string Line = File.ReadLine();

                    if (Line.StartsWith("[ExDescription]"))
                    {
                        while (true)
                        {
                            Line = File.ReadLine();

                            if (!Line.StartsWith("[ExCodeTemplate]"))
                                Entry = Entry + " " + Line;
                            else
                                return Entry;
                        }
                    }
                }

                return null;
            }

            public static string GetExCodeTemplate()
            {
                string Entry = "";

                while (!File.EndOfStream)
                {
                    string Line = File.ReadLine();

                    if (Line.StartsWith("[ExCodeTemplate]"))
                    {
                        while (true)
                        {
                            Line = File.ReadLine();

                            if (!Line.StartsWith("[ExAppendCode]"))
                                Entry = Entry + " " + Line;
                            else
                                return Entry;
                        }
                    }
                }

                return null;
            }

            public static string GetExAppendCode()
            {
                string Entry = "";

                while (!File.EndOfStream)
                {
                    string Line = File.ReadLine();

                    if (Line.StartsWith("[ExAppendCode]"))
                    {
                        while (true)
                        {
                            Line = File.ReadLine();

                            if (!Line.StartsWith("[ExMarkScheme]"))
                                Entry = Entry + " " + Line;
                            else
                                return Entry;
                        }
                    }
                }

                return null;
            }

            public static XmlDocument GetExMarkSchemeXML()
            {
                XmlDocument XML = new XmlDocument();
                string Entry = "";
                string Line = "";

                while (!Line.StartsWith("[ExMarkScheme]"))
                    Line = File.ReadLine();

                while (!File.EndOfStream)
                {
                    Line = File.ReadLine();

                    Entry += Line;
                }

                XML.LoadXml(Entry);

                return XML;
            }

            public static class XMLParsing
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
                    try
                    {
                        return XML.SelectSingleNode("/MarkScheme/Output").InnerText;
                    }
                    catch
                    {
                        return null;
                    }
                }

                private static void GetExVars(XmlDocument XML)
                {
                    try
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
                    catch
                    {
                        ExMarkScheme.Variable NewVar = new ExMarkScheme.Variable();

                        NewVar.VarName = null;
                        NewVar.VarValue = null;

                        NewMarkScheme.AssignedVariables.Add(NewVar);
                    }
                }

                private static void GetExExprs(XmlDocument XML)
                {
                    try
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
                    catch
                    {
                        ExMarkScheme.Expression NewExpr = new ExMarkScheme.Expression();

                        NewExpr.ExpressionStr = null;
                        NewExpr.ExpressionType = null;

                        NewMarkScheme.Expressions.Add(NewExpr);
                    }
                }

                private static void GetExConStructs(XmlDocument XML)
                {
                    try
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
                    catch
                    {
                        ExMarkScheme.ControlStructure NewConStruct = new ExMarkScheme.ControlStructure();

                        NewConStruct.StructureCondition = null;
                        NewConStruct.StructureType = null;

                        NewMarkScheme.ControlStructures.Add(NewConStruct);
                    }
                }
            }
        }
    }
}