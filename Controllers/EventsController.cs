using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly BlobService _blobService;
        public EventsController(ApplicationDBContext context, BlobService blobService) { _context = context; _blobService = blobService; }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.Events.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev == null) return NotFound();
            return Ok(ev);
        }

        [HttpPost]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> Create([FromForm] Event ev, IFormFile? BannerImage)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            ev.OrganiserId = userId;
            if (BannerImage != null)
            {
                ev.BannerUrl = await _blobService.UploadFileAsync(BannerImage);
            }
            _context.Events.Add(ev);
            await _context.SaveChangesAsync();
            return Ok(ev);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> Update(int id, Event updated)
        {
            var existing = await _context.Events.FindAsync(id);
            if (existing == null) return NotFound();

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            if (existing.OrganiserId != userId) return Forbid();

            existing.Name = updated.Name;
            existing.Description = updated.Description;
            existing.Date = updated.Date;
            existing.Location = updated.Location;
            existing.Distance = updated.Distance;
            existing.EventType = updated.EventType;

            await _context.SaveChangesAsync();
            return Ok(existing);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _context.Events.FindAsync(id);
            if (existing == null) return NotFound();

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            if (existing.OrganiserId != userId) return Forbid();

            _context.Events.Remove(existing);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Deleted" });
        }
    }
}
