using GameStore.Api.DTOs.Common;
using GameStore.Api.DTOs.Games;
using GameStore.Api.Exceptions;
using GameStore.Api.Mappings;
using GameStore.Data.Constants;
using GameStore.Data.Contexts;
using GameStore.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Controllers;
[ApiController]
[Route("api/games")]
public class GamesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public GamesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(PagedResultDto<GameResponseDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<
        PagedResultDto<GameResponseDto>>> GetAll(
        [FromQuery] GameQueryParametersDto query)
    {
        var gamesQuery =
            _context.Games
                .AsNoTracking()
                .Include(g => g.Genre)
                .AsQueryable();
        // Filter by search term if provided
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            gamesQuery =
                gamesQuery.Where(g =>
                    g.Title.Contains(query.Search));
        }
        // Filter by genre if GenreId is provided
        if (query.GenreId.HasValue)
        {
            gamesQuery =
                gamesQuery.Where(g =>
                    g.GenreId == query.GenreId.Value);
        }
        
        var totalCount =
            await gamesQuery.CountAsync();

        var games =
            await gamesQuery
                .OrderBy(g => g.Title)
                .Skip((query.Page - 1)
                    * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

        var response =
            new PagedResultDto<GameResponseDto>
            {
                Items = games
                    .Select(g => g.ToDto())
                    .ToList(),

                Page = query.Page,

                PageSize = query.PageSize,

                TotalCount = totalCount,

                TotalPages =
                    (int)Math.Ceiling(
                        totalCount /
                        (double)query.PageSize)
            };

        return Ok(response);
    }
    [HttpGet("{id:guid}")]
    [ProducesResponseType(
        typeof(GameResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GameResponseDto>>
        GetById(Guid id)
    {
        var game =
            await _context.Games
                .AsNoTracking()
                .Include(g => g.Genre)
                .FirstOrDefaultAsync(g =>
                    g.Id == id);

        if (game is null)
        {
            throw new NotFoundException(
                "Game not found.");
        }

        return Ok(game.ToDto());
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPost]
    [ProducesResponseType(
        typeof(GameResponseDto),
        StatusCodes.Status201Created)]
    public async Task<ActionResult<GameResponseDto>>
        Create(CreateGameRequestDto request)
    {
        var genre =
            await _context.Genres
                .FirstOrDefaultAsync(g =>
                    g.Id == request.GenreId);

        if (genre is null)
        {
            throw new NotFoundException(
                "Genre not found.");
        }

        var game = new Game
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Price = request.Price,
            ReleaseDateUtc = request.ReleaseDateUtc,
            GenreId = request.GenreId,
            CreatedAtUtc = DateTime.UtcNow,
            CreatedBy = User.Identity?.Name
            
        };
        
        _context.Games.Add(game);

        await _context.SaveChangesAsync();

        game.Genre = genre;

        var response = game.ToDto();

        return CreatedAtAction(
            nameof(GetById),
            new { id = game.Id },
            response);
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(
        StatusCodes.Status204NoContent)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateGameRequestDto request)
    {
        var game =
            await _context.Games
                .FirstOrDefaultAsync(g =>
                    g.Id == id);

        if (game is null)
        {
            throw new NotFoundException(
                "Game not found.");
        }

        var genreExists =
            await _context.Genres
                .AnyAsync(g =>
                    g.Id == request.GenreId);

        if (!genreExists)
        {
            throw new NotFoundException(
                "Genre not found.");
        }

        game.Title = request.Title;
        game.Description = request.Description;
        game.Price = request.Price;
        game.ReleaseDateUtc = request.ReleaseDateUtc;
        game.GenreId = request.GenreId;
        game.UpdatedAtUtc = DateTime.UtcNow;
        game.UpdatedBy = User.Identity?.Name;

        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    [Authorize(Roles = Roles.Admin)]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(
        StatusCodes.Status204NoContent)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var game =
            await _context.Games
                .FirstOrDefaultAsync(g =>
                    g.Id == id);

        if (game is null)
        {
            throw new NotFoundException(
                "Game not found.");
        }

        _context.Games.Remove(game);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}
