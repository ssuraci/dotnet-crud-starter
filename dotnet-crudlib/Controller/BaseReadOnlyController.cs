using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NetCrudLib.Dto;
using NetCrudLib.Model;
using NetCrudLib.Service;
using WebapiTemplate.Dto;

namespace NetCrudLib.Controller;

//    [ApiController]

// [Authorize(Roles = "WebapiTemplate")]
public abstract class BaseReadOnlyController<T, TD, TK> : ControllerBase where T : BaseEntity<TK> where TD : BaseDto<TK>
{
    protected readonly ILogger Logger;

    protected readonly IMapper Mapper;

    protected abstract BaseService<T, TK> GetBaseService();


    public BaseReadOnlyController(ILogger<T> logger, IMapper mapper)
    {
        this.Logger = logger;
        this.Mapper = mapper;
    }

    protected virtual void BeforeGet(PageModel pageModel)
    {
    }

    protected virtual void BeforeGet(TK id, PageModel pageModel)
    {
    }

    // GET api/values/5
    [HttpGet("{id}")]
    public  virtual async Task<IActionResult> Get(TK id, [FromQuery] PageModel pageModel)
    {
        BeforeGet(id, pageModel);
        var res = await GetBaseService().GetById(id, pageModel);
        return Ok(Mapper.Map<TD>(res));
    }


    [HttpGet]
    public virtual async Task<IActionResult> Get([FromQuery] PageModel pageModel)
    {
        int count = -1;
        BeforeGet(pageModel);
        IList<TD> list = Mapper.Map<IList<TD>>((await GetBaseService().GetByPageModel(pageModel)).Items);

        Request.HttpContext.Response.Headers.Append("Access-Control-Expose-Headers", "X-Total-Count");
        Request.HttpContext.Response.Headers.Append("X-Total-Count", Convert.ToString(count));
        return Ok(list);
    }

    [HttpGet("dropdown")]
    public virtual IList<DropdownDto> GetDropdown([FromQuery] PageModel pageModel)
    {
        int count = 0; // non calcola count(*)    
        BeforeGet(pageModel);
        IList<DropdownDto> list = Mapper.Map<IList<DropdownDto>>(GetBaseService().GetByPageModel(pageModel));
        return list;
    }
}