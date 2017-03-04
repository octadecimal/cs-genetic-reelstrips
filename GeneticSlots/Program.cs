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

            for(int i = 0; i < environment.population; i++)
            {
                // Create slot machine
                slotMachine = new SlotMachine(environment);

                // Create a first-generation chromosome
                chromosome = new Chromosome(environment);
                chromosome.CreateFirstGeneration();

                // Inject chromosome into slot machine
                slotMachine.InjectChromosome(chromosome);
            }
        }
    }
}
