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
            startTime = DateTime.Now;
            numSlotMachinesComplete = 0;

            for (int i = 0; i < population.Length; i++)
            {
                Thread t = new Thread(new ThreadStart(population[i].StartSimulating));
                t.Start();
            }

            Console.WriteLine("All threads started.");
        }

        private void OnSlotMachineSimulationComplete(object sender)
        {
            SlotMachine slotMachine = (SlotMachine)sender;

            numSlotMachinesComplete++;

            //Console.WriteLine(numSlotMachinesComplete + "/" + population.Length);

            if(numSlotMachinesComplete == population.Length)
            {
                Console.WriteLine("ALL DONE!");

                Console.WriteLine((DateTime.Now - startTime));
            }
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
            Console.WriteLine("Creating slot machines: " + Settings.population);

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
