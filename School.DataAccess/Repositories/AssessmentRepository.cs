using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using School.DataAccess.Model;

namespace School.DataAccess.Repositories;

public class AssessmentRepository : BaseEfRepository<Assessment>, IAssessmentRepository
{
    public AssessmentRepository(DbContext dbContext) : base(dbContext)
    {
    }

    public List<AverageAssessment> GetAverageAssessments(Subject? subject)
    {
        if (subject == null)
        {
            return new List<AverageAssessment>();
        }

        // не подключал proxies, поэтому несколько запросов
        var students = base.DbContext.Set<Student>().ToList();

        return DbSet.Where(a => a.SubjectId == subject.Id)
            .Select(a =>
                new AverageAssessment()
                {
                    StudentFirstName = students.First(s => s.Id == a.StudentId).FirstName,
                    StudentLastName = students.First(s => s.Id == a.StudentId).LastName,
                    SubjectName = subject.Name,
                    SubjectMark = a.Mark
                }
            )
            .ToList();
    }
}
