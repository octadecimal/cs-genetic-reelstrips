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
        public PaytableGene[] paytable;
        public ReelStripGene[] reelStrips;

        // Data
        public int[] winScenarios;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Chromosome(Environment environment)
        {
            this.environment = environment;

            // Create SymbolSet gene
            symbolSet = new SymbolSetGene(environment);

            // Create Paytable genes
            paytable = new PaytableGene[environment.symbols.Length];
            for(int i = 0; i < environment.symbols.Length; i++)
            {
                paytable[i] = new PaytableGene(environment);
            }

            // Create ReelStrip genes
            reelStrips = new ReelStripGene[environment.numCols];
            for(int i = 0; i < environment.numCols; i++)
            {
                reelStrips[i] = new ReelStripGene(environment);
            }

            // Generate win scenarios from paytable
            //winScenarios = GenerateWinScenariosFromPaytable();
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
            symbolSet.CreateFirstGeneration();

            // Create paytable
            for(int i = 0; i < paytable.Length; i++)
            {
                paytable[i].CreateFirstGeneration();
            }

            // Create reel strips
            for(int i = 0; i < reelStrips.Length; i++)
            {
                reelStrips[i].CreateFirstGeneration(symbolSet);
            }

            // Generate win scenarios from paytable
            //winScenarios = GenerateWinScenariosFromPaytable();

            //PrintChromosome();
        }

        /// <summary>
        /// Mates this chromosome with a passed chromosome, returning a new
        /// chromosome instance with mixed traits of both.
        /// </summary>
        public Chromosome Mate(Chromosome mate)
        {
            return null;
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
        /// Prints the entire chromosome.
        /// </summary>
        public void PrintChromosome()
        {
            // Print symbol set
            Console.WriteLine("[symbols]");
            symbolSet.PrintGenome();

            // Print paytable
            Console.WriteLine("[paytable]");
            for (int i = 0; i < paytable.Length; i++)
            {
                paytable[i].PrintGenome();
            }

            // Print reel strips
            Console.WriteLine("[reelstrips]");
            for (int i = 0; i < reelStrips.Length; i++)
            {
                reelStrips[i].PrintGenome();
            }
        }


        // PRIVATE
        // ==============================================================================

        
    }
}
