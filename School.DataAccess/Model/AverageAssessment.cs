namespace School.DataAccess.Model;

public class AverageAssessment
{
    public string SubjectName { get; set; } = string.Empty;

    public string StudentFirstName { get; set; } = string.Empty;

    public string StudentLastName { get; set; } = string.Empty;

    public ushort SubjectMark { get; set; } 
}