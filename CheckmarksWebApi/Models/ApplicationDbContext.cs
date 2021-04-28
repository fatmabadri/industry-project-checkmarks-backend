using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CheckmarksWebApi.Models
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<NICEClass> NICEClasses { get; set; }
        public DbSet<NICETerm> NICETerms { get; set; }

        public DbSet<Trademark> Trademarks { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
}
