using GeneticSlots.Genes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        // Our current generation.
        int currentGeneration = 0;


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

            // Create initial population
            createInitialPopulation();

            /// Temp ///
            for(int i = 0; i < population.Length; i++)
            {
                population[i].Simulate();
            }

            // Wait
            Console.ReadKey();
        }


        // API
        // ==============================================================================

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

                // Save in population
                population[i] = slotMachine;
            }
        }
    }
}
