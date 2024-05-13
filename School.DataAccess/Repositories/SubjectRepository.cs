using Microsoft.EntityFrameworkCore;
using School.DataAccess.Model;

namespace School.DataAccess.Repositories;

public class SubjectRepository : BaseEfRepository<Subject>, ISubjectRepository
{
    public SubjectRepository(DbContext dbContext) : base(dbContext)
    {
    }
}