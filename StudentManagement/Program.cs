using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models;
using StudentManagement.Repositories;

namespace StudentManagement
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Öğrenci Yönetim Sistemi (Database First) ===\n");

            // DbContext oluştur
            using var context = new StudentDbContext();
            
            // Repository oluştur
            var studentRepository = new StudentRepository(context);

            try
            {
                // Veritabanı bağlantısını test et
                await context.Database.EnsureCreatedAsync();
                Console.WriteLine("✓ Veritabanı bağlantısı başarılı!\n");

                // 1. Tüm öğrencileri listele
                Console.WriteLine("1. TÜM ÖĞRENCİLER:");
                Console.WriteLine("==================");
                var students = await studentRepository.GetAllStudentsAsync();
                foreach (var student in students)
                {
                    Console.WriteLine($"ID: {student.StudentId}, Ad: {student.FirstName} {student.LastName}, Email: {student.Email}");
                    if (student.StudentCourses.Any())
                    {
                        Console.WriteLine("  Kayıtlı Kurslar:");
                        foreach (var course in student.StudentCourses)
                        {
                            Console.WriteLine($"    - {course.Course.CourseName} (Not: {course.Grade?.ToString() ?? "Henüz girilmemiş"})");
                        }
                    }
                    Console.WriteLine();
                }

                // 2. Yeni öğrenci ekle
                Console.WriteLine("2. YENİ ÖĞRENCİ EKLEME:");
                Console.WriteLine("=======================");
                var newStudent = new Student
                {
                    FirstName = "Zeynep",
                    LastName = "Arslan",
                    Email = "zeynep.arslan@email.com",
                    DateOfBirth = new DateTime(2002, 7, 12)
                };

                var addedStudent = await studentRepository.AddStudentAsync(newStudent);
                Console.WriteLine($"✓ Yeni öğrenci eklendi: {addedStudent.FirstName} {addedStudent.LastName} (ID: {addedStudent.StudentId})\n");

                // 3. Öğrenci güncelle
                Console.WriteLine("3. ÖĞRENCİ GÜNCELLEME:");
                Console.WriteLine("======================");
                var studentToUpdate = await studentRepository.GetStudentByIdAsync(1);
                if (studentToUpdate != null)
                {
                    studentToUpdate.FirstName = "Ahmet Mehmet";
                    var updatedStudent = await studentRepository.UpdateStudentAsync(studentToUpdate);
                    Console.WriteLine($"✓ Öğrenci güncellendi: {updatedStudent.FirstName} {updatedStudent.LastName}\n");
                }

                // 4. Email ile öğrenci ara
                Console.WriteLine("4. EMAIL İLE ÖĞRENCİ ARAMA:");
                Console.WriteLine("============================");
                var foundStudent = await studentRepository.GetStudentByEmailAsync("ayse.demir@email.com");
                if (foundStudent != null)
                {
                    Console.WriteLine($"✓ Öğrenci bulundu: {foundStudent.FirstName} {foundStudent.LastName} (ID: {foundStudent.StudentId})");
                    Console.WriteLine($"  Doğum Tarihi: {foundStudent.DateOfBirth?.ToShortDateString() ?? "Belirtilmemiş"}");
                    Console.WriteLine($"  Kayıt Tarihi: {foundStudent.EnrollmentDate.ToShortDateString()}\n");
                }

                // 5. Kurs bazında öğrenci listesi
                Console.WriteLine("5. KURSA GÖRE ÖĞRENCİLER:");
                Console.WriteLine("=========================");
                var studentsInMath = await studentRepository.GetStudentsByCourseAsync(1); // Matematik kursu
                Console.WriteLine("Matematik kursuna kayıtlı öğrenciler:");
                foreach (var student in studentsInMath)
                {
                    var mathGrade = student.StudentCourses.FirstOrDefault(sc => sc.CourseId == 1)?.Grade;
                    Console.WriteLine($"  - {student.FirstName} {student.LastName} (Not: {mathGrade?.ToString() ?? "Henüz girilmemiş"})");
                }
                Console.WriteLine();

                // 6. Öğrenci silme (soft delete)
                Console.WriteLine("6. ÖĞRENCİ SİLME:");
                Console.WriteLine("==================");
                var deleteResult = await studentRepository.DeleteStudentAsync(5); // Zeynep Arslan'ı sil
                if (deleteResult)
                {
                    Console.WriteLine("✓ Öğrenci başarıyla silindi (soft delete)\n");
                }

                // 7. Güncel öğrenci listesi
                Console.WriteLine("7. GÜNCEL ÖĞRENCİ LİSTESİ:");
                Console.WriteLine("===========================");
                var currentStudents = await studentRepository.GetAllStudentsAsync();
                Console.WriteLine($"Toplam aktif öğrenci sayısı: {currentStudents.Count()}");
                foreach (var student in currentStudents)
                {
                    Console.WriteLine($"  - {student.FirstName} {student.LastName} ({student.Email})");
                }

                Console.WriteLine("\n=== Program tamamlandı ===");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                Console.WriteLine($"Detay: {ex.InnerException?.Message}");
            }

            Console.WriteLine("\nÇıkmak için herhangi bir tuşa basın...");
            Console.ReadKey();
        }
    }
} 