using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Four18.Common.Logging;
using Four18.Common.Web.Controllers;
using Four18.Challenge.Business.Dtos;
using Four18.Challenge.Business.Interfaces;

namespace Four18.Challenge.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
//[Authorize]
public class CustomerController : ResultController {
    private readonly ILogger<CustomerController> _logger;
    private readonly ICustomerService _service;

    public CustomerController(ILogger<CustomerController> logger, ICustomerService service) : base(logger) {
        _logger = logger;
        _service = service;
    }

    /// <summary>
    ///     Gets all Customers
    /// </summary>
    /// <returns><see cref="Array"/> of <see cref="CustomerDto"/></returns>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<CustomerDto>))]
    //[AuthorizeAction(SecurityActions.CustomerView)]
    public async Task<IActionResult> GetAll() {
        Logger.LogInformation(LoggingHelper.GetLogClassMethodWithParams(),
            nameof(CustomerController), nameof(GetAll));
        return ResolveResult(await _service.GetAllAsync());
    }

    /// <summary>
    ///     Gets a Customer by its Identifier
    /// </summary>
    /// <param name="id">Customer identifier for <see cref="CustomerDto" /></param>
    /// <returns>
    ///     <see cref="CustomerDto" />
    /// </returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(200, Type = typeof(CustomerDto))]
    //[AuthorizeAction(SecurityActions.CustomerView)]
    public async Task<IActionResult> GetById([FromRoute] int id) {
        Logger.LogInformation(LoggingHelper.GetLogClassMethodWithParams(nameof(id)),
            nameof(CustomerController), nameof(GetById), id);
        return ResolveResult(await _service.GetByIdAsync(id));
    }

    /// <summary>
    ///     Create a new Customer.
    /// </summary>
    /// <param name="dto">
    ///     <see cref="CustomerDto" />
    /// </param>
    /// <returns>
    ///     <see cref="CustomerDto" />
    /// </returns>
    [HttpPost]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(200, Type = typeof(CustomerDto))]
    //[AuthorizeAction(SecurityActions.CustomerCreate)]
    public async Task<IActionResult> Create([FromBody] CustomerDto dto) {
        Logger.LogInformation(LoggingHelper.GetLogClassMethodWithParams(nameof(dto)),
            nameof(CustomerController), nameof(Update), LoggingHelper.JsonSerializeObject(dto));
        return ResolveResult(await _service.AddAsync(dto));
    }

    /// <summary>
    ///     Delete a Customer by its identifier.
    /// </summary>
    /// <param name="id">Customer identifier for <see cref="CustomerDto" /></param>
    /// <returns>No Content</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(404)]
    [ProducesResponseType(204)]
    //[AuthorizeAction(SecurityActions.CustomerDelete)]
    public async Task<IActionResult> DeleteById([FromRoute] int id) {
        Logger.LogInformation(LoggingHelper.GetLogClassMethodWithParams(nameof(id)),
            nameof(CustomerController), nameof(DeleteById), id);
        return ResolveResult(await _service.DeleteByIdAsync(id));
    }

    /// <summary>
    ///     Update an existing Customer by its identifier.
    /// </summary>
    /// <param name="dto">
    ///     <see cref="CustomerDto" />
    /// </param>
    /// <returns>
    ///     <see cref="CustomerDto" />
    /// </returns>
    [HttpPut]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(200, Type = typeof(CustomerDto))]
    //[AuthorizeAction(SecurityActions.CustomerModify)]
    public async Task<IActionResult> Update([FromBody] CustomerDto dto) {
        Logger.LogInformation(LoggingHelper.GetLogClassMethodWithParams(nameof(dto)),
            nameof(CustomerController), nameof(Update), LoggingHelper.JsonSerializeObject(dto));
        return ResolveResult(await _service.UpdateAsync(dto));
    }
}
