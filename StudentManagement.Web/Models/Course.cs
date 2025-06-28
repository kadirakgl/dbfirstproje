using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Web.Models
{
    [Table("Courses")]
    public partial class Course
    {
        public Course()
        {
            StudentCourses = new HashSet<StudentCourse>();
        }

        [Key]
        public int CourseId { get; set; }

        [Required]
        [StringLength(100)]
        public string CourseName { get; set; } = null!;

        [StringLength(500)]
        public string? Description { get; set; }

        public int Credits { get; set; }

        public bool IsActive { get; set; }

        // Navigation property
        public virtual ICollection<StudentCourse> StudentCourses { get; set; }
    }
} 