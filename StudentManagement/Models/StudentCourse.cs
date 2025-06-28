using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Models
{
    [Table("StudentCourses")]
    public partial class StudentCourse
    {
        [Key]
        public int StudentCourseId { get; set; }

        public int StudentId { get; set; }

        public int CourseId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime EnrollmentDate { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? Grade { get; set; }

        // Navigation properties
        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; } = null!;

        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; } = null!;
    }
} 