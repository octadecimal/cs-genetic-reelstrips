using GeneticSlots.Genes;
using GeneticSlots.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GeneticSlots
{
    /// <summary>
    /// Root class for our GeneticSlots program.
    /// </summary>
    class Program
    {
        // World environment.
        Environment environment;

        // Our current population.
        SlotMachine[] population;

        // Breeder
        Breeder breeder;

        // Our current generation.
        int currentGeneration = 1;

        // Number of slot machines that have completed simulations.
        int numSlotMachinesComplete = 0;


        /// <summary>
        /// Program entry point.
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
            // Load environment
            environment = new Environment();

            // Create breeder
            breeder = new Breeder(environment);

            // Create initial population
            createInitialPopulation();

            // Start
            Start();

            // Wait
            Console.ReadKey();
        }


        // API
        // ==============================================================================

        /// <summary>
        /// Starts the simulations.
        /// </summary>
        public void Start()
        {
            Simulate();
        }

        /// <summary>
        /// Runs simulations on all slot machines.
        /// </summary>
        public void Simulate()
        {
            //Console.Clear();
            Console.WriteLine("Simulating generation: " + currentGeneration);

            numSlotMachinesComplete = 0;

            for (int i = 0; i < population.Length; i++)
            {
                Task.Factory.StartNew(population[i].Simulate);
            }
        }

        /// <summary>
        /// Creates our initial population.
        /// </summary>
        public void createInitialPopulation()
        {
            SlotMachine slotMachine;
            Chromosome chromosome;

            population = new SlotMachine[environment.population];

            for(int i = 0; i < environment.population; i++)
            {
                // Create slot machine
                slotMachine = new SlotMachine(i, environment);

                // Create a first-generation chromosome
                chromosome = new Chromosome(environment);
                chromosome.CreateFirstGeneration();

                // Inject chromosome into slot machine
                slotMachine.InjectChromosome(chromosome);

                // Add events
                slotMachine.CompleteEvent += OnSlotMachineSimulationComplete;

                // Save in population
                population[i] = slotMachine;
            }
        }


        // EVENT HANDLERS
        // ==============================================================================

        /// <summary>
        /// An individual slot machine has completed simulations.
        /// </summary>
        /// <param name="sender"></param>
        private void OnSlotMachineSimulationComplete(object sender)
        {
            // Increment
            numSlotMachinesComplete++;

            // All complete
            if (numSlotMachinesComplete == population.Length)
            {
                // Pause
                Thread.Sleep(1000);

                // Handle completion
                HandleGenerationSimulationsCompleted();

                // Start new generation
                currentGeneration++;
                Simulate();
            }
        }


        // PRIVATE
        // ==============================================================================

        /// <summary>
        /// Handles all simulations completed for a single generation.
        /// </summary>
        private void HandleGenerationSimulationsCompleted()
        {
            // Create weighted values pool
            WeightedValues<SlotMachine> weightedValues = new WeightedValues<SlotMachine>();

            // Add all slot machines to pool weighted by fitness
            for(int i = 0; i < population.Length; i++)
            {
                weightedValues.AddValue(population[i], population[i].fitness);
            }

            // Create new generation
            Console.WriteLine();
            for(int i = 0; i < population.Length; i++)
            {
                SlotMachine a = weightedValues.GetValue();
                SlotMachine b = weightedValues.GetValue();

                Console.Write(a.id + "<->" + b.id + "  ");

                Chromosome child = breeder.Breed(a.chromosome, b.chromosome);

                population[i].chromosome = child;
            }
            Console.WriteLine();
        }
    }
}
