using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reflection.Randomness
{
    public class ExponentialDistribution : IContinousDistribution
    {
        public readonly double Lambda;

        public ExponentialDistribution(double lambda)
        {
            Lambda = lambda;
        }

        public double Generate(Random rnd)
        {
            var u = rnd.NextDouble();
            return -Math.Log(u) / Lambda;
        }
    }
}
