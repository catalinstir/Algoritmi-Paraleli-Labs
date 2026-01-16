namespace ex06.Generics
{
    public class SingletonRepository<T, T_Obj, T_Builder>
        where T : IRepository<T_Obj>
        where T_Obj : class
        where T_Builder: IRepositoryBuilder<T, T_Obj>, new()
    {
        private static SingletonRepository<T, T_Obj, T_Builder> _instance;

        private T _repository;
        private T_Builder _builder;

        private SingletonRepository()
        {
            _builder = new T_Builder();
        }

        public static SingletonRepository<T, T_Obj, T_Builder> GetInstance()
        {
            ////////////////////////////////////////////////////////////////////////////
            /// Implement your solution here
            if(_instance == null)
            {
                _instance = new SingletonRepository<T, T_Obj, T_Builder>();
                _instance._repository = _instance._builder.Build();
            }
            // Should we do this check?? Maybe...
            //if (_instance._repository == null)
            //{
            //    _instance._repository = _instance._builder.Build();
            //}
            ////////////////////////////////////////////////////////////////////////////
            
            return _instance;
        }

        public T Repository
        {
            get { return _repository; }
        }
    }
}
