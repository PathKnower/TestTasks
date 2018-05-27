using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask2
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() : base("DbConnection") { }

        public DbSet<FileModel> Files { get; set; }
    }
}
