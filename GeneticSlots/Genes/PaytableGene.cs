using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticSlots.Genes
{
    class PaytableGene
    {
        // Environment
        Environment environment;

        /// <summary>
        /// PaytableGene constructor.
        /// </summary>
        /// <param name="environment"></param>
        public PaytableGene(Environment environment)
        {
            this.environment = environment;
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
