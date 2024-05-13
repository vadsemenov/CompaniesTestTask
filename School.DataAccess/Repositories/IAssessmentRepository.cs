using System.Collections.Generic;
using School.DataAccess.Model;

namespace School.DataAccess.Repositories;

public interface IAssessmentRepository : IMainRepository<Assessment>
{
    List<AverageAssessment>  GetAverageAssessments(Subject subject);
}