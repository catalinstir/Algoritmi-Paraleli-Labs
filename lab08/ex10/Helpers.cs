using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ex10
{
    public static class Helpers
    {
        private static string[] _aliases = { "Bucharest", "Budapest", "Kiev", "Varsovia", "Viena", "Stockholm" };

        public static MapArea GenerateMapArea()
        {
            int areaPointCount = Random.Shared.Next(100, 1000);

            MapArea mapArea = new MapArea()
            {
                Alias = GetRandomAlias(),
                Area = Enumerable.Range(0, areaPointCount).Select(_ => Random.Shared.NextSingle() * 1000).ToArray()
            };

            return mapArea;
        }

        public static string GetRandomAlias()
        {
            return _aliases[Random.Shared.Next() % _aliases.Length];
        }
    }
}
