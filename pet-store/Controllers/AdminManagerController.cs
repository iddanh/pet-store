using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pet_store.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pet_store.Controllers
{
    public class AdminManagerController : Controller
    {

        private readonly PetStoreDBContext _context;

        public AdminManagerController(PetStoreDBContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public JsonResult GetProductsCategorySizes()
        {

            var query_res = from product in _context.Product
                            group product by product.CategoryId into g
                            select new
                            {
                                Category = (from category in _context.Category
                                            where category.Id == g.Key
                                            select category.Name).FirstOrDefault(),
                                Size = g.Count()
                            };

            Dictionary<string, int> res = new Dictionary<string, int>();
            foreach (var item in query_res)
            {
                res.Add(item.Category, item.Size);
            }
            return Json(res);

        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public JsonResult GetUsersOrderStatistics()
        {
            var query_res = from order in _context.Order
                            group order by order.User into g
                            select new
                            {
                                UserName = (from user in _context.User
                                           where user.Id == g.Key
                                           select user.FullName).FirstOrDefault(),
                                OrderCount = g.Count()
                            };
            Dictionary<string, int> res = new Dictionary<string, int>();
            foreach (var item in query_res)
            {
                res.Add(item.UserName, item.OrderCount);
            }

            return Json(res);

        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public JsonResult GetLastestRegisteredUsersInfo()
        {
            var query_res = (from user in _context.User                           
                             orderby user.RegisterTime descending
                            select new
                            {
                                UserName = user.FullName ?? user.Id.ToString(),
                                RegisterTime = user.RegisterTime.ToString()
                            }).Take(5);
            Dictionary<string, string> res = new Dictionary<string, string>();
            foreach (var item in query_res)
            {
                res.Add(item.UserName, item.RegisterTime);
            }

            return Json(res);

        }
    }
}
