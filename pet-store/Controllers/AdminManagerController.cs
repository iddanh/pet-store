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

        public IActionResult Index()
        {
            return View();
        }

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
        public JsonResult GetUsersStatistics()
        {
            var query_res = from order in _context.Product
                            group order by order.CategoryId into g
                            select new
                            {
                                Category = g.Key,
                                Size = g.Count()
                            };



            //from user in _context.User

            List<Models.User> users = _context.User.ToList();
            Dictionary<string, object> stats = new Dictionary<string, object>();
            stats.Add("usersCount", users.Count);
            stats.Add("usersCount2", users.Count);


            return Json(stats);

        }
    }
}
