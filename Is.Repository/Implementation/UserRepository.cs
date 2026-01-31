using Is.Domain.Identity;
using Is.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Is.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<IsApplicationUser> entities;
        string errorMessage;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
            entities = context.Set<IsApplicationUser>();
            errorMessage = string.Empty;
        }

        public void Delete(IsApplicationUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            _context.SaveChanges();
        }

        public IsApplicationUser Get(string id)
        {
            return entities.
                Include(z=>z.UserCard)
                .ThenInclude(c=>c.Courses)
                .ThenInclude(m=>m.Course)
                .SingleOrDefault(s => s.Id==id);
        }

        public IEnumerable<IsApplicationUser> GetAll()
        {
            return entities.AsEnumerable();
        }

        public void Insert(IsApplicationUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            _context.SaveChanges();
        }

        public void Update(IsApplicationUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            _context.SaveChanges();
        }
    }
}
