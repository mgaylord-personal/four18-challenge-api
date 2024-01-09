using Four18.Common.Result;
using Four18.Challenge.Business.Dtos;

namespace Four18.Challenge.Business.Interfaces;

public interface ICustomerService {
    Task<IResult> GetAllAsync();
    Task<IResult> GetByIdAsync(int id);
    Task<IResult> AddAsync(CustomerDto dto);
    Task<IResult> DeleteByIdAsync(int id);
    Task<IResult> UpdateAsync(CustomerDto dto);
}
