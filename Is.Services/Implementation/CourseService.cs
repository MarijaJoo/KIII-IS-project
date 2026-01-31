using Is.Domain.DomainModels;
using Is.Domain.DTO;
using Is.Repository.Interface;
using Is.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Is.Services.Implementation
{
    public class CourseService : ICourseService
    {
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<CourseInMyCoursesCard> _courseInCoursesRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<CourseService> _logger;

        public CourseService(IRepository<Course> ic, IRepository<CourseInMyCoursesCard> courseInCoursesRepository, IUserRepository userRepository, ILogger<CourseService> logger)
        {
            _courseRepository = ic;
            _userRepository = userRepository;
            _courseInCoursesRepository = courseInCoursesRepository;
            _logger = logger;
            _logger.LogDebug(1, "NLog injected into CourseService");

        }
        public bool AddToMyCoursesCard(AddCourseToCardDTO item, string userID)
        {
            var user = this._userRepository.Get(userID);

            var userCourseCard = user.UserCard;

            if (item.CourseId != null && userCourseCard != null)
            {
                var course = this.GetDetailsForCourse(item.CourseId);

                if (course != null)
                {
                    CourseInMyCoursesCard itemToAdd = new CourseInMyCoursesCard
                    {
                        Id=Guid.NewGuid(),
                        Course = course,
                        CourseId = course.Id,
                        MyCourseCard = userCourseCard,
                        MyCourseCardId = userCourseCard.Id,
                        Quantity = item.Quantity
                    };
                    this._courseInCoursesRepository.Insert(itemToAdd);
                    _logger.LogInformation("Course was successfully added into the Card");
                    return true;
                }
                return false;
            }
            _logger.LogInformation("Something was wrong in AddToMyCoursesCard. It's eather the card or user is unavailable");
            return false;
        }

        public void CreateNewCourse(Course c)
        {
            this._courseRepository.Insert(c);
        }

        public void DeleteCourse(Guid id)
        {
            var course = this.GetDetailsForCourse(id);
            this._courseRepository.Delete(course);
        }

        public List<Course> GetAllCorses()
        {
            _logger.LogInformation("GetAllCourses was called!");
            return this._courseRepository.GetAll().ToList();
        }

        public AddCourseToCardDTO GetCourseCardInfo(Guid? id)
        {
            var course = this.GetDetailsForCourse(id);
            AddCourseToCardDTO model = new AddCourseToCardDTO
            {
                SelectedCourse= course,
                CourseId=course.Id,
                Quantity=1
            };
            return model;
        }

        public Course GetDetailsForCourse(Guid? id)
        {
            return this._courseRepository.Get(id);
        }

        public void UpdateExistingCourse(Course c)
        {
            this._courseRepository.Update(c);
        }
    }
}
