using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Modal_Upload_ASP.Net_Core_5._0.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Modal_Upload_ASP.Net_Core_5._0.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<BlogAuthor> BlogAuthors { get; set; }

    }
}
