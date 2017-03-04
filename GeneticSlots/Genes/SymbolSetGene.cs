using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticSlots.Genes
{
    class SymbolSetGene
    {
        // Environment
        Environment environment;

        // RNG
        Random rand;


        /// <summary>
        /// SymbolSetGene constructor.
        /// </summary>
        /// <param name="environment"></param>
        public SymbolSetGene(Environment environment)
        {
            this.environment = environment;
            this.rand = new Random(this.GetHashCode());
        }


        // API
        // ==============================================================================

        /// <summary>
        /// Creates a first generation of this gene, using the environment as a source
        /// of trait paramters instead of inherited traits.
        /// </summary>
        public void CreateFirstGeneration()
        {
            
        }
    }
}
