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
        /// <summary>
        /// Encoded genome.
        /// </summary>
        public int[] genome;


        // Environment
        Environment environment;

        // RNG
        Random rand;

        // Number of activated dna
        int numActivatations;


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

            // Save number of activated genes
            this.numActivatations = numSymbolsToActivate;
        }

        /// <summary>
        /// Decodes the genome, returning an array of symbol ids.
        /// </summary>
        /// <returns></returns>
        public Symbol[] DecodeGenome()
        {
            // Flag to keep track of current output index
            int currentIndex = 0;

            // Create output array
            Symbol[] output = new Symbol[numActivatations];

            // Search for activated genes
            for(int i = 0; i < genome.Length; i++)
            {
                if(genome[i] != 0)
                {
                    output[currentIndex] = environment.symbols[i];
                    currentIndex++;
                }
            }

            // Return output
            return output;
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
            for(int i = 0; i < genome.Length; i++) Console.Write("[" + genome[i] + "]");
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
