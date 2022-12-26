using Microsoft.EntityFrameworkCore;

namespace RequestTracker.Models.DBModels
{
    public class EF_dataContext : DbContext
    {
        public EF_dataContext(DbContextOptions<EF_dataContext> options) : base(options) { }

        public DbSet<RequestModel> Requests { get; set; }
        public DbSet<EmployeeModel> Employees { get; set; }
        public DbSet<DepartmentModel> Departments { get; set; }
        public DbSet<RoleModel> Roles { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<ManagerModel> Managers { get; set; }
        public DbSet<StatusModel> Status { get; set; }


    }

}
