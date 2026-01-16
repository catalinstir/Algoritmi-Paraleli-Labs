using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ex10
{
    public class MapArea
    {
        public string Alias { get; set; }
        public float[] Area { get; set; }

        public override string ToString()
        {
            return $"{Alias}: [{string.Join(", ", Area)}]";
        }
    }
}
