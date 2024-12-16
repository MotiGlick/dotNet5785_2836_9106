

namespace BlApi;

public interface IVolunteer
{
    BO.job enterSystem(int id , int passward);
    void Create(BO.Volunteer boVolunreer);
    BO.Volunteer? Read(int id);

    IEnumerable<BO.VolunteerInList> ReadAll(bool? isActive, Enum? sortField);
    void Update(BO.Volunteer boVolunreer ,int id);
    void Delete(int id);

    //void RegisterStudentToCourse(int volunreerId, int callId);
    //void UnRegisterStudentFromCourse(int volunreerId, int callId);

    //IEnumerable<BO.CallInList> GetRegisteredCoursesForStudent(int volunreerId, BO.Year year = BO.Year.None);
    //IEnumerable<BO.CallInList> GetUnRegisteredCoursesForStudent(int volunreerId, BO.Year year = BO.Year.None);

    //BO.VolunteerGradeSheet GetGradeSheetPerStudent(int volunreerId, BO.Year year = BO.Year.None);
    //void UpdateGrade(int volunreerId, int callId, double grade);



}
