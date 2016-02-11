using System;
using System.IO;
using System.Xml;

namespace Code_College.Models
{
    public static class ExerciseParser
    {
        private static ExDBEntities ExDB = new ExDBEntities();
        private static Exercise NewExercise = new Exercise();

        public static void ParseExFile(string Filename)
        {
            StreamReader File = new StreamReader(Filename);

            NewExercise.ExID = Parsing.GetExID(File);
            NewExercise.ExTitle = Parsing.GetExTitle(File);
            NewExercise.ExDescription = Parsing.GetExDescription(File);
            NewExercise.ExCodeTemplate = Parsing.GetExCodeTemplate(File);
            NewExercise.ExAppendCode = Parsing.GetExAppendCode(File);
            NewExercise.ExMarkScheme = Parsing.GetExMarkScheme(File);
        }

        private class ExParsing
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

            public static XmlDocument GetExMarkScheme(StreamReader File)
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

            private static class XMLParsing
            {

            }
        }
    }
}
