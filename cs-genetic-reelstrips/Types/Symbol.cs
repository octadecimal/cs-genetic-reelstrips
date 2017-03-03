using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_genetic_reelstrips.Types
{
    /// <summary>
    /// Denotes an individual symbol.
    /// </summary>
    class Symbol
    {
        /// <summary>
        /// Symbol id.
        /// </summary>
        public int id;

        /// <summary>
        /// Symbol name identifier.
        /// </summary>
        public string name;

        /// <summary>
        /// Is this symbol is a wild substitution symbol.
        /// </summary>
        public bool isWild = false;

        /// <summary>
        /// If this symbol is a scatter symbol.
        /// </summary>
        public bool isScatter = false;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isWild"></param>
        /// <param name="isScatter"></param>
        public Symbol(int id, string name, bool isWild, bool isScatter)
        {
            this.id = id;
            this.name = name;
            this.isWild = isWild;
            this.isScatter = isScatter;
        }
    }
}
