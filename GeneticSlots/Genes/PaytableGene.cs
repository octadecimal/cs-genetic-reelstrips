using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticSlots.Genes
{
    class PaytableGene
    {
        // Genome
        public int[] genome;

        // Environment
        Environment environment;

        // RNG
        Random rand;


        /// <summary>
        /// PaytableGene constructor.
        /// </summary>
        /// <param name="environment"></param>
        public PaytableGene(Environment environment)
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
            // Create initial genome
            genome = new int[environment.numCols];

            // Loop through genome, ignoring symbols not falling under `minPayingSymbols` count
            for(int i = environment.minPayingSymbols - 1; i < environment.numCols; i++)
            {
                // Select random pay index
                int index = rand.Next(0, environment.payouts.Length);

                // Set pay index
                genome[i] = index;
            }

            // Sort by payout low->high
            Array.Sort(genome);
        }

        /// <summary>
        /// Prints the genome to the console.
        /// </summary>
        public void PrintGenome()
        {
            for (int i = 0; i < genome.Length; i++) Console.Write("[" + genome[i] + "]");
            Console.Write("\n");
        }
    }
}
