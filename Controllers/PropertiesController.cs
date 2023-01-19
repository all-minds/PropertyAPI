using PropertyAPI.Enums;
using PropertyAPI.Models;
using PropertyAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace PropertyAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PropertiesController : ControllerBase
{
    private readonly ILogger<PropertiesController> _logger;
    private readonly PropertyRepository _propertyRepository = new (Collection.properties);

    public PropertiesController(ILogger<PropertiesController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<Property>>> GetAllPropertiesAsync()
    {
        return Ok(await _propertyRepository.GetAllAsync());
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

    [HttpPost]
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
    
    [HttpPut]    
    [Authorize]
    public async Task<ActionResult<Property>> AddPropertyAsync(Property property)
    {
        return Ok(await _propertyRepository.AddAsync(property));
    }
}