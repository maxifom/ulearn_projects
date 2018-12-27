using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.PairsAnalysis
{
    public abstract class PairsAnalyzer<TSource,TIntermediate,TResult>
    {
        public TResult Analyze(TSource[] data)
        {
            if (data.Length < 2)
                throw new ArgumentException();
            var temp = new List<TIntermediate>();
            for (int i = 0; i < data.Length - 1; i++)
                temp.Add(Process(data[i], data[i + 1]));
            return Aggregate(temp);
        }

        protected abstract TResult Aggregate(List<TIntermediate> temp);
        protected abstract TIntermediate Process(TSource source1, TSource source2);
    }
}
