namespace Models
{
    public class DarsJadvaliDto
    {
        public long id { get; set; }
        public SubjectDto? subject { get; set; }
        public SemesterDto? semester { get; set; }
        public EducationYearDto? educationYear { get; set; }
        public GroupDto? group { get; set; }
        public FacultyDto? faculty { get; set; }
        public DepartmentDto? department { get; set; }
        public AuditoriumDto? auditorium { get; set; }
        public TrainingTypeDto? trainingType { get; set; }
        public LessonPairDto? lessonPair { get; set; }
        public EmployeeDto? employee { get; set; }
        public long? weekStartTime { get; set; }
        public long? weekEndTime { get; set; }
        public long? lesson_date { get; set; }
        public long? updated_at { get; set; }
        public int? _week { get; set; }
    }

    public class SubjectDto
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? code { get; set; }
    }

    public class SemesterDto
    {
        public int id { get; set; }
        public string? code { get; set; }
        public string? name { get; set; }
    }

    public class EducationYearDto
    {
        public string? code { get; set; }
        public string? name { get; set; }
        public bool current { get; set; }
    }

    public class GroupDto
    {
        public int id { get; set; }
        public string? name { get; set; }
        public EducationLangDto? educationLang { get; set; }
    }

    public class EducationLangDto
    {
        public string? code { get; set; }
        public string? name { get; set; }
    }

    public class FacultyDto
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? code { get; set; }
    }

    public class DepartmentDto
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? code { get; set; }
    }

    public class AuditoriumDto
    {
        public int code { get; set; }
        public string? name { get; set; }
    }

    public class TrainingTypeDto
    {
        public string? code { get; set; }
        public string? name { get; set; }
    }

    public class LessonPairDto
    {
        public string? code { get; set; }
        public string? name { get; set; }
        public string? start_time { get; set; }
        public string? end_time { get; set; }
    }

    public class EmployeeDto
    {
        public int id { get; set; }
        public string? name { get; set; }
    }

    public class DarsJadvalResponse
    {
        public bool success { get; set; }
        public string? error { get; set; }
        public List<DarsJadvaliDto>? data { get; set; }
    }
}
