using GeneticSlots.Genes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticSlots
{
    class Breeder
    {
        // Environment
        Environment environment;

        // RNG
        Random rand;

        
        /// <summary>
        /// Breeder constructor.
        /// </summary>
        public Breeder(Environment environment)
        {
            this.environment = environment;
            this.rand = new Random(this.GetHashCode());
        }


        // API
        // ==============================================================================

        /// <summary>
        /// Breeds two chromosomes together.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public Chromosome Breed(Chromosome a, Chromosome b)
        {
            Chromosome child = new Chromosome(environment);

            // Breed
            int[] symbolSetGenome = BreedSymbolSet(a, b);
            int[][] paytablesGenome = BreedPaytables(a, b);
            int[][] reelStripsGenome = BreedReelStrips(a, b, symbolSetGenome);

            // Attach symbol set
            child.symbolSet.AttachGenome(symbolSetGenome);

            // Attach paytables
            for (int i = 0; i < paytablesGenome.Length; i++)
            {
                child.paytables[i].AttachGenome(paytablesGenome[i]);
            }

            // Attach reelstrips
            for (int i = 0; i < reelStripsGenome.Length; i++)
            {
                child.reelStrips[i].AttachGenome(reelStripsGenome[i]);
            }

            return child;
        }

        /// <summary>
        /// Breeds two symbol set genes and returns a child genome.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public int[] BreedSymbolSet(Chromosome a, Chromosome b)
        {
            // Create initial child genome
            int[] genome = new int[a.symbolSet.genome.Length];

            // Handle each
            //Console.WriteLine();
            for(int i = 0; i < genome.Length; i++)
            {
                int g0 = a.symbolSet.genome[i];
                int g1 = b.symbolSet.genome[i];

                int g = 0;

                if (g0 == 1 && g1 == 1) g = 1;
                if (g0 + g1 == 1) g = rand.Next(0, 2);

                genome[i] = g;

                //Console.WriteLine(i + ":  " + g0 + " " + g1 + "  " + g + " ");
            }
            //Console.WriteLine();

            // Mutate
            for(int i = 0; i < genome.Length; i++)
            {
                if(rand.NextDouble() < 0.01)
                {
                    if (genome[i] == 0)
                    {
                        genome[i] = 1;
                    }
                    //else
                    //{
                    //    int sum = 0;
                    //    for(int s = 0; s < genome.Length; s++)
                    //    {
                    //        sum += genome[i];
                    //    }

                    //    if(sum >= environment.minSymbols)
                    //    {
                    //        Console.WriteLine("HA: " + sum);
                    //        genome[i] = 0;
                    //    }
                    //}
                }
            }

            // Return new child genome
            return genome;
        }

        /// <summary>
        /// Breeds two paytable genes and returns a child gene.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public int[][] BreedPaytables(Chromosome a, Chromosome b)
        {
            int[][] genome = new int[environment.symbols.Length][];

            for(int i = 0; i < genome.Length; i++)
            {
                int[] g0 = a.paytables[i].genome;
                int[] g1 = b.paytables[i].genome;

                genome[i] = new int[g0.Length];

                for(int j = 0; j < g0.Length; j++)
                {
                    if (g0[j] != 0 && g1[j] != 0)
                    {
                        int index0 = g0[j];
                        int index1 = g1[j];
                        genome[i][j] = rand.Next(Math.Min(index0, index1), Math.Max(index0, index1));
                    }
                }
            }
            
            return genome;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public int[][] BreedReelStrips(Chromosome a, Chromosome b, int[] childSymbolSet)
        {
            int[][] genome = new int[environment.numCols][];

            for(int i = 0; i < genome.Length; i++)
            {
                genome[i] = new int[environment.maxReelStripLength];

                int[] g0 = a.reelStrips[i].genome;
                int[] g1 = b.reelStrips[i].genome;

                for(int j = 0; j < g0.Length; j++)
                {
                    int symbol0 = g0[j];
                    int symbol1 = g1[j];

                    bool exists0 = HasActiveSymbol(childSymbolSet, symbol0);
                    bool exists1 = HasActiveSymbol(childSymbolSet, symbol1);

                    if(!exists0 && !exists1)
                    {
                        genome[i][j] = -1;
                    }
                    else if(exists0 && exists1)
                    {
                        genome[i][j] = (rand.NextDouble() < 0.5) ? symbol0 : symbol1;
                    }
                    else if(exists0)
                    {
                        genome[i][j] = symbol0;
                    }
                    else if(exists1)
                    {
                        genome[i][j] = symbol1;
                    }

                    //Console.WriteLine(s(symbol0) + "\t" + s(symbol1) + "\t" + exists0 + "\t" + exists1 + "\t" + s(genome[i][j]));
                }

                for (int j = 0; j < genome.Length; j++)
                {
                    if (rand.NextDouble() < 0.01)
                    {
                        genome[i][j] = genome[i][rand.Next(0, genome[i].Length)];
                    }
                }
            }

            return genome;
        }

        private string s(int value)
        {
            if (value == -1) return " ";
            else return value.ToString();
        }


        // PRIVATE
        // ==============================================================================

        private bool HasActiveSymbol(int[] symbolSet, int symbolId)
        {
            if (symbolId == -1) return true;
            return symbolSet[symbolId] == 1;
        }
    }
}
