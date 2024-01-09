using Four18.Challenge.Data.Entities;

namespace Four18.Challenge.Business.Dtos.Mapper;

public static class CustomerMap {
    public static CustomerDto ToDto(this Customer? source) {
        return source == null
            ? new CustomerDto()
            : new CustomerDto {
                CustomerId = source.Id,
                FirstName = source.First,
                LastName = source.Last,
                EmailAddress = source.Email ?? string.Empty,
                CreatedAt = source.CreatedAt,
                ModifiedAt = source.ModifiedAt,
            };
    }

    public static Customer ToEntity(this CustomerDto? source) {
        return source == null
            ? new Customer()
            : new Customer {
                Id = source.CustomerId,
                First = source.FirstName,
                Last = source.LastName,
                Email = source.EmailAddress,
                CreatedAt = source.CreatedAt,
                ModifiedAt = source.ModifiedAt,
            };
    }
}
