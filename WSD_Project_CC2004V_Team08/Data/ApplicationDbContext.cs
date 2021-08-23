using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WSD_Project_CC2004V_Team08.Models;

namespace WSD_Project_CC2004V_Team08.Models
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<WSD_Project_CC2004V_Team08.Models.Order> Orders { get; set; }

    }
}
