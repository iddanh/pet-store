using pet_store.Data;
using pet_store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pet_store.Services
{
    public class UsersService
    {
        private readonly PetStoreDBContext _context;

        public UsersService(PetStoreDBContext context)
        {
            _context = context;
        }

        public IQueryable<User> UserSearch(string email, string name, string company, UserType[] type)
        {
            var res = _context.User.AsQueryable<User>();
            if (!String.IsNullOrEmpty(email))
            {
                res = from User in res
                      where User.Email.Contains(email)
                      select User;
            }
            if (!String.IsNullOrEmpty(name))
            {
                res = from User in res
                      where User.FullName.Contains(name)
                      select User;
            }
            if (!String.IsNullOrEmpty(company))
            {
                res = from User in res
                      where User.CompanyName.Contains(company)
                      select User;
            }
            if (type.Length > 0)
            {
                res = from User in res
                      where type.Contains(User.Type)
                      select User;
            }
            res = from User in res
                  orderby User.RegisterTime
                  select User;
            return res;
        }
    }
}