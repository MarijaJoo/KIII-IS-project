using Is.Domain.Identity;

namespace Is.Domain.DomainModels
{
    public class MyCoursesCard : BaseEntity
    {
        public virtual IsApplicationUser Owner { get; set; }
        public virtual ICollection<CourseInMyCoursesCard> Courses { get; set; }

        public string OwnerId { get; set; }
    
        public MyCoursesCard()
        {

        }
    }
}
