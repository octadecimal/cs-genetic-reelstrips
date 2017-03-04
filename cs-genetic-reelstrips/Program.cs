using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cs_genetic_reelstrips
{
    class Program
    {
        /// <summary>
        /// Slot machine population.
        /// </summary>
        private SlotMachine[] population;

        /// <summary>
        /// Flag to keep track of slot machines that are still running simulations.
        /// </summary>
        private int numSlotMachinesComplete = 0;
        private DateTime startTime;


        /// <summary>
        /// Application entry point.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            new Program();
        }

        /// <summary>
        /// Program constructor.
        /// </summary>
        public Program()
        {
            // Load and parse settings
            LoadSettings();

            // Show start screen
            Screen.Init(this);
            Screen.ShowStartScreen();
        }

        /// <summary>
        /// Starts the program.
        /// </summary>
        public void Start()
        {
            // Create initial population
            CreatePopulation();

            // Start spinning
            StartSimulations();

            // Wait
            Console.ReadKey();
        }

        /// <summary>
        /// Starts simulations.
        /// </summary>
        public void StartSimulations()
        {
            //Console.Clear();
            Console.WriteLine();
            Console.WriteLine("Simulating generation: "+ Status.currentGeneration);

            Status.currentAction = "simulating";
            
            if(Screen.currentScreen != "simulation")
            { 
                //Thread ts = new Thread(new ThreadStart(Screen.ShowSimulationScreen));
                //ts.Priority = ThreadPriority.Highest;
                //ts.Start();
            }

            startTime = DateTime.Now;
            numSlotMachinesComplete = 0;
            Status.simulationsCompleted = 0;

            for (int i = 0; i < population.Length; i++)
            {
                Thread.Sleep(100);
                Thread t = new Thread(new ThreadStart(population[i].StartSimulating));
                t.Start();
            }

            //Console.WriteLine("All threads started.");
        }

        private void OnSlotMachineSimulationComplete(object sender)
        {
            SlotMachine slotMachine = (SlotMachine)sender;

            numSlotMachinesComplete++;
            Status.simulationsCompleted++;
            //Screen.ShowSimulationScreen();

            if (numSlotMachinesComplete == population.Length)
            {
                //Console.WriteLine("COMPLETE: "+(DateTime.Now - startTime));

                CompleteGeneration();
            }
        }

        public void CompleteGeneration()
        {
            Console.WriteLine("\n\nGeneration complete: "+Status.currentGeneration);
            PrintAverageRTP();

            // Normalize fitnesses
            double maxFitness = 1;
            for (int i = 0; i < population.Length; i++)
            {
                //maxFitness = Math.Max(population[i].fitness, maxFitness);
            }
            for (int i = 0; i < population.Length; i++)
            {
                //population[i].NormalizeFitness(maxFitness);
            }

            // Generate weighted breeding pool
            WeightedValues<SlotMachine> values = new WeightedValues<SlotMachine>();
            for(int i = 0; i < population.Length; i++)
            {
                //if(population[i].rtp < 100 && population[i].rtp > -100)
                { 
                    values.AddValue(population[i], population[i].fitness);
                }
            }

            // Update status
            Status.currentGeneration++;
            //Screen.ShowSimulationScreen();

            // Breed
            BeginCrossBreeding(values);
        }

        private void BeginCrossBreeding(WeightedValues<SlotMachine> values)
        {
            Status.currentAction = "breeding";

            // Search for matching RTP
            for (int i = 0; i < population.Length; i++)
            {
                double delta = population[i].rtp - (double)Settings.targetRTP;
                if (Math.Abs(delta) < 0.01 && population[i].winRate > Settings.targetWinRate)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("\n\n\n");
                    Console.WriteLine("FOUND!");
                    Console.WriteLine("id: " + population[i].id);
                    Console.WriteLine("rtp: " + population[i].rtp);
                    Console.WriteLine("wrt: " + population[i].winRate);
                    Console.WriteLine();
                    population[i].dna.PrintPaytable();
                    Console.WriteLine();
                    population[i].dna.PrintReelStrips();
                    Console.WriteLine();

                    return;
                }
            }

            //values.PrintThresholds();

            for (int i = 0; i < Settings.population; i++)
            {
                SlotMachine a = values.getValue();
                SlotMachine b = values.getValue();

                //while(a.id == b.id)
                //{
                //Console.Write(".");
                //b = values.getValue();
                //}

                Console.Write("[" + a.id + "<->" + b.id + "] ");
                //Console.WriteLine(a.fitness+"<->"+ b.fitness);

                SlotMachineDNA child = a.dna.Cross(b.dna);
                population[i].InsertDNA(child);
            }

            Thread.Sleep(1000);
            
            StartSimulations();
        }

        private void PrintAverageRTP()
        {
            Console.WriteLine();

            double sum = 0;

            for(int i = 0; i < population.Length; i++)
            {
                sum += population[i].rtp;
            }

            double avg = sum / population.Length;

            Console.WriteLine("\nAverage RTP: " + (avg*100)+"%\n");
        }

        /// <summary>
        /// Loads and parses the settings.
        /// </summary>
        private void LoadSettings()
        {
            Settings.LoadXml("todo");
        }

        /// <summary>
        /// Creates the initial slot machine population.
        /// </summary>
        private void CreatePopulation()
        {
            //Console.WriteLine("Creating slot machines: " + Settings.population);

            // Create population array
            population = new SlotMachine[Settings.population];

            // Create slot machines
            for(int i = 0; i < Settings.population; i++)
            {
                SlotMachine slotMachine = new SlotMachine(i);
                population[i] = slotMachine;

                slotMachine.CompleteEvent += new CompleteDelegate(OnSlotMachineSimulationComplete);
            }
        }
    }
}
