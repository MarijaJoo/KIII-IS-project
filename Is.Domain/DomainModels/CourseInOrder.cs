namespace Is.Domain.DomainModels
{
    public class CourseInOrder : BaseEntity
    {
        public Guid CourseId { get; set; }
        public Course SelectedCourse { get; set; }
        public Guid OrderId { get; set; }
        public Order UserOrder { get; set; }
        public int Quantity { get; set; }
    }
}
