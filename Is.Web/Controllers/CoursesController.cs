using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Is.Domain.Identity;
using Is.Domain.DTO;
using Is.Domain.DomainModels;
using Is.Repository;
using Is.Services.Interface;

namespace Is.Web.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICourseService _courseService;
       
        public CoursesController(ICourseService courseService)
        {
            _courseService=courseService;
        }

        // GET: Courses
        public IActionResult Index()
        {
            var allCourses = this._courseService.GetAllCorses();
            return View(allCourses);
        }
        public IActionResult AddCourseToCard(Guid? id)
        {
           var model=this._courseService.GetCourseCardInfo(id); 
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCourseToCard([Bind("CourseId","Quantity")]AddCourseToCardDTO item)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this._courseService.AddToMyCoursesCard(item, userId);

            if (result)
            {
                return RedirectToAction("Index", "Courses");
            }

            return View(item);
        }

        // GET: Courses/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = this._courseService.GetDetailsForCourse(id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,CourseName,CourseImage,CourseDescription,Link,CoursePrice,Rating")] Course course)
        {
            if (ModelState.IsValid)
            {
                this._courseService.CreateNewCourse(course);
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = this._courseService.GetDetailsForCourse(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,CourseName,CourseImage,CourseDescription,Link,CoursePrice,Rating")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    this._courseService.UpdateExistingCourse(course);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = this._courseService.GetDetailsForCourse(id);
            
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            this._courseService.DeleteCourse(id);
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(Guid id)
        {
            return this._courseService.GetDetailsForCourse(id) != null;
        }


    }
}
