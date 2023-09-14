using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Discrete_Simulation_Population_2.Program.Constants;

namespace Discrete_Simulation_Population_2.Program
{
    internal class NumberGen
    {
        public static double SampleGaussian(double mean, double stddev)
        {
            Random random = new Random();
            double x1 = 1 - random.NextDouble();
            double x2 = 1 - random.NextDouble();

            double y1 = Math.Sqrt(-2.0 * Math.Log(x1)) * Math.Cos(2.0 * Math.PI * x2);
            return y1 * stddev + mean;
        }
        public static int GetPoisson(double lambda)
        {
            {
                Random x = new Random();
                // Algorithm due to Donald Knuth, 1969.
                double p = 1.0,
                L = Math.Exp(-lambda);
                int k = 0;
                while (p > L)
                {
                    k++;
                    p *= x.NextDouble();
                }

                return k - 1;
            }
        }
    }
}
