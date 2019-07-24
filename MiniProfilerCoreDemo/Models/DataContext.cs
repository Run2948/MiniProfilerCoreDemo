using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MiniProfilerCoreDemo.Models.Entities;

namespace MiniProfilerCoreDemo.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options)
        : base(options)
        { }

        public DbSet<User> Users { get; set; }
    }
}
