using Is.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Is.Repository.Interface
{
    public interface IUserRepository
    {
        IEnumerable<IsApplicationUser> GetAll();
        IsApplicationUser Get(string? id);
        void Insert(IsApplicationUser entity);
        void Update(IsApplicationUser entity);
        void Delete(IsApplicationUser entity);
    }
}
