using Is.Domain.DomainModels;
using Is.Domain.Identity;
using Is.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Is.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<IsApplicationUser> _userManager;
        private readonly IOrderService _orderService;
        public AdminController(UserManager<IsApplicationUser> userManager, IOrderService orderService)
        {
           this. _userManager = userManager;
           this. _orderService = orderService;
        }
        [HttpGet("[action]")]
        public List<Order> GetAllActiveOrders()
        {
            return this._orderService.GetAllOrders();
        }
        [HttpPost("[action]")]
        public Order GetAllDetailsForOrder(BaseEntity model)
        {
            return this._orderService.GetOrderDetails(model);
        }
        [HttpPost("[action]")]
        public bool ImportAllUsers(List<UserRegistrationDto> model)
        {
            bool status = true;
            foreach (var item in model)
            {
                var userCheck = _userManager.FindByEmailAsync(item.Email).Result;
                if(userCheck == null)
                {
                    var user = new IsApplicationUser
                    {
                        UserName = item.Email,
                        NormalizedUserName = item.Email,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        UserCard = new Domain.DomainModels.MyCoursesCard()

                    };
                    var result = _userManager.CreateAsync(user, item.Password).Result;

                    status = status && result.Succeeded;
                }
                else
                {
                    continue;
                }
            }
            return status;
        }

    }
}
