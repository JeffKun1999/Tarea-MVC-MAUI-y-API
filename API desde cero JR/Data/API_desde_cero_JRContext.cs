using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using API_desde_cero_JR.Data.Models;

namespace API_desde_cero_JR.Data
{
    public class API_desde_cero_JRContext : DbContext
    {
        public API_desde_cero_JRContext (DbContextOptions<API_desde_cero_JRContext> options)
            : base(options)
        {
        }

        public DbSet<API_desde_cero_JR.Data.Models.Burger> Burger { get; set; } = default!;
        public DbSet<API_desde_cero_JR.Data.Models.Promo> Promo { get; set; } = default!;
    }
}
