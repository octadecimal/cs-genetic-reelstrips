using GeneticSlots.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticSlots.Genes
{
    class SymbolSetGene
    {
        int[] genome;

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
        /// Creates our first generation genome, using the environment as a source of
        /// trait parameters instead of inherited traits from other genes.
        /// </summary>
        public void CreateFirstGeneration()
        {
            int numActivations = 0;

            // Create genome
            genome = new int[environment.symbols.Length];

            // Generate random number of total symbol count
            int numSymbolsToActivate = rand.Next(environment.minSymbols, environment.symbols.Length);

            // Activate random dna
            while (numActivations < numSymbolsToActivate)
            {
                // Select a random index
                int randomIndex = rand.Next(0, environment.symbols.Length);

                // Activate position if not already activated
                if(genome[randomIndex] == 0)
                {
                    genome[randomIndex] = 1;
                    numActivations++;
                }
            }
        }

        /// <summary>
        /// Mates this gene with the passed gene, returning a child gene
        /// with mixes traits.
        /// </summary>
        /// <param name="mate"></param>
        /// <returns></returns>
        public SymbolSetGene Mate(SymbolSetGene mate)
        {
            SymbolSetGene child = Mix(mate);

            return child;
        }

        /// <summary>
        /// Prints the genome to the console.
        /// </summary>
        public void PrintGenome()
        {
            Console.Write("\n");
            for(int i = 0; i < genome.Length; i++)
            {
                Console.Write("[" + genome[i] + "]");
            }
            Console.Write("\n");
        }


        // PRIVATE
        // ==============================================================================

        private SymbolSetGene Mix(SymbolSetGene mate)
        {
            return null;
        }
    }
}
