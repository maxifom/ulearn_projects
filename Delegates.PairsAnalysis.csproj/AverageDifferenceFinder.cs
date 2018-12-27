using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.PairsAnalysis
{
    public class AverageDifferenceFinder : PairsAnalyzer<double, double, double>
    {
        protected override double Aggregate(List<double> temp)
        {
            var sum = 0.0;
            for (int i = 0; i < temp.Count; i++)
                sum += temp[i];
            return sum / temp.Count;
        }

        protected override double Process(double source1, double source2)
        {
            return (source2 - source1) / source1;
        }
    }
}
