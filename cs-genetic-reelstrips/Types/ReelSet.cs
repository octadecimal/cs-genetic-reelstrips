using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_genetic_reelstrips.Types
{
    class ReelSet
    {
        private SlotMachineDNA dna;
        private Symbol[][] reelStrips;
        public Symbol[][] visibleSymbols;

        /// <summary>
        /// ReelSet constructor.
        /// </summary>
        /// <param name="dna"></param>
        public ReelSet(SlotMachineDNA dna)
        {
            this.dna = dna;

            // Initialize storage
            reelStrips = new Symbol[Settings.numCols][];
            visibleSymbols = new Symbol[Settings.numCols][];

            // Create reel strips
            for(int i = 0; i < Settings.numCols; i++)
            {
                reelStrips[i] = createReelStrip();
            }
        }

        /// <summary>
        /// Spins the reel set.
        /// </summary>
        public void Spin()
        {
            for(int i = 0; i < reelStrips.Length; i++)
            {
                int position = Settings.rng.Next(0, reelStrips[i].Length);
                visibleSymbols[i] = getSymbolsAt(i, position);
            }
        }

        /// <summary>
        /// Gets the symbols at the specified position, will handle wrapping.
        /// </summary>
        /// <param name="reelIndex">Reel index</param>
        /// <param name="position">Reel position</param>
        /// <returns></returns>
        private Symbol[] getSymbolsAt(int reelIndex, int position)
        {
            Symbol[] output = new Symbol[Settings.numRows];

            for(int i = 0; i < output.Length; i++)
            {
                int symbolIndex = (position + i) % reelStrips[reelIndex].Length;
                output[i] = reelStrips[reelIndex][symbolIndex];
            }

            return output;
        }

        /// <summary>
        /// Prints the visible symbols for debugging purposes.
        /// </summary>
        public void PrintSymbols()
        {
            Console.WriteLine();
            for (int y = 0; y < Settings.numRows; y++)
            {
                for (int x = 0; x < Settings.numCols; x++)
                {
                    Console.Write("[" + visibleSymbols[x][y].name + "] ");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Creates and returns a populated reelstrip, accounting for symbol-specific biases.
        /// </summary>
        /// <returns></returns>
        private Symbol[] createReelStrip()
        {
            // Generate random length that is within bounds defined in settings
            int length = Settings.rng.Next(Settings.minReelStripLength, Settings.maxReelStripLength);

            // Create empty reelStrip array
            Symbol[] reelStrip = new Symbol[length];

            // Flag
            bool canAdd = false;

            // Generate
            int currentCount = 0;
            while(currentCount < length)
            {
                // Reset
                canAdd = false;

                // Get a random symbol
                Symbol symbol = Settings.symbols[Settings.rng.Next(0, Settings.symbols.Length)];

                // Check for specific symbol types against biases
                if (symbol.isWild && Settings.rng.NextDouble() <= dna.wildBias)
                {
                    canAdd = true;
                }
                else if(symbol.isScatter && Settings.rng.NextDouble() <= dna.scatterBias)
                {
                    canAdd = true;
                }
                else
                {
                    canAdd = true;
                }

                // Populate symbol if allowed to be added
                if(canAdd)
                {
                    reelStrip[currentCount] = symbol;
                    currentCount++;
                }
            }

            return reelStrip;
        }
    }
}
