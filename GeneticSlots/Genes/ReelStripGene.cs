using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticSlots.Genes
{
    class ReelStripGene
    {
        // Environment
        Environment environment;


        /// <summary>
        /// ReelStripGene constructor.
        /// </summary>
        /// <param name="environment"></param>
        public ReelStripGene(Environment environment)
        {
            this.environment = environment;
            Console.WriteLine(this.GetHashCode());
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
