using Is.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Is.Web.Controllers
{
    public class MyCoursesCardsController : Controller
    {
        private readonly IMyCoursesCardService _cardService;
        public MyCoursesCardsController(IMyCoursesCardService cardService)
        {
            _cardService= cardService;
        }
        public IActionResult Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userShoppingCart = _cardService.GetByUserIdWithIncludedCourses(Guid.Parse(userId));
            return View(userShoppingCart);
        }

        public IActionResult DeleteCourseFromMyCoursesCard(Guid id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _cardService.DeleteCourseFromMyCoursesCard(userId, id);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult OrderNow()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                throw new Exception("Log in");
            }

            _cardService.OrderCourses(Guid.Parse(userId));

            return RedirectToAction("Index", "MyCoursesCards");
        }

    }


}