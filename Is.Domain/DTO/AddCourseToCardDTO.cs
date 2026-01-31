using Is.Domain.DomainModels;

namespace Is.Domain.DTO
{
    public class AddCourseToCardDTO
    {
        public Course SelectedCourse { get; set; }
        public Guid CourseId { get; set; }
        public int Quantity { get; set; }
    }
}
