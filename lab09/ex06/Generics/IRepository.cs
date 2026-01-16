namespace ex06.Generics
{
    public interface IRepository<T>
    {
        List<T> Get();
        void Add(T item);
    }

    public interface IRepositoryBuilder<T, T_Obj>
        where T : IRepository<T_Obj>
        where T_Obj : class
    {
        T Build();
    }
}
