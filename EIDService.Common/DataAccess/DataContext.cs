using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.DataAccess
{
    public class DataContext : DbContext
    {
        public DataContext()
            : base("DefaultConnection")
        {
            //this.Database.Delete();
            //Configuration.ProxyCreationEnabled = false;
            //Configuration.LazyLoadingEnabled = true;

            this.Configuration.AutoDetectChangesEnabled = true;

            Database.SetInitializer(new TTInitializer());
        }



        public DbSet<Emitent> Issues { get; set; }

        public DbSet<IssueStatus> Statuses { get; set; }
    }
}
