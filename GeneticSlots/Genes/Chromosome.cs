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
        public SymbolSetGene symbolSet;
        public PaytableGene[] paytables;
        public ReelStripGene[] reelStrips;

        // Data
        public int[] winScenarios;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Chromosome(Environment environment)
        {
            this.environment = environment;

            symbolSet = new SymbolSetGene(environment);

            // Create paytable
            paytables = new PaytableGene[environment.symbols.Length];
            for (int i = 0; i < environment.symbols.Length; i++)
            {
                paytables[i] = new PaytableGene(environment);
            }

            // Create reel strips
            reelStrips = new ReelStripGene[environment.numCols];
            for (int i = 0; i < environment.numCols; i++)
            {
                reelStrips[i] = new ReelStripGene(environment);
            }
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
            // Create symbol set
            symbolSet = new SymbolSetGene(environment);
            symbolSet.CreateFirstGeneration();

            // Create paytable
            paytables = new PaytableGene[environment.symbols.Length];
            for (int i = 0; i < environment.symbols.Length; i++)
            {
                paytables[i] = new PaytableGene(environment);
                paytables[i].CreateFirstGeneration();
            }

            // Create reel strips
            reelStrips = new ReelStripGene[environment.numCols];
            for (int i = 0; i < environment.numCols; i++)
            {
                reelStrips[i] = new ReelStripGene(environment);
                reelStrips[i].CreateFirstGeneration(symbolSet);
            }
        }

        /// <summary>
        /// Returns a random symbol from each reelstrip genome.
        /// </summary>
        /// <returns></returns>
        public int[][] getRandomSymbolChunks()
        {
            int[][] randomSymbols = new int[environment.numCols][];

            for (int i = 0; i < environment.numCols; i++)
            {
                randomSymbols[i] = reelStrips[i].GetRandomSymbolChunk();
            }

            return randomSymbols;
        }

        /// <summary>
        /// Returns an encoded genotype of this chromosome.
        /// </summary>
        public int[][] EncodeGenotype()
        {
            int currentOffset = 0;

            int[][] output = new int[1 + paytables.Length + reelStrips.Length][];

            output[currentOffset] = symbolSet.genome;
            currentOffset++;
            
            for(int i = 0; i < paytables.Length; i++)
            {
                output[currentOffset] = new int[paytables[i].genome.Length];
                for(int j = 0; j < paytables[i].genome.Length; j++)
                {
                    output[currentOffset][j] = environment.payouts[paytables[i].genome[j]];
                }
                currentOffset++;
            }

            for (int i = 0; i < reelStrips.Length; i++)
            {
                output[currentOffset] = reelStrips[i].genome;
                currentOffset++;
            }

            return output;
        }

        public string[] GetEncodedGenotypeString()
        {
            int[][] genotype = EncodeGenotype();

            string[] output = new string[genotype.Length];

            for (int i = 0; i < genotype.Length; i++)
            {
                output[i] = "";
                for (int j = 0; j < genotype[i].Length; j++)
                {
                    string s = genotype[i][j] == -1 ? " " : genotype[i][j].ToString();

                    output[i] += s + ",";
                }
            }

            return output;
        }

        /// <summary>
        /// Prints the genotype.
        /// </summary>
        public void PrintGenotype()
        {
            int[][] genotype = EncodeGenotype();

            for (int i = 0; i < genotype.Length; i++)
            {
                Console.WriteLine();
                for (int j = 0; j < genotype[i].Length; j++)
                {
                    string s = genotype[i][j] == -1 ? " " : genotype[i][j].ToString();

                    Console.Write(s + " ");
                }
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Prints the entire chromosome.
        /// </summary>
        public void PrintChromosome()
        {
            // Print symbol set
            Console.WriteLine("[symbols]");
            symbolSet.PrintGenome();

            // Print paytable
            Console.WriteLine("[paytable]");
            for (int i = 0; i < paytables.Length; i++)
            {
                paytables[i].PrintGenome();
            }

            // Print reel strips
            Console.WriteLine("[reelstrips]");
            for (int i = 0; i < reelStrips.Length; i++)
            {
                reelStrips[i].PrintGenome();
            }
        }

        
    }
}
