using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Extensions;

namespace GlobomanticsWeb.Controllers
{
    public class BrandsController : Controller
    {
        Models.GloboDBContext dbContext;

        public BrandsController(Models.GloboDBContext ctx)
        {
            dbContext = ctx;
        }

        public IActionResult Index()
        {
            var brands = from b in dbContext.Brands
                         select b;

            return View(brands);
        }

        [HttpPost]
        public async Task<IActionResult> Index (Models.BrandModel model)
        {
            if (ModelState.IsValid)
            {
                //save new brand to the database
                dbContext.Brands.Add(model);
                await dbContext.SaveChangesAsync();

                //redirect to the main view
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
    }
}