using GeneticSlots.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GeneticSlots
{
    /// <summary>
    /// The environment represents the global world every slot machine exists it.
    /// It contains paramters or ranges that would be global to every slot machine,
    /// including any target final parameters that we may be interested in.
    /// </summary>
    class Environment
    {
        /// <summary>
        /// Population count.
        /// </summary>
        public int population;

        /// <summary>
        /// Simulation precision, the higher value, the more rounds run per machine.
        /// </summary>
        public int precision;

        /// <summary>
        /// Target return to player ratio.
        /// </summary>
        public double targetRtp;

        /// <summary>
        /// Target win rate.
        /// </summary>
        public double targetWinRate;

        /// <summary>
        /// Number of reels.
        /// </summary>
        public int numCols;

        /// <summary>
        /// Number of rows per reel.
        /// </summary>
        public int numRows;

        /// <summary>
        /// Maximum amount a line can pay.
        /// </summary>
        public int maxPay;

        /// <summary>
        /// Minimum number of paying symbols per win line scenario.
        /// </summary>
        public int minPayingSymbols;

        /// <summary>
        /// Minimum reel strip length.
        /// </summary>
        public int minReelStripLength;

        /// <summary>
        /// Maximum reel strip length.
        /// </summary>
        public int maxReelStripLength;

        /// <summary>
        /// Min number of selected symbols per slot machine.
        /// </summary>
        public int minSymbols;

        /// <summary>
        /// Symbol definitions.
        /// </summary>
        public Symbol[] symbols;

        /// <summary>
        /// Payline definitions.
        /// </summary>
        public int[][] lines;


        // Xml document root node.
        private XmlNode xml;

        
        /// <summary>
        /// Environment constructor.
        /// </summary>
        public Environment()
        {
            Load();
            Parse();
        }


        // API
        // ==============================================================================

        /// <summary>
        /// Loads the environment xml data.
        /// </summary>
        public void Load()
        {
            // Create xml document
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = false;

            // Load xml document
            doc.Load("Data\\Environment.xml");

            // Save first node as our root node
            xml = doc.FirstChild;
        }

        /// <summary>
        /// Parses the environment settings.
        /// </summary>
        public void Parse()
        {
            // Parse data values
            population = GetInt("population");
            precision = GetInt("precision");
            targetRtp = GetFloat("targetRtp");
            targetWinRate = GetFloat("targetWinRate");
            numCols = GetInt("numCols");
            numRows = GetInt("numRows");
            maxPay = GetInt("maxPay");
            minPayingSymbols = GetInt("minPayingSymbols");
            minReelStripLength = GetInt("minReelStripLength");
            maxReelStripLength = GetInt("maxReelStripLength");
            minSymbols = GetInt("minSymbols");

            // Parse data structures
            symbols = ParseSymbols();
            lines = ParseLines();
        }


        // PRIVATE
        // ==============================================================================

        /// <summary>
        /// Parses our symbol definitions.
        /// </summary>
        private Symbol[] ParseSymbols()
        {
            // Grab our symbols node
            XmlNode symbolsNode = xml["symbols"];

            // Create our output
            Symbol[] output = new Symbol[symbolsNode.ChildNodes.Count];

            // Loop through each symbol node
            for (int i = 0; i < symbolsNode.ChildNodes.Count; i++)
            {
                // Grab our current symbol node
                XmlNode currentNode = symbolsNode.ChildNodes[i];

                // Parse params
                string name = currentNode.InnerText;
                bool isWild = GetBoolAttr(currentNode, "isWild");
                bool isScatter = GetBoolAttr(currentNode, "isScatter");
                double minBias = GetDoubleAttr(currentNode, "minBias", 1);
                double maxBias = GetDoubleAttr(currentNode, "maxBias", 1);

                // Create symbol
                output[i] = new Symbol(i, name, isWild, isScatter, minBias, maxBias);
            }

            // Return output
            return output;
        }

        /// <summary>
        /// Parses our line definitions.
        /// </summary>
        /// <returns></returns>
        private int[][] ParseLines()
        {
            // Grab our lines node
            XmlNode linesNode = xml["lines"];

            // Create our output
            int[][] output = new int[linesNode.ChildNodes.Count][];

            // Loop through each line node
            for(int i = 0; i < linesNode.ChildNodes.Count; i++)
            {
                // Grab our current line node
                XmlNode currentNode = linesNode.ChildNodes[i];

                // Split our line definition into individual position components
                string[] split = currentNode.InnerText.Split(',');

                // Create our typed line array
                int[] line = new int[split.Length];

                // Convert our chars into ints and insert into typed line array
                for(int j = 0; j < split.Length; j++)
                {
                    line[j] = Convert.ToInt32(split[j]);
                }

                // Save line in output
                output[i] = line;
            }

            // Return output
            return output;
        }


        // UTIL
        // ==============================================================================

        /// <summary>
        /// Gets a value by id and returns it as a 32 bit int.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private int GetInt(string id)
        {
            return Convert.ToInt32(xml[id].InnerText);
        }

        /// <summary>
        /// Gets a value by id and returns it as a 32 bit float.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private float GetFloat(string id)
        {
            return Convert.ToSingle(xml[id].InnerText);
        }

        /// <summary>
        /// Gets a string by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string GetStr(string id)
        {
            return xml[id].InnerText;
        }

        /// <summary>
        /// Gets an attribute by id and returns as a bool.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        private static bool GetBoolAttr(XmlNode node, string attr)
        {
            if (node.Attributes.GetNamedItem(attr) != null)
            {
                return node.Attributes.GetNamedItem(attr).InnerText.ToLower() == "true";
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets an attribute by id and returns as a double.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        private static double GetDoubleAttr(XmlNode node, string attr, double defaultValue)
        {
            if (node.Attributes.GetNamedItem(attr) != null)
            {
                return Convert.ToSingle(node.Attributes.GetNamedItem(attr).InnerText);
            }
            else
            {
                return defaultValue;
            }
        }
    }
}
