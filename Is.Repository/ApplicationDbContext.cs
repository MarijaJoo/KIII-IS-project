using Is.Domain.DomainModels;
using Is.Domain.Email;
using Is.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Is.Repository
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<IsApplicationUser>(options)
    {
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<MyCoursesCard> MyCoursesCards { get; set; }
        public virtual DbSet<CourseInMyCoursesCard> CourseInMyCoursesCards { get; set; }
        public virtual DbSet<CourseInOrder> CourseInOrders { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<EmailMessage> EmailMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<Course>().Property(z => z.CourseName).IsRequired();
            builder.Entity<Course>().Property(z => z.Id).ValueGeneratedOnAdd();

            builder.Entity<MyCoursesCard>().Property(z => z.Id).ValueGeneratedOnAdd();

            //builder.Entity<CourseInMyCoursesCard>()
            //    .HasKey(z => new { z.CourseId, z.MyCourseCardId });

            builder.Entity<CourseInMyCoursesCard>()
                .HasOne(z => z.Course)
                .WithMany(z => z.CourseInMyCourseCards)
                .HasForeignKey(z => z.MyCourseCardId);

            builder.Entity<CourseInMyCoursesCard>()
                .HasOne(z => z.MyCourseCard)
                .WithMany(z => z.Courses)
                .HasForeignKey(z => z.CourseId);

            builder.Entity<IsApplicationUser>().Property(z => z.FirstName);
            builder.Entity<IsApplicationUser>().Property(z => z.LastName);
            builder.Entity<IsApplicationUser>().Property(z => z.Address);

            builder.Entity<MyCoursesCard>()
                .HasOne<IsApplicationUser>(z => z.Owner)
                .WithOne(z => z.UserCard)
                .HasForeignKey<MyCoursesCard>(z => z.OwnerId);

            builder.Entity<Order>().Property(z => z.Id).ValueGeneratedOnAdd();
            //builder.Entity<CourseInOrder>().HasKey(
            //    z => new { z.CourseId, z.OrderId }
            //    );
            builder.Entity<CourseInOrder>()
            .HasOne(z => z.SelectedCourse)
            .WithMany(z => z.courseInOrder)
            .HasForeignKey(z => z.CourseId);

            builder.Entity<CourseInOrder>()
                .HasOne(z => z.UserOrder)
                .WithMany(z => z.orderInCourse)
                .HasForeignKey(z => z.OrderId);
        }
    }
}
