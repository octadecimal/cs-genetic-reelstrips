using GeneticSlots.Genes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticSlots
{
    class SlotMachine
    {
        // World environment.
        Environment environment;

        // Chromosome
        Chromosome chromosome;


        /// <summary>
        /// SlotMachine constructor.
        /// </summary>
        /// <param name="environment"></param>
        public SlotMachine(Environment environment)
        {
            this.environment = environment;
        }


        // API
        // ==============================================================================

        public void InjectChromosome(Chromosome chromosome)
        {
            this.chromosome = chromosome;
        }
    }
}
