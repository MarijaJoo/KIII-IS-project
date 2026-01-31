using System.ComponentModel.DataAnnotations;

namespace Is.Domain.DomainModels
{
    public class CourseInMyCoursesCard : BaseEntity
    {
        public Guid CourseId { get; set; }
        [Required]
        public Course Course { get; set; }
        public Guid MyCourseCardId { get; set; }
        [Required]
        public MyCoursesCard MyCourseCard { get; set; }
        public int Quantity { get; set; }
    }
}
