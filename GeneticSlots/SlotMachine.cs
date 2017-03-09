using GeneticSlots.Genes;
using GeneticSlots.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticSlots
{
    public delegate void CompleteDelegate(object sender);

    class SlotMachine
    {
        // Complete event.
        public event CompleteDelegate CompleteEvent;

        // Slot Machine unique identifier.
        public int id;

        // Chromosome
        public Chromosome chromosome;

        // Data
        public double fitness;
        public double rtp;
        public double winRate;
        public double distanceFromTargetRtp;
        public double distanceFromTargetWrt;

        // World environment.
        Environment environment;

        // Win Scenarios
        int[][] winScenarios;

        // State
        int balance;
        int startingBalance;
        int numSpins;
        int numWins;


        /// <summary>
        /// SlotMachine constructor.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="environment"></param>
        public SlotMachine(int id, Environment environment)
        {
            this.id = id;
            this.environment = environment;
        }


        // API
        // ==============================================================================

        /// <summary>
        /// Resets state.
        /// </summary>
        public void Reset()
        {
            this.balance = this.startingBalance = 100000;
            this.numSpins = 0;
            this.numWins = 0;
            this.rtp = 0;
            this.winRate = 0;
        }

        /// <summary>
        /// Starts simulating spins n times as defined by environment precision.
        /// </summary>
        public void Simulate()
        {
            // Reset state
            Reset();

            // Simulate spins
            for(int i = 0; i < environment.precision; i++)
            {
                Spin();
            }

            // Post-process
            this.rtp = (double)balance / (double)startingBalance;
            this.winRate = (double)numWins / (double)numSpins;
            distanceFromTargetRtp = Math.Abs(environment.targetRtp - rtp);
            distanceFromTargetWrt = Math.Pow((1 / Math.Abs(environment.targetWinRate - winRate)) * 100, 2);
            this.fitness = Math.Pow((1 / Math.Abs(environment.targetRtp - rtp)) * 100, 2) * winRate;

            if (rtp > 0.8 && rtp < 1.1)
            {
                Console.WriteLine("\n[" + id + "]\t" + (rtp * 100).ToString("N4") + "%\t" + winRate.ToString("F") + "\t\t" + fitness.ToString("F"));
            }
            else
            {
                Console.Write(".");
            }

            // Dispatch complete
            CompleteEvent(this);
        }

        /// <summary>
        /// Performs an individual spin.
        /// </summary>
        public void Spin()
        {
            // Pre-process
            numSpins++;
            balance -= environment.lines.Length;

            // Get random symbol chunks
            int[][] symbols = chromosome.getRandomSymbolChunks();

            // Evaluate wins
            int win = EvaluateWins(symbols);

            // Post-process
            if(win > 0)
            {
                balance += win;
                numWins++;
            }
        }

        /// <summary>
        /// Injects a chromosome into this slot machine, effectively altering its traits.
        /// </summary>
        /// <param name="chromosome"></param>
        public void InjectChromosome(Chromosome chromosome)
        {
            this.chromosome = chromosome;

            Reset();
            GenerateWinScenariosFromChromosome();
        }


        // PRIVATE
        // ==============================================================================

        /// <summary>
        /// Evaluates all lines and all scenarios for wins.
        /// </summary>
        /// <param name="symbols"></param>
        /// <returns></returns>
        private int EvaluateWins(int[][] symbols)
        {
            int totalWin = 0;

            // Get lines
            int[][] lines = environment.lines;

            // Loop through each line
            for(int i = 0; i < lines.Length; i++)
            {
                // Accumulate line wins
                totalWin += EvaluateLine(lines[i], symbols); ;
            }

            return totalWin;
        }

        /// <summary>
        /// Tests a line against all win scenarios.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="symbols"></param>
        /// <returns></returns>
        private int EvaluateLine(int[] line, int[][] symbols)
        {
            // Loop through each win scenario
            for(int i = 0; i < winScenarios.Length; i++)
            {
                // Get scenario win
                int win = EvaluateScenario(line, winScenarios[i], symbols);

                // Return win if win resulted from current scenario
                if(win > 0)
                {
                    return win;
                }
            }

            // No scenario on current line, return 0 for win
            return 0;
        }

        /// <summary>
        /// Tests for a single win on a single line and single scenario.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="scenario"></param>
        /// <param name="symbols"></param>
        /// <returns></returns>
        private int EvaluateScenario(int[] line, int[] scenario, int[][] symbols)
        {
            Symbol symbol;
            Symbol[] symbolDefinitions = environment.symbols;

            // Loop through each line position
            for(int i = 0; i < line.Length; i++)
            {
                // Get current symbol at current line position
                symbol = symbolDefinitions[symbols[i][line[i]]];

                // Test symbol
                if(!symbol.isWild && scenario[i] != symbol.id)
                {
                    return 0;
                }
            }

            // All tests passed, return payout for scenario
            return scenario[scenario.Length - 1];
        }


        // UTIL
        // ==============================================================================

        /// <summary>
        /// Generates a set of win scenarios from the paytable, allowing for more efficient
        /// and less-complicated win testing.
        /// </summary>
        private void GenerateWinScenariosFromChromosome()
        {
            int scenarioId = 0;
            int numSymbols = chromosome.symbolSet.genome.Length;

            // Create win scenarios table
            winScenarios = new int[numSymbols * (environment.numCols - environment.minPayingSymbols + 1)][];

            // Loop through each symbol
            for (int i = 0; i < numSymbols; i++)
            {
                // Get paytable genome for current symbol
                int[] symbolPays = chromosome.paytables[i].genome;

                // Create scenario for each pay
                for (int j = environment.minPayingSymbols - 1; j < environment.numCols; j++)
                {
                    int[] scenario = new int[environment.numCols + 1];

                    for (int k = 0; k < environment.numCols; k++)
                    {
                        if (k <= j)
                        {
                            scenario[k] = i;
                        }
                        else
                        {
                            scenario[k] = -1;
                        }
                    }

                    // Fill last cell with payout for this scenario
                    scenario[environment.numCols] = chromosome.paytables[i].genome[j];

                    // Save scenario
                    winScenarios[scenarioId] = scenario;

                    // Post-process
                    scenarioId++;
                }
            }

            // Sort scenarios by highest payout
            Array.Sort(winScenarios, SortWinScenariosByPay);
        }

        /// <summary>
        /// Prints a set of symbols for debuggin.
        /// </summary>
        /// <param name="symbols"></param>
        private void PrintSymbols(int[][] symbols)
        {
            for(int i = 0; i < symbols[0].Length; i++)
            {
                for(int j = 0; j < symbols.Length; j++)
                {
                    Console.Write(environment.symbols[symbols[j][i]].name + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Sorts win scenarios by win, highest first.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private int SortWinScenariosByPay(int[] a, int[] b)
        {
            int lastIndex = a.Length - 1;
            if (a[lastIndex] < b[lastIndex]) return 1;
            if (a[lastIndex] > b[lastIndex]) return -1;
            return 0;
        }
    }
}
