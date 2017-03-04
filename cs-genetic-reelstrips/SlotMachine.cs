using cs_genetic_reelstrips.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cs_genetic_reelstrips
{
    public delegate void CompleteDelegate(object sender);

    class SlotMachine
    {
        public int id;
        public SlotMachineDNA dna;

        public event CompleteDelegate CompleteEvent;

        public int balance = 1000;
        public double rtp = 1.0;
        public double winRate = 0.0;

        public int numSpins;
        public int numWins;

        public double fitness = 0;


        /// <summary>
        /// SlotMachine constructor.
        /// </summary>
        public SlotMachine(int id)
        {
            this.id = id;

            // Create DNA
            this.dna = new SlotMachineDNA();
            
            //this.dna.PrintValues();
        }


        /// <summary>
        /// Inserts new DNA, effectively creating a new machine.
        /// Here all values should be reset.
        /// </summary>
        /// <param name="dna"></param>
        public void InsertDNA(SlotMachineDNA dna)
        {
            numSpins = 0;
            numWins = 0;
            balance = 1000;
            winRate = 0.0;
            rtp = 1;

            this.dna = dna;
        }


        /// <summary>
        /// Starts the simulation runs.
        /// </summary>
        public void StartSimulating()
        {
            // Reset
            numSpins = 0;
            numWins = 0;
            balance = 1000;
            winRate = 0.0;
            rtp = 1;

            // Simulate spins
            for (int i = 0; i < Settings.precision; i++)
            {
                Spin();
            }

            // Calculate fitness
            double distanceFromTargetRTP = Math.Pow((1 / Math.Abs(Settings.targetRTP - rtp)) * 100, 2);
            double distanceFromWinRate = Math.Pow((1 / Math.Abs(Settings.targetWinRate - winRate)) * 1, 1);
            //fitness = distanceFromTargetRTP;
            fitness = distanceFromWinRate;

            if(rtp > 1000 && rtp < -1000)
            {
                //fitness *= .1;
            }

            //if(rtp >= -200.0 && rtp < 300.0)
            //if((numWins/numSpins) > 0.10 && (numWins/numSpins) < 0.95)
            if (numWins > 10000 && numWins < Settings.precision && rtp > -10 && rtp < 10)
            //if(numWins > 0 && numWins < Settings.precision)
            {
                //Console.WriteLine();
                //Console.WriteLine("rtp: "+distanceFromTargetRTP);
                //Console.WriteLine("win: " + distanceFromWinRate);
                //Console.WriteLine("FITNESS: " + fitness);
                Console.Write("\n["+id+"]\t"+(rtp*100).ToString("N4")+ "%\t" + numWins+"/"+numSpins+"\t"+fitness.ToString("N6"));
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

            // Apply bet
            balance -= Settings.lines.Length;

            // Spin
            dna.reelSet.Spin();

            // Evaluate wins
            int win = EvaluateWins();

            // Apply win
            balance += win;

            // Count wins
            if(win > 0)
            {
                numWins++;
            }

            // Calculate rates
            winRate = (float)numWins / (float)numSpins;
            rtp = balance / 1000f;
        }

        public void NormalizeFitness(double max)
        {
            fitness /= max;
            
            if(fitness >= 1)
            { 
                //Console.WriteLine(rtp +" -> "+fitness +" " + (fitness>1));
            }
        }

        public int EvaluateWins()
        {
            int[][] lines = Settings.lines;

            int totalWin = 0;

            // Loop through each line
            for(int i = 0; i < lines.Length; i++)
            {
                int win = EvaluateLine(lines[i]);

                if (win > 0)
                {
                    //Console.WriteLine();
                    //Console.WriteLine("Win: line=" + i+ " win="+win);
                }

                totalWin += win;
            }

            return totalWin;
        }

        private int EvaluateLine(int[] line)
        {
            //Console.WriteLine("Evaluating line...");

            int[][] winScenarios = dna.winScenarios;

            // Loop through each win scenario
            for(int i = 0; i < winScenarios.Length; i++)
            {
                int win = EvaluateLineScenario(line, winScenarios[i]);

                if(win > 0)
                {
                    return win;
                }
            }

            return 0;
        }

        private int EvaluateLineScenario(int[] line, int[] scenario)
        {
            Symbol currentSymbol;

            for (int i = 0; i < line.Length; i++)
            {
                currentSymbol = dna.reelSet.visibleSymbols[i][line[i]];

                if (scenario[i] != currentSymbol.id && !currentSymbol.isWild)
                {
                    return 0;
                }
            }

            return scenario[scenario.Length-1];
        }
    }
}
