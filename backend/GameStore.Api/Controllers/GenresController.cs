using GameStore.Api.DTOs.Genres;
using GameStore.Data.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Controllers;

[ApiController]
[Route("api/genres")]
public sealed class GenresController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public GenresController(
        ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(List<GenreResponseDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<
        List<GenreResponseDto>>> GetAll()
    {
        var genres =
            await _context.Genres
                .AsNoTracking()
                .OrderBy(g => g.Name)
                .Select(g =>
                    new GenreResponseDto
                    {
                        Id = g.Id,
                        Name = g.Name
                    })
                .ToListAsync();

        return Ok(genres);
    }
}