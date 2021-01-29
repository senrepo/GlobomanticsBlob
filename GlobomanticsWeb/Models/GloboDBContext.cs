using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobomanticsWeb.Models
{
    public class GloboDBContext : DbContext
    {
        public GloboDBContext(DbContextOptions options):base(options)
        {

        }
        public virtual DbSet<BrandModel> Brands { get; set; }

    }
}
