using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cs_genetic_reelstrips
{
    class Screen
    {
        public static string currentScreen = "start";

        private static Program program;
        
        private static int W = 80;
        private static int B = 20;
        private static int T = 20;

        public static void Init(Program program)
        {
            Screen.program = program;
        }

        public static void ShowSimulationScreen()
        {
            lock(new object())
            { 
                currentScreen = "simulation";
            
                Console.Clear();

                WriteHeader();
                Break(1);

                WriteValue("time: ", DateTime.Now);
                Break();
                WriteValue("action: ", Status.currentAction);
                Break();
                WriteValue("progress: ", Status.simulationsCompleted + "/" + Settings.population);
                Break();
                WriteValue("generation: ", Status.currentGeneration);
                Break(2);

                Thread.Sleep(1000);
                ShowSimulationScreen();
            }
        }

        public static void ShowStartScreen()
        {
            Console.Clear();
            WriteHeader();
            Break(1);

            WriteValue("population", Settings.population);
            WriteValue("precision", Settings.precision);
            Break(2);

            WriteValue("targetRTP", Settings.targetRTP);
            WriteValue("targetWinRate", Settings.targetWinRate);
            Break(2);

            WriteValue("numCols", Settings.numCols);
            WriteValue("numRows", Settings.numRows);
            Break(2);
            WriteValue("minReelStripLength", Settings.minReelStripLength);
            WriteValue("maxReelStripLength", Settings.maxReelStripLength);
            Break(2);
            WriteValue("minWildBias", Settings.minWildBias);
            WriteValue("maxWildBias", Settings.maxWildBias);
            Break(2);
            WriteValue("minScatterBias", Settings.minScatterBias);
            WriteValue("maxScatterBias", Settings.maxScatterBias);
            Break(2);

            WriteLine("-");

            string input = Console.ReadLine();
            HandleInput(input);
        }

        private static void WriteHeader()
        {
            WriteLine("=");
            Write("Strip Gen");
            Write("Copyright 2016");
            Break();
            WriteLine("-");
        }

        private static void WriteLine(string input)
        {
            for(int i = 0; i < W; i++)
            {
                Console.Write(input);
            }
            Console.Write("\n");
        }

        private static void Write(string input)
        {
            int pad = B - input.Length;

            Console.Write(input);

            for(int i = 0; i < pad; i++)
            {
                Console.Write(" ");
            }
        }

        private static void WriteValue(string caption, object value)
        {
            int cpad = T - caption.Length;

            Console.Write(caption);

            for(int i = 0; i < cpad; i++)
            {
                Console.Write(" ");
            }

            int vpad = B - value.ToString().Length;

            Console.Write(value);

            for (int i = 0; i < vpad; i++)
            {
                Console.Write(" ");
            }
        }

        private static void Break(int count = 1)
        {
            for(int i = 0; i < count; i++)
            { 
                Console.Write("\n");
            }
        }

        private static void HandleInput(string input)
        {
            string command;
            string param;

            // Get input
            if (input.IndexOf(" ") == -1)
            {
                command = input;
                param = "0";
            }
            else
            {
                string[] split = input.Split(' ');
                command = split[0];
                param = split[1];
            }

            // Handle input
            try
            {
                switch (command)
                {
                    case "start":
                        program.Start();
                        break;
                    
                    case "population":
                        Settings.population = Convert.ToInt32(param);
                        ShowStartScreen();
                        break;

                    case "precision":
                        Settings.precision = Convert.ToInt32(param);
                        ShowStartScreen();
                        break;

                    default:
                        InvalidInput();
                        break;
                }
            }
            catch(Exception e)
            {
                InvalidInput();
            }
        }

        private static void InvalidInput()
        {
            Console.WriteLine("INVALID INPUT");
            Thread.Sleep(500);
            ShowStartScreen();
        }
    }
}
