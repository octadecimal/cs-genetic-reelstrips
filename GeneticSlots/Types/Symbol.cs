using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticSlots.Types
{
    class Symbol
    {
        /// <summary>
        /// Unique symbol identifier.
        /// </summary>
        public int id;

        /// <summary>
        /// Human-readable name for this symbol.
        /// </summary>
        public string name;

        /// <summary>
        /// Denotes if this is a wild symbol.
        /// </summary>
        public bool isWild = false;

        /// <summary>
        /// Denotes if this is a scatter symbol.
        /// </summary>
        public bool isScatter = false;

        /// <summary>
        /// Contributing multiplier when part of a win.
        /// </summary>
        public int multiplier = 1;

        /// <summary>
        /// Minimum selection bias for this symbol.
        /// </summary>
        public double minBias = 1.0;

        /// <summary>
        /// Maximum selection bias for this symbol.
        /// </summary>
        public double maxBias = 1.0;


        /// <summary>
        /// Symbol constructor.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="isWild"></param>
        /// <param name="isScatter"></param>
        /// <param name="minBias"></param>
        /// <param name="maxBias"></param>
        public Symbol(int id, string name, bool isWild, bool isScatter, double minBias, double maxBias)
        {
            this.id = id;
            this.name = name;
            this.isWild = isWild;
            this.isScatter = isScatter;
            this.minBias = minBias;
            this.maxBias = maxBias;
        }
    }
}
