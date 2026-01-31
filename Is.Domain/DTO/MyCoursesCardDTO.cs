using Is.Domain.DomainModels;

namespace Is.Domain.DTO
{
    public class MyCoursesCardDTO
    {
        public List<CourseInMyCoursesCard> courseInMyCourseCards { get; set; }
        public  double TotalPrice { get; set; }
    }
}
