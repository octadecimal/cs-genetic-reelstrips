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
        /// Symbol definitions.
        /// </summary>
        public Symbol[] symbols;


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
            population = GetInt("population");
            precision = GetInt("precision");
            targetRtp = GetFloat("targetRtp");
            targetWinRate = GetFloat("targetWinRate");

            symbols = ParseSymbols();
        }


        // PRIVATE
        // ==============================================================================

        /// <summary>
        /// Parses our symbol definitions.
        /// </summary>
        private Symbol[] ParseSymbols()
        {
            // Create our output
            Symbol[] output = new Symbol[xml["symbols"].ChildNodes.Count];

            // Grab our symbols child nodes
            XmlNode symbolsNode = xml["symbols"];

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

            // Return
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
