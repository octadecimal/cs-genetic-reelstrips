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

        /// <summary>
        /// Injects a chromosome into this slot machine, effectively altering its traits.
        /// </summary>
        /// <param name="chromosome"></param>
        public void InjectChromosome(Chromosome chromosome)
        {
            this.chromosome = chromosome;
        }
    }
}
