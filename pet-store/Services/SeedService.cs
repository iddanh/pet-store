using Newtonsoft.Json;
using pet_store.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace pet_store.Services
{
    public class SeedService
    {
        private readonly PetStoreDBContext _context;

        public SeedService(PetStoreDBContext context)
        {
            _context = context;
        }

        public async Task Seed()
        {

        }
        public async Task Weather()
        {
            dynamic json = JsonConvert.DeserializeObject(@"https://www.7timer.info/bin/api.pl?lon=34.8516&lat=31.0461&product=civillight&output=json");
            //XElement;
            
        }
    }
}
