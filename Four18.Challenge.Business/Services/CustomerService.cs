using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Four18.Common.Logging;
using Four18.Common.Result;
using Four18.Challenge.Business.Dtos;
using Four18.Challenge.Business.Dtos.Mapper;
using Four18.Challenge.Business.Interfaces;
using Four18.Challenge.Data.Entities;
using Four18.Challenge.Data.Interfaces;

namespace Four18.Challenge.Business.Services;

public class CustomerService : ICustomerService {
    private readonly ILogger _logger;
    private readonly ICustomerRepository _repository;

    public CustomerService(ILogger<CustomerService> logger, ICustomerRepository repository) {
        _logger = logger;
        _repository = repository;
    }

    public async Task<IResult> GetAllAsync() {
        try {
            _logger.LogInformation(LoggingHelper.GetLogClassMethodWithParams(),
                nameof(CustomerService), nameof(GetAllAsync));

            var entities = await GetAllEntities().ToListAsync();

            return new ResultOk<IEnumerable<CustomerDto>>(entities.Select(x => x.ToDto()));
        }
        catch (Exception ex) {
            return new ResultException(ex);
        }
    }

    public async Task<IResult> GetByIdAsync(int id) {
        try {
            _logger.LogInformation(LoggingHelper.GetLogClassMethodWithParams(nameof(id)),
                nameof(CustomerService), nameof(GetByIdAsync), id);

            var entity = await GetAllEntities().FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return new ResultNotFound();

            return new ResultOk<CustomerDto>(entity.ToDto());
        }
        catch (Exception e) {
            return new ResultException(e);
        }
    }


    public async Task<IResult> AddAsync(CustomerDto dto) {
        try {
            _logger.LogInformation(LoggingHelper.GetLogClassMethodWithParams(nameof(dto)),
                nameof(CustomerService), nameof(AddAsync), LoggingHelper.JsonSerializeObject(dto));

            var entity = dto.ToEntity();

            // TOOO: add fluent valiation
            //
            //var validate = await new CustomerValidator().ValidateAsync(entity,
            //    o => o.IncludeRuleSets(ValidatorConstants.RuleSetAddDefaultParams));
            //if (!validate.IsValid) {
            //    return ValidationResolver.GetResult(validate.ToIValidationResult());
            //}

            var newEntity = await _repository.AddAsync(entity);
            if (newEntity?.Id == 0) // this shouldn't happen
                return new ResultUnknown<CustomerDto>();

            return new ResultCreated<CustomerDto>(newEntity.ToDto());
        }
        catch (Exception ex) {
            return new ResultException(ex);
        }
    }

    public async Task<IResult> DeleteByIdAsync(int id) {
        try {
            _logger.LogInformation(LoggingHelper.GetLogClassMethodWithParams(nameof(id)),
                nameof(CustomerService), nameof(DeleteByIdAsync), id);

            var entity = await GetAllEntities().FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return new ResultNotFound();

            // TODO: change to soft delete with an "active / inactive" bool property and change RemoveAsync to UpdateAsync
            await _repository.RemoveAsync( id);
            return new ResultOk();
        }
        catch (Exception ex) {
            return new ResultException(ex);
        }
    }

    public async Task<IResult> UpdateAsync(CustomerDto dto) {
        try {
            _logger.LogInformation(LoggingHelper.GetLogClassMethodWithParams(nameof(dto)),
                nameof(CustomerService), nameof(UpdateAsync), LoggingHelper.JsonSerializeObject(dto));

            var existingEntity = await _repository.GetByIdAsync(dto.CustomerId);

            // TOOO: add fluent valiation
            //
            //var validate = await new CustomerValidator().ValidateAsync(existingEntity,
            //    o => o.IncludeRuleSets(ValidatorConstants.RuleSetUpdateDefaultParams));
            //if (!validate.IsValid) {
            //    return ValidationResolver.GetResult(validate.ToIValidationResult());
            //}

            var entity = dto.ToEntity();

            var updatedEntity = await _repository.UpdateAsync(entity, entity.Id);
            if (updatedEntity == null) return new ResultUnknown<CustomerDto>();

            return new ResultOk<CustomerDto>(updatedEntity.ToDto());
        }
        catch (Exception ex) {
            return new ResultException(ex);
        }
    }

    #region helpers

    /// <summary>
    ///     Non-Deleted Customers
    ///     Assumption - We are allowing non-Active customers to be modified
    ///     Assumption - A deleted customers should not be found
    /// </summary>
    /// <returns></returns>
    private IQueryable<Customer> GetAllEntities() {
        return _repository.GetAll().OrderByDescending(x => x.Id);
    }

    #endregion
}
