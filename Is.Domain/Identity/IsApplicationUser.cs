using Is.Domain.DomainModels;
using Microsoft.AspNetCore.Identity;

namespace Is.Domain.Identity
{
    public class IsApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }

        public virtual MyCoursesCard? UserCard { get; set; }
    }
}
