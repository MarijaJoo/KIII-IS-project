using Is.Domain.Identity;

namespace Is.Domain.DomainModels
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; }
        public IsApplicationUser User { get; set; }
        public virtual ICollection<CourseInOrder> orderInCourse { get; set; }

    }
}
