using pet_store.Data;
using pet_store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;


namespace pet_store.Services
{
    public class SeedService
    {
        private const int QUANTITY = 25;
        private readonly PetStoreDBContext _context;
        string _seed_api_token;

        public SeedService(PetStoreDBContext context, string seed_api_token="")
        {
            _context = context;
            //_seed_api_token = seed_api_token;
        }


        public async Task GooProSearch()
        {
            var baseURL = "https://serpapi.com/search.json?engine=google&tbm=shop&q=";
            var apiKey = "&api_key=a6b0302209798e2eff9f705e466950ffc61e078cbec5b88e842b2e3839b0655d";
            foreach (var category in _context.Category) try
                {
                    if (category.Name.Equals("Food") || category.Name.Equals("Toys"))
                    {
                        continue;
                    }
                    JsonElement jsonTemp;
                    var count = 0;
                    var urlEncodedQ = UrlEncoder.Default.Encode(category.Name);
                    if (category.Name.Contains("Accessories"))
                    {
                        urlEncodedQ = UrlEncoder.Default.Encode("Pet Accessories");
                    }
                    var client = new HttpClient();
                    var targetURL = baseURL + urlEncodedQ + apiKey;
                    var response = await HttpClientJsonExtensions.GetFromJsonAsync<JsonDocument>(client, targetURL);
                    var data = response.RootElement.GetProperty("shopping_results");
                    IEnumerable<Product> products = new List<Product>();
                    foreach (var prop in data.EnumerateArray())
                    {
                        string name = "";
                        string description = "";
                        string image = "";
                        double price = 0;
                        if (prop.TryGetProperty("title", out jsonTemp))
                        {
                            name = jsonTemp.GetString();
                        }

                        if (!prop.TryGetProperty("extracted_price", out jsonTemp))
                        {
                            if (prop.TryGetProperty("price", out jsonTemp))
                            {
                                price = Double.Parse(jsonTemp.GetString().Trim('$'));
                                try
                                {
                                    if (!Double.TryParse(price.ToString().Substring(0, 5), out price)) { }
                                }
                                catch (Exception e) { }
                            }
                        }
                        else
                        {
                            price = jsonTemp.GetSingle();
                            try
                            {
                                if (!Double.TryParse(price.ToString().Substring(0, 5), out price)) { }
                            }
                            catch (Exception e) { }
                        }

                        if (prop.TryGetProperty("snippet", out jsonTemp))
                        {
                            description = jsonTemp.GetString();
                        }
                        if (prop.TryGetProperty("extensions", out jsonTemp))
                        {
                            description = new StringBuilder().AppendJoin(", ", jsonTemp.EnumerateArray().Select(x => x.GetString())).Append(", " + description).ToString().Trim(',', ' ');
                        }

                        if (prop.TryGetProperty("thumbnail", out jsonTemp))
                        {
                            image = jsonTemp.GetString();
                        }

                        products = products.Append(new Product
                        {
                            Category = category,
                            Description = description,
                            Image = image,
                            Name = name,
                            Price = price,
                        });
                        count++;
                        if (count == QUANTITY) break;
                        Console.WriteLine(description);
                    }
                    _context.AddRange(products.ToList());
                }
                catch (Exception e) { Console.WriteLine(e); }
            await _context.SaveChangesAsync();
        }
    }
}