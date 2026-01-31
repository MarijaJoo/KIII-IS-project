using Is.Domain.DomainModels;
using Is.Domain.DTO;
using Is.Domain.Email;
using Is.Repository.Interface;
using Is.Services.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Is.Services.Implementation
{
    public class MyCoursesCardService : IMyCoursesCardService
    {
        private readonly IRepository<MyCoursesCard> _myCoursesCardRepository;
        private readonly IRepository<CourseInMyCoursesCard> _courseInMyCoursesCardRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<CourseInOrder> _courseInOrderRepository;
        private readonly IRepository<EmailMessage> _emailRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public MyCoursesCardService(IRepository<MyCoursesCard> myCoursesCardRepository, IRepository<CourseInMyCoursesCard> courseInMyCoursesCardRepository, IRepository<Order> orderRepository, IRepository<CourseInOrder> courseInOrderRepository, IUserRepository userRepository, IEmailService emailService, IRepository<EmailMessage> emailRepository)
        {
            _myCoursesCardRepository = myCoursesCardRepository;
            _courseInMyCoursesCardRepository = courseInMyCoursesCardRepository;
            _orderRepository = orderRepository;
            _courseInOrderRepository = courseInOrderRepository;
            _userRepository = userRepository;
            _emailService = emailService;
            _emailRepository = emailRepository;
        }
        [HttpPost]
        public void DeleteCourseFromMyCoursesCard(string userId, Guid id)
        {
           if(!string.IsNullOrEmpty(userId) && id != null)
            {
                var loggedInUser = this._userRepository.Get(userId);
                //var userCard = loggedInUser.UserCard;
                var userCard = _myCoursesCardRepository.GetAll().Include(z => z.Courses).FirstOrDefault(x => x.OwnerId == userId);
                if (userCard != null)
                {
                    var itemToDelete = userCard.Courses.FirstOrDefault(z => z.CourseId==id);

                    if (itemToDelete != null)
                    {
                        userCard.Courses.Remove(itemToDelete);
                        this._myCoursesCardRepository.Update(userCard);
                    }
                }
            }
        }

        public MyCoursesCard? GetByUserId(Guid userId)
        {
            return _myCoursesCardRepository.GetAll().SingleOrDefault(x => x.OwnerId==userId.ToString());
        }

        public MyCoursesCardDTO GetByUserIdWithIncludedCourses(Guid userId)
        {

            var userCart = _userRepository.Get(userId.ToString()).UserCard;
            //treba da se dodade za ako nema nishto vo kartickata 

            var allProducts = userCart.Courses.ToList();

            var allProductPrices = allProducts.Select(z => new
            {
                ProductPrice = z.Course.CoursePrice,
                Quantity = z.Quantity
            }).ToList();

            double totalPrice = 0.0;

            foreach (var item in allProductPrices)
            {
                totalPrice += item.Quantity * item.ProductPrice;
            }

            MyCoursesCardDTO model = new MyCoursesCardDTO
            {
                courseInMyCourseCards = allProducts,
                TotalPrice = totalPrice
            };

            return model;
        }

        public bool OrderCourses(Guid userId)
        {
            var loggedInUser = _userRepository.Get(userId.ToString());

            var userCart = loggedInUser.UserCard;

            var emailMessage = new EmailMessage();

            emailMessage.MailTo = loggedInUser.Email;
            emailMessage.Subject = "Successfully created order";
            emailMessage.status = false;

            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = loggedInUser.Id,
                User = loggedInUser
            };

            _orderRepository.Insert(order);

            var productInOrder = userCart.Courses.Select(x => new CourseInOrder
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                UserOrder = order,
                CourseId = x.CourseId,
                SelectedCourse = x.Course,
                Quantity=x.Quantity
            }).ToList();

            StringBuilder sb = new StringBuilder();

            var totalPrice = 0.0;

            sb.AppendLine("Your order is completed. The order conatins: ");

            for (int i = 1; i <= productInOrder.Count(); i++)
            {
                var currentItem = productInOrder[i - 1];
                totalPrice += currentItem.Quantity * currentItem.SelectedCourse.CoursePrice;
                sb.AppendLine(i.ToString() + ". " + currentItem.SelectedCourse.CourseName + " with quantity of: " + currentItem.Quantity + " and price of: $" + currentItem.SelectedCourse.CoursePrice);
            }

            sb.AppendLine("Total price for your order: " + totalPrice.ToString());
            emailMessage.Content = sb.ToString();

            this._emailRepository.Insert(emailMessage);

            //_productInOrderRepository.InsertMany(productInOrder);
            foreach (var item in productInOrder)
            {
                _courseInOrderRepository.Insert(item);
            }

            userCart.Courses.Clear();

            _myCoursesCardRepository.Update(userCart);
            this._userRepository.Update(loggedInUser);

            return true;
        }
    }
}
