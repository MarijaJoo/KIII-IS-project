using Is.Domain.DomainModels;
using Is.Domain.DTO;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Is.Services.Interface
{
    public interface ICourseService
    {
        List<Course> GetAllCorses();
        Course GetDetailsForCourse(Guid? id);
        void CreateNewCourse(Course c);
        void UpdateExistingCourse(Course c);
        AddCourseToCardDTO GetCourseCardInfo(Guid? id);
        void DeleteCourse(Guid id);
        bool AddToMyCoursesCard(AddCourseToCardDTO item, string userID);

    }
}
