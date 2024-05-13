using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using School.DataAccess.Repositories;
using System.Runtime.Remoting.Contexts;

namespace School.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;

        private IDbContextTransaction? _transaction;

        private bool _disposed;

        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new Exception("DbContext is null!");

            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();
        }

        public T GetRepository<T>() where T : class, IRepository
        {
            if (_disposed)
            {
                throw new MemberAccessException("UnitOfWork is already disposed!");
            }

            if (typeof(T) == typeof(IAssessmentRepository))
            {
                return (new AssessmentRepository(_dbContext) as T)!;
            }

            if (typeof(T) == typeof(IStudentRepository))
            {
                return (new StudentRepository(_dbContext) as T)!;
            }

            if (typeof(T) == typeof(ISubjectRepository))
            {
                return (new SubjectRepository(_dbContext) as T)!;
            }

            throw new Exception($"Repository {typeof(T)} does not exist!");
        }

        public void BeginTransaction()
        {
            if (_disposed)
            {
                throw new MemberAccessException("UnitOfWork is already disposed!");
            }

            if (_transaction != null)
            {
                throw new Exception("There is already an existing transaction!");
            }

            _transaction = _dbContext.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (_disposed)
            {
                throw new MemberAccessException("UnitOfWork is already disposed!");
            }

            if (_transaction == null)
            {
                return;
            }

            _transaction.Commit();

            _transaction = null;
        }

        public void RollbackTransaction()
        {
            if (_disposed)
            {
                throw new MemberAccessException("UnitOfWork is already disposed!");
            }

            if (_transaction == null)
            {
                return;
            }

            _transaction.Rollback();

            _transaction = null;
        }

        public void Save()
        {
            if (_disposed)
            {
                throw new MemberAccessException("UnitOfWork is already disposed!");
            }

            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _disposed = true;

            _dbContext.Dispose();
        }
    }
}
