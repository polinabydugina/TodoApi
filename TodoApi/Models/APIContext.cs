using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
namespace RestApi.Data
{
    public class APIContext : DbContext
    {
        public DbSet<MyTask> Tasks { get; set; }
        public APIContext(DbContextOptions<APIContext> options) : base(options)
        {

        }
    }
}
