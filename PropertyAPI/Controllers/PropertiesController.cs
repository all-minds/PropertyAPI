using PropertyAPI.Enums;
using PropertyAPI.Models;
using PropertyAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PropertyAPI.Services;

namespace PropertyAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PropertiesController : ControllerBase
{
    private readonly ILogger<PropertiesController> _logger;
    private readonly PropertyRepository _propertyRepository = new (Collection.properties);
    public TokenService _tokenService = new TokenService();

    public PropertiesController(ILogger<PropertiesController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<Property>>> GetAllPropertiesAsync()
    {
        var idToken = this.HttpContext.Request.Headers["Authorization"];
        return Ok(await _propertyRepository.GetAllAsync(idToken));
    }

    [HttpGet]
    [Authorize]
    [Route("{id}")]
    public async Task<ActionResult<Property>> GetPropertyAsync(string id)
    {        
        var property = new Property()
        {
            Id = id
        };

        return Ok(await _propertyRepository.GetAsync(property));
    }

    [HttpPut]
    [Authorize]
    [Route("{id}")]
    public async Task<ActionResult<Property>> UpdatePropertyAsync(string id, Property property)
    {
        if (id != property.Id)
        {
            return BadRequest("O Id da Propriedade não corresponde com o objeto a ser atualizado.");
        }

        return Ok(await _propertyRepository.UpdateAsync(property));
    }

    [HttpDelete]
    [Authorize]
    [Route("{id}")]
    public async Task<ActionResult> DeletePropertyAsync(string id, Property property)
    {
        if (id != property.Id)
        {
            return BadRequest("O Id da Propriedade não corresponde com o objeto a ser removido.");
        }

        await _propertyRepository.DeleteAsync(property);

        return Ok();
    }
    
    [HttpPost]    
    [Authorize]
    public async Task<ActionResult<Property>> AddPropertyAsync(Property property)
    {
        var idToken = this.HttpContext.Request.Headers["Authorization"];
        return Ok(await _propertyRepository.AddAsync(property, idToken));
    }
}