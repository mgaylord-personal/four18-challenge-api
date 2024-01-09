using Four18.Challenge.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Four18.Challenge.Data.Context;

public class CustomerDbContext : DbContext {

    public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options) {
        //initialize base only
    }

    public DbSet<Customer> Customers { get; set; }
}

//public class DataContext : DbContext {
//    protected readonly IConfiguration Configuration;

//    public DataContext(DbContextOptions<DataContext> options) : base(options) {
//        //Configuration = configuration;
//    }

//    //protected override void OnConfiguring(DbContextOptionsBuilder options) {
//    //    // in memory database used for simplicity, change to a real db for production applications
//    //    options.UseInMemoryDatabase("MemoryDb");
//    //}

//    public DbSet<Customer> Customers { get; set; }
//}