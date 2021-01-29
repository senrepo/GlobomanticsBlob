using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using GlobomanticsWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Caching.Memory;

namespace GlobomanticsWeb.Controllers
{
    public class LocationsController : Controller
    {
        private readonly static string COUNTRIES_KEY = "Globocations";

        private IMemoryCache countryCache;
        private CosmosClient docClient;
        private Microsoft.Azure.Cosmos.Container docContainer;

        public LocationsController(IMemoryCache cache, CosmosClient client)
        {
            countryCache = cache;
            docClient = client;
            docContainer = docClient.GetContainer(
                "m3globocdb", "globolocations");
            
        }

        public async Task<IActionResult> Index()
        {
            //get countries for search list
            var countries = await GetCountriesListAsync();
            ViewData["CountriesList"] = countries;

            //get the list of all locations from CosmosDb
            var locations = new List<LocationModel>();

            var iterator = docContainer.GetItemQueryIterator<LocationModel>();
            while (iterator.HasMoreResults)
            {
                var pageOfLocations = await iterator.ReadNextAsync();
                locations.AddRange(pageOfLocations.ToList());
            }

            //pass locations
            return View(locations);
        }

        private async Task<List<string>> GetCountriesListAsync()
        {
            var countries = await countryCache.GetOrCreateAsync<List<string>>(
                COUNTRIES_KEY, async(entry) => {
                    //TODO: Get countries from CosmosDB
                    var innerCountries = new List<string>();

                    var iterator = docContainer.GetItemQueryIterator<string>(
                        "SELECT DISTINCT VALUE c.country FROM globolocations c");
                    
                    while(iterator.HasMoreResults)
                    {
                        var pagedCountries = await iterator.ReadNextAsync();
                        innerCountries.AddRange(pagedCountries);
                    }

                    return innerCountries;
                });

            return countries;
           
        }

        [HttpPost]
        public async Task<IActionResult> Add(LocationModel model)
        {
            if (ModelState.IsValid)
            {
                //Add a new item to CosmosDB
                model.Id = Guid.NewGuid().ToString();
                await docContainer.CreateItemAsync(
                    model, new PartitionKey(model.Country));

                return RedirectToAction("index");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string country)
        {
            //get all locations for a country          
            var locations = new List<LocationModel>();

            var iterator = docContainer.GetItemQueryIterator<LocationModel>(
                requestOptions: new QueryRequestOptions {
                    PartitionKey=
                        new PartitionKey(country)}
                );
            while (iterator.HasMoreResults)
            {
                var pageOfLocations = await iterator.ReadNextAsync();
                locations.AddRange(pageOfLocations.ToList());
            }

            //pass locations
            return View(locations);
        }

    }
}