using System.Security;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using NetCrudStarter.Model;
using NetCrudStarter.Dto;
using FluentValidation;
using FluentValidation.Results;
using System.Security.Claims;
using DotNetCore.CAP;
using NetCrudStarter.Filter;
using Newtonsoft.Json.Linq;

namespace NetCrudStarter.Controller;

//    [ApiController]

// [Authorize(Roles = "NetCrudStarter")]
public abstract class BaseController<T, TD, TV, TK, TE> : BaseReadOnlyController<T, TD, TK, TE>
    where T : BaseEntity<TK> 
    where TD : BaseDto<TK> 
    where TV : AbstractValidator<TD>
    where TE: Enum
{
    protected readonly TV validator;
    protected readonly ICapPublisher capBus;
    public BaseController(ILogger<T> logger, IMapper mapper, TV validator) : base(logger, mapper)
    {
        this.validator = validator;
        this.capBus = capBus;
    }

    
    private String getValidationErrorMessage(ValidationResult results)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        foreach (var failure in results.Errors)
        {
            sb.AppendLine(failure.ErrorMessage);
        }

        return sb.ToString();
    }

    private ValidationResult validate(TD dto)
    {
        return validator.Validate(dto);
    }

    // POST api/values
    [ValidateModel]
    [HttpPost]
    public virtual async Task<ActionResult<TD>> Post([FromBody] TD dto, [FromServices] ICapPublisher capBus)
    {
        T entity = Mapper.Map<T>(dto);
        entity = await GetBaseService().Add(entity);
        var res = Mapper.Map<TD>(entity);
        capBus.PublishAsync("entity.created." + typeof(T).Name, res);
        return Ok(res);
    }

    // PUT api/values/5
    [ValidateModel]
    [HttpPut("{id}")]
    public virtual async Task<ActionResult<TD>> Put(TK id, [FromBody] TD dto, [FromServices] ICapPublisher capBus)
    {
        if (id == null || !id.Equals(dto.Id))
        {
            throw new ValidationException("Id mismatch tra oggetto e url");
        }

        T entity = Mapper.Map<T>(dto);
        entity = await GetBaseService().Update(entity);
        var res = Mapper.Map<TD>(entity);
        capBus.PublishAsync("entity.modified." + typeof(T).Name, res);
        return Ok(res);
    }

    [HttpDelete("{id}")]
    public virtual async Task<ActionResult> Delete(TK id, [FromServices] ICapPublisher capBus)
    {
        await GetBaseService().RemoveById(id);
        capBus.PublishAsync("entity.deleted." + typeof(T).Name, id);
        return Ok();
    }
    
}