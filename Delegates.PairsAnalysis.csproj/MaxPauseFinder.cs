using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.PairsAnalysis
{
    public class MaxPauseFinder : PairsAnalyzer<DateTime, double, int>
    {
        protected override int Aggregate(List<double> temp)
        {
            if (temp.Count == 0) throw new ArgumentException();
            var max = temp[0];
            var bestIndex = 0;
            for (int i=1;i<temp.Count;i++)
                if (temp[i]>max)
                {
                    max = temp[i];
                    bestIndex = i;
                }
            return bestIndex;
        }

        protected override double Process(DateTime source1, DateTime source2)
        {
            return (source2 - source1).TotalSeconds;
        }
    }
}
