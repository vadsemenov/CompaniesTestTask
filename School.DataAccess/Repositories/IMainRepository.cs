using System.Collections.Generic;

namespace School.DataAccess.Repositories;

public interface IMainRepository<T> : IRepository where T : class
{
    void Create(T entity);

    void Create(ICollection<T> entities);

    void Update(T entity);

    void Delete(T entity);

    T[] GetAll();

    T GetById(int id);
}