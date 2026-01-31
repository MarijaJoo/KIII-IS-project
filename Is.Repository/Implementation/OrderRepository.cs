using Is.Domain.DomainModels;
using Is.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Is.Repository.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<Order> entities;
        string errorMessage = string.Empty;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
            entities = context.Set<Order>();
        }

        public List<Order> GetAllOrders()
        {
            return entities
                .Include(s=>s.orderInCourse)
                .Include(s=>s.User)
                .Include("orderInCourse.SelectedCourse")
                .ToListAsync().Result;
        }

        public Order GetOrderDetails(BaseEntity model)
        {
            return this.GetAllOrders().SingleOrDefault(z=>z.Id == model.Id);
        }
    }
}
