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

        private Random rng;

        public ReelSet reelSet;


        public SlotMachineDNA(int generation = 0)
        {
            rng = new Random(Settings.rng.Next());

            // Randomly generate if first generation
            if(generation == 0)
            {
                // Generate random biases
                wildBias = (float)(rng.NextDouble() * (Settings.maxWildBias - Settings.minWildBias)) + Settings.minWildBias;
                scatterBias = (float)(rng.NextDouble() * (Settings.maxScatterBias - Settings.minScatterBias)) + Settings.minScatterBias;

                // Generate reel set
                reelSet = new ReelSet(this);
            }

            // Generate paytable
            GeneratePaytable();

            // Generate win scenarios from paytable
            GenerateWinScenarios();
        }

        public SlotMachineDNA Cross(SlotMachineDNA partner)
        {
            SlotMachineDNA child = new SlotMachineDNA(1);

            child.wildBias = MixFloat(wildBias, partner.wildBias);
            child.scatterBias = MixFloat(scatterBias, partner.scatterBias);

            // TEMP USE THIS REELSET ON CHILD
            child.reelSet = this.reelSet;
            child.winScenarios = this.winScenarios;

            for (int i = 0; i < paytable.Length; i++)
            {
                int[] row = paytable[i];
                
                for(int j = 0; j < row.Length; j++)
                {
                    child.paytable[i][j] = Math.Abs(MixInt(paytable[i][j], partner.paytable[i][j], 0.15));

                    if (rng.NextDouble() <= 0.02)
                    {
                        if(child.paytable[i][j] != 0)
                        { 
                            child.paytable[i][j] += rng.Next(-10, 10);
                            child.paytable[i][j] = Math.Max(0, child.paytable[i][j]);
                        }
                    }
                }
            }

            return child;
        }

        private float MixFloat(float a, float b, float weight = 0.5f)
        {
            return a + (b - a) * weight;
        }

        private int MixInt(int a, int b, double weight = 0.5)
        {
            return Convert.ToInt32(a + (b - a) * weight);
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
                        symbolPays[j] = rng.Next(1, Settings.maxPay);
                        //symbolPays[j] *= 5;

                        if (symbolPays[j] < 10)
                        {
                            //symbolPays[j] *= 5;
                        }
                        else if(symbolPays[j] < 25)
                        {
                            //symbolPays[j] *= 10;
                        }
                        else
                        {
                            //symbolPays[j] *= 25;
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

        public void PrintPaytable()
        {
            Console.WriteLine();
            for (int y = 0; y < paytable.Length; y++)
            {
                int[] row = paytable[y];

                for (int x = 0; x < row.Length; x++)
                {
                    Console.Write(paytable[y][x] + " ");
                }

                Console.WriteLine();
            }
        }

        public void PrintReelStrips()
        {
            reelSet.PrintReelStrips();
        }
    }
}
