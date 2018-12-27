using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryTasks;
using System.Drawing;

namespace GeometryPainting
{
    public static class SegmentExtensions
    {
        private static readonly Dictionary<Segment, Color> SegmentColor = new Dictionary<Segment, Color>();
        public static void SetColor(this Segment segment, Color color)
        {
            SegmentColor[segment] = color;
        }
        
        public static Color GetColor(this Segment segment)
        {
            SegmentColor.TryGetValue(segment, out Color color);
            return color;
        }
    }
}
