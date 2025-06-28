using StudentManagement.Web.Models;

namespace StudentManagement.Web.Repositories
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<Student?> GetStudentByIdAsync(int id);
        Task<Student?> GetStudentByEmailAsync(string email);
        Task<IEnumerable<Student>> GetStudentsByCourseAsync(int courseId);
        Task<Student> AddStudentAsync(Student student);
        Task<Student> UpdateStudentAsync(Student student);
        Task<bool> DeleteStudentAsync(int id);
        Task<bool> StudentExistsAsync(int id);
    }
} 