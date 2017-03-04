using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticSlots.Genes
{
    class Chromosome
    {
        // Environment
        Environment environment;

        // Genes
        SymbolSetGene symbolSet;
        PaytableGene paytable;
        ReelStripGene reelStrips;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Chromosome(Environment environment)
        {
            symbolSet = new SymbolSetGene(environment);
            paytable = new PaytableGene(environment);
            reelStrips = new ReelStripGene(environment);
        }


        // API
        // ==============================================================================

        /// <summary>
        /// Creates this chromosome as a first generation chromosome, not
        /// inheriting any traits from any other chromosome and instead randomly
        /// generating its attributes based on the world environment.
        /// </summary>
        public void CreateFirstGeneration()
        {
            symbolSet.CreateFirstGeneration();
        }

        /// <summary>
        /// Mates this chromosome with a passed chromosome, returning a new
        /// chromosome instance with mixed traits of both.
        /// </summary>
        public Chromosome Mate(Chromosome mate)
        {
            return null;
        }
    }
}
