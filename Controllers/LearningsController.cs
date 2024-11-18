using System.Security.Claims;
using Backend.Models.DTOs.Learning;
using Backend.Repository.Interfaces;
using Backend.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

// Controllers/LearningsController.cs
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LearningsController : ControllerBase
{
    private readonly ILearningService _learningService;
    private readonly ILogger<LearningsController> _logger;

    public LearningsController(
        ILearningService learningService,
        ILogger<LearningsController> logger)
    {
        _learningService = learningService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<LearningResponseDto>>> GetUserLearnings()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var learnings = await _learningService.GetUserLearningsAsync(userId);
            return Ok(learnings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user learnings");
            return StatusCode(500, "An error occurred while retrieving learnings.");
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LearningResponseDto>> GetLearning(int id)
    {
        try
        {
            var learning = await _learningService.GetLearningAsync(id);
            return Ok(learning);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = $"Learning with ID {id} not found." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving learning {LearningId}", id);
            return StatusCode(500, "An error occurred while retrieving the learning.");
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LearningResponseDto>> CreateLearning([FromForm] CreateLearningDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var learning = await _learningService.CreateLearningAsync(dto, userId);

            return CreatedAtAction(
                nameof(GetLearning),
                new { id = learning.Id },
                learning);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating learning");
            return StatusCode(500, "An error occurred while creating the learning.");
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<LearningResponseDto>> UpdateLearning(int id, [FromForm] UpdateLearningDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var learning = await _learningService.UpdateLearningAsync(id, dto, userId);
            return Ok(learning);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = $"Learning with ID {id} not found." });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating learning {LearningId}", id);
            return StatusCode(500, "An error occurred while updating the learning.");
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteLearning(int id)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _learningService.DeleteLearningAsync(id, userId);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = $"Learning with ID {id} not found." });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting learning {LearningId}", id);
            return StatusCode(500, "An error occurred while deleting the learning.");
        }
    }

    [HttpGet("tags")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<string>>> GetAllTags(
        [FromServices] ITagRepository tagRepository)
    {
        try
        {
            var tags = await tagRepository.GetAllAsync();
            return Ok(tags.Select(t => t.Name));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tags");
            return StatusCode(500, "An error occurred while retrieving tags.");
        }
    }
}
