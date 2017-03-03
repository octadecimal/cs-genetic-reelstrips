using cs_genetic_reelstrips.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace cs_genetic_reelstrips
{
    class Settings
    {
        public static Random rng;

        public static int population;
        public static int precision;
        public static int numCols;
        public static int numRows;
        public static int minPayingSymbols;
        public static int minReelStripLength;
        public static int maxReelStripLength;
        public static float minWildBias;
        public static float maxWildBias;
        public static float minScatterBias;
        public static float maxScatterBias;

        public static Symbol[] symbols;
        public static int[][] lines;

        private static XmlNode xml;

        public static void LoadXml(string filename)
        {
            rng = new Random();

            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = false;
            doc.Load("Data\\Settings.xml");

            xml = doc.FirstChild;

            Parse();
        }

        private static void Parse()
        {
            population = GetInt("population");
            precision = GetInt("precision");
            numCols = GetInt("numCols");
            numRows = GetInt("numRows");
            minPayingSymbols = GetInt("minPayingSymbols");
            minReelStripLength = GetInt("minReelStripLength");
            maxReelStripLength = GetInt("maxReelStripLength");
            minWildBias = GetFloat("minWildBias");
            maxWildBias = GetFloat("maxWildBias");
            minScatterBias = GetFloat("minScatterBias");
            maxScatterBias = GetFloat("maxScatterBias");

            symbols = ParseSymbols();
            lines = ParseLines();
        }

        private static Symbol[] ParseSymbols()
        {
            Symbol[] output = new Symbol[xml["symbols"].ChildNodes.Count];

            int count = 0;

            foreach(XmlNode node in xml["symbols"].ChildNodes)
            {
                string name = node.InnerText;
                bool isWild = GetBoolAttr(node, "isWild");
                bool isScatter = GetBoolAttr(node, "isScatter");

                Console.WriteLine("Symbol: " + name + "\tisWild=" + isWild + "\tisScatter=" + isScatter);

                Symbol symbol = new Symbol(count, name, isWild, isScatter);

                output[count] = symbol;

                count++;
            }

            return output;
        }

        private static int[][] ParseLines()
        {
            int[][] output = new int[xml["lines"].ChildNodes.Count][];
            int count = 0;

            foreach (XmlNode node in xml["lines"].ChildNodes)
            {
                string[] split = node.InnerText.Split(',');
                int[] line = new int[split.Length];

                for(int i = 0; i < split.Length; i++)
                {
                    line[i] = Convert.ToInt32(split[i]);
                }

                output[count] = line;

                count++;
            }

            return output;
        }

        private static int GetInt(string id)
        {
            return Convert.ToInt32(xml[id].InnerText);
        }

        private static float GetFloat(string id)
        {
            return Convert.ToSingle(xml[id].InnerText);
        }

        private static string GetStr(string id)
        {
            return xml[id].InnerText;
        }

        private static bool GetBoolAttr(XmlNode node, string attr)
        {
            if(node.Attributes.GetNamedItem(attr) != null)
            {
                return node.Attributes.GetNamedItem(attr).InnerText.ToLower() == "true";
            }
            else
            {
                return false;
            }
        }

        //public static Symbol GetRandomSymbol()
        //{
            //return symbols[rng.Next(0, symbols.Length)];
        //}
    }
}
