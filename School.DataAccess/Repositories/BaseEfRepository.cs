using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Linq;

namespace School.DataAccess.Repositories;

public abstract class BaseEfRepository<T> : IMainRepository<T> where T : class
{
    protected DbContext DbContext;

    protected DbSet<T> DbSet;

    protected BaseEfRepository(DbContext dbContext)
    {
        DbContext = dbContext ?? throw new Exception("DbContext is null!"); ;

        DbSet = dbContext.Set<T>();
    }

    public virtual void Save() => DbContext.SaveChanges();

    public virtual void Create(T entity)
    {
        DbSet.Add(entity);
    }

    public virtual void Create(ICollection<T> entities)
    {
        DbSet.AddRange(entities);
    }

    public virtual void Update(T entity)
    {
        DbSet.Attach(entity);
        DbContext.Entry(entity).State = EntityState.Modified;
    }

    public virtual void Delete(T entity)
    {
        if (DbContext.Entry(entity).State == EntityState.Detached)
        {
            DbSet.Attach(entity);
        }

        DbSet.Remove(entity);
    }

    public virtual T[] GetAll() => DbSet.ToArray();

    public virtual T GetById(int id) => DbSet.Find(id)!;
}