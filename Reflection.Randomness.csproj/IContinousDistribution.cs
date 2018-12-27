using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reflection.Randomness
{
    public interface IContinousDistribution
    {
        double Generate(Random rnd);
    }
}
