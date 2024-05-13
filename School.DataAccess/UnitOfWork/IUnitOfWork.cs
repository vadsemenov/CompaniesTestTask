using System;
using System.Collections.Generic;
using System.Text;
using School.DataAccess.Repositories;

namespace School.DataAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();

        T GetRepository<T>() where T : class, IRepository;
    }
}
