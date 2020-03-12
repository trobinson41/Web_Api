using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrgDAL
{
    public class OrganizationDbContext : IdentityDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //optionsBuilder.UseSqlServer(@"data source=.\SQLEXPRESS; initial catalog=CodeFirstDemo; integrated security=SSPI" providerName = "System.Data.SqlClient");
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS; Database=OrgAPIDb; Trusted_Connection=True");
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
