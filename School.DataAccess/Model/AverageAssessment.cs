namespace School.DataAccess.Model;

public class AverageAssessment
{
    public string SubjectName { get; set; } = null!;

    public string StudentFirstName { get; set; } = null!;

    public string StudentLastName { get; set; } = null!;

    public ushort SubjectMark { get; set; } 
}