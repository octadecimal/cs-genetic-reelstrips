using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticSlots.Utilities
{
    /// <summary>
    /// Class that allows for weighted, noramlized values to be added
    /// and retrieved.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class WeightedValues<T>
    {
        private List<WeightedValue<T>> values;
        private Random rng;

        /// <summary>
        /// WeightedValues constructor.
        /// </summary>
        public WeightedValues()
        {
            values = new List<WeightedValue<T>>();
            rng = new Random();
        }


        // API
        // ==============================================================================

        /// <summary>
        /// Adds a value to the pool.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="weight"></param>
        public void AddValue(T value, double weight)
        {
            WeightedValue<T> v = new WeightedValue<T>();
            v.value = value;
            v.weight = weight;

            values.Add(v);

            Normalize();
        }

        /// <summary>
        /// Retrieves a weighted value from the pool.
        /// </summary>
        /// <returns></returns>
        public T GetValue()
        {
            double rand = rng.NextDouble();
            T output = values[0].value;

            for (int i = 0; i < values.Count; i++)
            {
                WeightedValue<T> value = values[i];

                if (rand <= value.threshold)
                {
                    output = value.value;
                    break;
                }
            }

            return output;
        }


        // PRIVATE
        // ==============================================================================

        /// <summary>
        /// Normalizes all added values in the pool.
        /// </summary>
        private void Normalize()
        {
            double sum = 0;

            foreach (WeightedValue<T> value in values)
            {
                sum += value.weight;
            }

            for (int i = 0; i < values.Count; i++)
            {
                WeightedValue<T> value = values[i];
                value.normalizedWeight = value.weight / sum;
            }

            sum = 0;

            for (int i = 0; i < values.Count; i++)
            {
                WeightedValue<T> value = values[i];
                sum += value.normalizedWeight;
                value.threshold = sum;
            }
        }
    }


    /// <summary>
    /// A single weighted value object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class WeightedValue<T>
    {
        public T value;
        public double weight;
        public double normalizedWeight;
        public double threshold;
    }
}
