using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reflection.Randomness
{
    public class NormalDistribution : IContinousDistribution
    {
        public readonly double Sigma;
        public readonly double Mean;

        public NormalDistribution()
            : this(0.0, 1.0)
        {
        }

        public NormalDistribution(double mean, double sigma)
        {
            Sigma = sigma;
            Mean = mean;
        }

        public double Generate(Random rnd)
        {
            var u = rnd.NextDouble();
            var v = rnd.NextDouble();
            var x = Math.Sqrt(-2 * Math.Log(u)) * Math.Cos(2 * Math.PI * v);
            return x * Sigma + Mean;
        }
    }
}
