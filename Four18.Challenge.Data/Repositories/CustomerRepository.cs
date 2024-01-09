using Four18.Challenge.Data.Context;
using Four18.Challenge.Data.Entities;
using Four18.Challenge.Data.Interfaces;
using Four18.Common.EntityFramework;

namespace Four18.Challenge.Data.Repositories;

public class CustomerRepository : Repository<Customer, int, CustomerDbContext>, ICustomerRepository {
    private const string SprocActions = "[dbo].[sproc_actions_retrieve_by_subjectid] @SubjectId={0}";
    private const string SprocActionIds = "dbo.sproc_actionids_retrieve_by_subjectid @SubjectId={0}";

    public CustomerRepository(CustomerDbContext dbContext) : base(dbContext) {
        DbSet = dbContext.Customers;
    }
}