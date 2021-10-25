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

        /*
        public async Task Seed()
        {
            $(function() {
        $.ajax({
                type: "GET",
            dataType: "json",
            url: "https://api.sandbox.ebay.com/commerce/catalog/v1_beta/product_summary/search?q=drone&limit=3",
            headers:
                    {
                        "Authorization": "Bearer v^1.1#i^1#r^0#I^3#f^0#p^3#t^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",
                "Accept": "application/json",
                "Content-Type": "application/json",
                "X-EBAY-C-MARKETPLACE-ID": "EBAY_US"
            }
                }).done(function(result) {
                    var tbody = $('tbody');
                    var template = $('#template').html();
                    tbody.html('');
            $.each(result, function(key, value) {
                        console.log(key + ": " + value);
                    });
                });
            });
        }
        */
    }
}
