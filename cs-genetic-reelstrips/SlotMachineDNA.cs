using cs_genetic_reelstrips.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_genetic_reelstrips
{
    class SlotMachineDNA
    {
        public int[][] paytable;
        public int[][] winScenarios;
        public float wildBias;
        public float scatterBias;

        public SlotMachineDNA()
        {
            // Generate random biases
            wildBias = (float)(Settings.rng.NextDouble() * (Settings.maxWildBias - Settings.minWildBias)) + Settings.minWildBias;
            scatterBias = (float)(Settings.rng.NextDouble() * (Settings.maxScatterBias - Settings.minScatterBias)) + Settings.minScatterBias;

            // Generate paytable
            GeneratePaytable();

            // Generate win scenarios from paytable
            GenerateWinScenarios();
        }

        private void GenerateWinScenarios()
        {
            int scenarioID = 0;

            winScenarios = new int[paytable.Length * (Settings.numCols - Settings.minPayingSymbols + 1)][];

            for (int i = 0; i < paytable.Length; i++)
            {
                int symbolID = i;
                int[] symbolPays = paytable[i];

                // Create each scenario
                for (int j = Settings.minPayingSymbols-1; j < Settings.numCols; j++)
                {
                    int[] scenario = new int[Settings.numCols+1];

                    for(int k = 0; k < Settings.numCols; k++)
                    {
                        if(k <= j)
                        {
                            scenario[k] = symbolID;
                        }
                        else
                        {
                            scenario[k] = -1;
                        }
                    }

                    // Fill last cell with payout 
                    scenario[Settings.numCols] = paytable[i][j];

                    // Save scenario
                    winScenarios[scenarioID] = scenario;

                    // Post-process
                    scenarioID++;
                }
            }

            // Sort scenarios by highest payout
            Array.Sort(winScenarios, sortWinScenariosByPay);
        }

        private int sortWinScenariosByPay(int[] a, int[] b)
        {
            int lastIndex = a.Length - 1;
            if (a[lastIndex] < b[lastIndex]) return 1;
            if (a[lastIndex] > b[lastIndex]) return -1;
            return 0;
        }

        private void GeneratePaytable()
        {
            paytable = new int[Settings.symbols.Length][];

            for(int i = 0; i < Settings.symbols.Length; i++)
            {
                Symbol symbol = Settings.symbols[i];
                //Console.Write(symbol.name + "(" + symbol.id + ")\t ");

                int[] symbolPays = new int[Settings.numCols];

                for(int j = 0; j < Settings.numCols; j++)
                {
                    if(j < Settings.minPayingSymbols-1)
                    {
                        symbolPays[j] = 0;
                    }
                    else
                    {
                        symbolPays[j] = Settings.rng.Next(1, 50);
                        symbolPays[j] *= 5;

                        if (symbolPays[j] < 10)
                        {
                            //symbolPays[j] *= 5;
                        }
                        else if(symbolPays[j] < 50)
                        {
                            //symbolPays[j] *= 25;
                        }
                        else
                        {
                            //symbolPays[j] *= 100;
                        }
                    }
                }

                Array.Sort(symbolPays);

                paytable[i] = symbolPays;

                foreach(int value in symbolPays)
                {
                    //Console.Write(value + "\t");
                }
                //Console.WriteLine();
            }
        }

        public void PrintValues()
        {
            Console.WriteLine("wildBias=" + wildBias);
            Console.WriteLine("scatterBias=" + scatterBias);
        }
    }
}
