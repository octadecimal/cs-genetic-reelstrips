using cs_genetic_reelstrips.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_genetic_reelstrips
{
    public delegate void CompleteDelegate(object sender);

    class SlotMachine
    {
        public int id;
        public SlotMachineDNA dna;

        public event CompleteDelegate CompleteEvent;

        private ReelSet reelSet;

        public int balance = 1000;
        public float rtp = 1.0f;

        public int numSpins;
        public int numWins;


        /// <summary>
        /// SlotMachine constructor.
        /// </summary>
        public SlotMachine(int id)
        {
            this.id = id;

            // Create DNA
            dna = new SlotMachineDNA();
            //dna.PrintValues();

            // Create reel set
            reelSet = new ReelSet(dna);
        }

        public void StartSimulating()
        {
            numSpins = 0;

            for(int i = 0; i < Settings.precision; i++)
            {
                Spin();
            }

            //if(rtp >= -200.0 && rtp < 300.0)
            //if((numWins/numSpins) > 0.10 && (numWins/numSpins) < 0.95)
            if(numWins > 0 && numWins < Settings.precision && rtp > -10 && rtp < 20)
            {
                Console.WriteLine("["+id+"]\t"+(rtp*100)+ "%\t" + numWins+"/"+numSpins+"\t$" + balance);
            }

            CompleteEvent(this);
        }

        public void Spin()
        {
            numSpins++;

            balance -= Settings.lines.Length;

            //Console.WriteLine("$"+balance);
            reelSet.Spin();
            //reelSet.PrintSymbols();

            int win = EvaluateWins();
            balance += win;

            if(win > 0)
            {
                numWins++;
            }

            rtp = balance / 1000f;
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
                currentSymbol = reelSet.visibleSymbols[i][line[i]];

                if (scenario[i] != currentSymbol.id && !currentSymbol.isWild)
                {
                    return 0;
                }
            }

            return scenario[scenario.Length-1];
        }
    }
}
