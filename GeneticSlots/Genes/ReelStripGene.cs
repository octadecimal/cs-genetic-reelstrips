using GeneticSlots.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticSlots.Genes
{
    class ReelStripGene
    {
        /// <summary>
        /// Encoded reel strip genome.
        /// </summary>
        public int[] genome;


        // Environment
        Environment environment;

        // RNG
        Random rand;


        /// <summary>
        /// ReelStripGene constructor.
        /// </summary>
        /// <param name="environment"></param>
        public ReelStripGene(Environment environment)
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
        public void CreateFirstGeneration(SymbolSetGene symbolSet)
        {
            int numSymbolsInjected = 0;
            int numSymbolsToInject = 0;

            // Create our genome
            genome = new int[environment.maxReelStripLength];

            // Initialize our genome with -1
            for(int i = 0; i < genome.Length; i++) {
                genome[i] = -1;
            }

            // Get our symbol ids by decoding the symbol set genome
            Symbol[] symbolIds = symbolSet.DecodeGenome();

            // Generate a random number of symbols to be inserted into the genome
            numSymbolsToInject = rand.Next(environment.minReelStripLength, environment.maxReelStripLength);

            // Begin injections
            while(numSymbolsInjected < numSymbolsToInject)
            {
                // Generate a random index
                int randomIndex = rand.Next(0, genome.Length);

                // Inject if value not already injected
                if(genome[randomIndex] == -1)
                {
                    // Inject random symbol
                    genome[randomIndex] = symbolIds[rand.Next(0, symbolIds.Length)].id;
                    numSymbolsInjected++;
                }
            }

            PrintGenome();
            Console.WriteLine();
        }

        /// <summary>
        /// Prints the genome to the console.
        /// </summary>
        public void PrintGenome()
        {
            Console.Write("\n");
            for (int i = 0; i < genome.Length; i++)
            {
                string value = genome[i] < 0 ? " " : genome[i].ToString();
                Console.Write("" + value + " ");
            }
            Console.Write("\n");
        }
    }
}
