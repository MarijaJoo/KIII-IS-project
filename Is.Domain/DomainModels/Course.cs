//using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
namespace Is.Domain.DomainModels
{
    public class Course :BaseEntity
    {
        [Required]
        public string CourseName { get; set; }
        [Required]
        public string CourseImage { get; set; }
        [Required]
        public string CourseDescription { get; set; }
        [Required]
        public string Link { get; set; }
        [Required]
        public int CoursePrice { get; set; }
        [Required]
        public int Rating { get; set; }
        public virtual ICollection<CourseInMyCoursesCard>? CourseInMyCourseCards { get; set; }
        public virtual ICollection<CourseInOrder>? courseInOrder { get; set; }

    }
}
