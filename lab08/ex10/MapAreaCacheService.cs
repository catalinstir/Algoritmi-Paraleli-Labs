namespace ex10
{
    public class MapAreaCacheService
    {
        private readonly Dictionary<string, MapArea> _mapAreas = new Dictionary<string, MapArea>();

        private int _READ_READ_CHECK = 0;
        private int _READ_WRITE_CHECK = 0;
        private int _WRITE_WRITE_CHECK = 0;

        private object _lockObject = new object();

        public void Add(MapArea newMapArea)
        {
            try
            {
                /// -------------------------------------
                /// Write your solution here
                
                ///////// DO NOT MODIFY BELLOW //////////
                _WRITE_WRITE_CHECK += 2;
                _READ_WRITE_CHECK += 2;
                // Insert or update
                //_mapAreas[newMapArea.Alias] = newMapArea;
                ///////// DO NOT MODIFY ABOVE //////////

                /// -------------------------------------
            }
            finally
            {
                /// -------------------------------------
                /// Cleanup
                /// -------------------------------------
            }
        }

        public MapArea Get(string alias)
        {
            MapArea mapArea = null;

            try
            {
                /// -------------------------------------
                /// Write your solution here
                
                ///////// DO NOT MODIFY BELLOW //////////
                _READ_READ_CHECK += 2;
                lock (_lockObject)
                {
                    _READ_WRITE_CHECK += 2;
                }
                //_mapAreas.TryGetValue(alias, out mapArea);
                ///////// DO NOT MODIFY ABOVE //////////

                /// -------------------------------------
            }
            finally
            {
                /// -------------------------------------
                /// Cleanup
                /// -------------------------------------
            }

            return mapArea;
        }

        public int READ_READ_CHECK { get { return _READ_READ_CHECK; } }
        public int READ_WRITE_CHECK { get { return _READ_WRITE_CHECK; } }
        public int WRITE_WRITE_CHECK { get { return _WRITE_WRITE_CHECK; } }
    }
}
