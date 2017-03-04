using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_genetic_reelstrips
{
    class WeightedValues<T>
    {
        private List<WeightedValue<T>> values;
        private Random rng;

        public WeightedValues()
        {
            values = new List<WeightedValue<T>>();
            rng = new Random();
        }

        public void AddValue(T value, double weight)
        {
            WeightedValue<T> v = new WeightedValue<T>();
            v.value = value;
            v.weight = weight;

            values.Add(v);

            Normalize();
        }

        public T getValue()
        {
            double rand = rng.NextDouble();
            T output = values[0].value;

            //Console.WriteLine("RAND: " + rand);
            
            for(int i = 0; i < values.Count; i++)
            {
                WeightedValue<T> value = values[i];

                //Console.WriteLine(rand + " <= " + value.threshold);

                if(rand <= value.threshold)
                {
                    output = value.value;
                    break;
                }
            }

            return output;
        }

        public void Normalize()
        {
            double sum = 0;

            foreach(WeightedValue<T> value in values)
            {
                sum += value.weight;
            }

            for(int i = 0; i < values.Count; i++)
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

        public void PrintThresholds()
        {
            for(int i = 0; i < values.Count; i++)
            {
                Console.WriteLine(i + ": " + values[i].threshold);
            }
        }
    }


    class WeightedValue<T>
    {
        public T value;
        public double weight;
        public double normalizedWeight;
        public double threshold;
    }
}
