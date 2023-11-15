using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Models;
using Play.Catalog.Service.Repositories;
using static Play.Catalog.Service.Dtos.Dtos;

namespace Play.Catalog.Service.Controllers;

[ApiController]
[Route("items")]
public class ItemsController : ControllerBase
{
    private readonly IRepository<Item> _itemRepository;
    private readonly ILogger<ItemsController> _logger;
    public ItemsController(IRepository<Item> itemRepository, ILogger<ItemsController> logger)
    {
        _itemRepository = itemRepository;
        _logger = logger;

    }

    [HttpGet]
    public async Task<IEnumerable<ItemDto>> GetAsync()
    {
        _logger.LogInformation("Entrou no GetAsync");
        return (await _itemRepository.GetAllAsync())
                            .Select(item => item.AsDto());
    }

    [HttpGet("{id}")]
    [ActionName(nameof(GetByIdAsync))]
    public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
    {
        var item = await _itemRepository.GetAsync(id);
        if (item is null)
        {
            _logger.LogInformation("Item não encontrado Id={0}", id);
            return NotFound();
        }
        return item.AsDto();
    }

    [HttpPost]
    public async Task<ActionResult<ItemDto>> PostAsync(CreateItemDto createItemDto)
    {
        var item = new Item
        {
            Name = createItemDto.Name,
            Description = createItemDto.Description,
            Price = createItemDto.Price,
            CreatedAt = DateTimeOffset.UtcNow
        };
        await _itemRepository.CreateAsync(item);

        return CreatedAtAction(nameof(GetByIdAsync),
            new { id = item.Id.ToString()},
            item);
        
    }

    [HttpPut]
    public async Task<IActionResult> PutAsync(Guid id, UpdateItemDto updateItemDto)
    {
        var existingItem =await  _itemRepository.GetAsync(id);
        if (existingItem is null) return NotFound();

        existingItem.Name = updateItemDto.Name;
        existingItem.Description = updateItemDto.Description;
        existingItem.Price = updateItemDto.Price;

        await _itemRepository.UpdateAsync(existingItem);
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        var existingItem = await _itemRepository.GetAsync(id);
        if (existingItem is null) return NotFound();

        await _itemRepository.RemoveAsync(id);
        return NoContent();
    }

}
