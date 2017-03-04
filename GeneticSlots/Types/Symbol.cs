using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticSlots.Types
{
    class Symbol
    {
        public int id;

        public string name;

        public bool isWild = false;

        public bool isScatter = false;

        public int multiplier = 1;

        public double minBias = 1.0;

        public double maxBias = 1.0;


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
