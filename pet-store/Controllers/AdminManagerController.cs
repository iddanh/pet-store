using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using pet_store.Data;
using pet_store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace pet_store.Controllers
{
    public class AdminManagerController : Controller
    {

        private readonly PetStoreDBContext _context;
        string _fb_api_token;


        public AdminManagerController(PetStoreDBContext context, IConfiguration configuration)
        {
            _context = context;
            _fb_api_token = configuration.GetValue<string>("FacebookApiToken");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }



        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostOnFacbook([Bind("Message")] PostMessage postMessage)
        {
            string pageId = "102326535608952";
            string facebookApiUrl = "https://graph.facebook.com/";

            string postReqUrl = facebookApiUrl + pageId + "/feed?message=" + postMessage.Message + "&access_token=" + _fb_api_token;
            HttpClient client = new HttpClient();

            var response = await client.PostAsync(postReqUrl, null);


            return RedirectToAction(nameof(Index));
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
                            join user in _context.User
                            on order.User equals user.Id
                            select new
                            {
                                user,
                                order
                            } into user2order
                            group user2order by user2order.order.User into g
                            select new
                            {
                                UserName = g.Key.ToString(),
                                OrderCount = g.Count()
                            };
            Dictionary<string, int> res = new Dictionary<string, int>();
            try
            {
                foreach (var item in query_res)
                {
                    res.Add(item.UserName, item.OrderCount);
                }
            }
            catch { }
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
