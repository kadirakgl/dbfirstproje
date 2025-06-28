using Microsoft.EntityFrameworkCore;
using StudentManagement.Web.Models;

namespace StudentManagement.Web.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentDbContext _context;

        public StudentRepository(StudentDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _context.Students
                .Include(s => s.StudentCourses)
                .ThenInclude(sc => sc.Course)
                .Where(s => s.IsActive == true)
                .ToListAsync();
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            return await _context.Students
                .Include(s => s.StudentCourses)
                .ThenInclude(sc => sc.Course)
                .FirstOrDefaultAsync(s => s.StudentId == id && s.IsActive == true);
        }

        public async Task<Student?> GetStudentByEmailAsync(string email)
        {
            return await _context.Students
                .Include(s => s.StudentCourses)
                .ThenInclude(sc => sc.Course)
                .FirstOrDefaultAsync(s => s.Email == email && s.IsActive == true);
        }

        public async Task<IEnumerable<Student>> GetStudentsByCourseAsync(int courseId)
        {
            return await _context.Students
                .Include(s => s.StudentCourses)
                .ThenInclude(sc => sc.Course)
                .Where(s => s.IsActive == true && s.StudentCourses.Any(sc => sc.CourseId == courseId))
                .ToListAsync();
        }

        public async Task<Student> AddStudentAsync(Student student)
        {
            student.EnrollmentDate = DateTime.Now;
            student.IsActive = true;
            
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<Student> UpdateStudentAsync(Student student)
        {
            var existingStudent = await _context.Students.FindAsync(student.StudentId);
            if (existingStudent == null)
                throw new ArgumentException("Öğrenci bulunamadı");

            existingStudent.FirstName = student.FirstName;
            existingStudent.LastName = student.LastName;
            existingStudent.Email = student.Email;
            existingStudent.DateOfBirth = student.DateOfBirth;

            await _context.SaveChangesAsync();
            return existingStudent;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return false;

            student.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> StudentExistsAsync(int id)
        {
            return await _context.Students.AnyAsync(s => s.StudentId == id && s.IsActive == true);
        }
    }
} 