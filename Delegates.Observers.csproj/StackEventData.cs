using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.Observers
{
    public class StackEventData<T>
    {
        public bool IsPushed { get; set; }
        public T Value { get; set; }
        public override string ToString()
        {
            return (IsPushed ? "+" : "-") + Value.ToString();
        }
    }
}
