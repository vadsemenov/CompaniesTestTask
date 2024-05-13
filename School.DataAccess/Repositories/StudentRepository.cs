using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using School.DataAccess.Model;

namespace School.DataAccess.Repositories;

public class StudentRepository : BaseEfRepository<Student>, IStudentRepository
{
    public StudentRepository(DbContext dbContext) : base(dbContext)
    {
    }
}