using Is.Domain.DomainModels;
using Is.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Is.Services.Interface
{
    public interface IMyCoursesCardService
    {
        MyCoursesCard? GetByUserId(Guid userId);
        MyCoursesCardDTO GetByUserIdWithIncludedCourses(Guid userId);
        void DeleteCourseFromMyCoursesCard(string userId, Guid courseInMyCoursesCardId);
        Boolean OrderCourses(Guid userId);
    }
}
