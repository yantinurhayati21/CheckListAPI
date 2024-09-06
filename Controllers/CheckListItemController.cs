using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckListAPI.Data;
using CheckListAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CheckListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CheckListItemController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CheckListItemController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/CheckListItem
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CheckListItem>>> GetAllCheckListItems(
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10,
            [FromQuery] int checkListId = 0)
        {
            var query = _context.CheckListItems.AsQueryable();

            if (checkListId > 0)
            {
                query = query.Where(ci => ci.CheckListId == checkListId);
            }

            var total = await query.CountAsync();

            var checkListItems = await query
                .Include(ci => ci.CheckList)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            var metadata = new
            {
                TotalItems = total,
                Limit = limit,
                Page = page,
                TotalPages = (int)Math.Ceiling((double)total / limit),
                NextPage = page * limit < total ? (int?)page + 1 : null,
                PrevPage = page > 1 ? (int?)page - 1 : null
            };

            return Ok(new { Metadata = metadata, CheckListItems = checkListItems });
        }

        // GET: api/CheckListItem/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<CheckListItem>> GetCheckListItemById(int id)
        {
            var checkListItem = await _context.CheckListItems
                .Include(ci => ci.CheckList)
                .SingleOrDefaultAsync(ci => ci.Id == id);

            if (checkListItem == null)
            {
                return NotFound(new { Message = "CheckListItem not found." });
            }

            return Ok(checkListItem);
        }

        // POST: api/CheckListItem
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateCheckListItem(CheckListItem checkListItem)
        {
            // Set CreatedAt dan UpdatedAt secara otomatis
            checkListItem.CreatedAt = DateTime.UtcNow;
            checkListItem.UpdatedAt = DateTime.UtcNow;

            // Pastikan CheckListId yang disediakan valid
            var checkList = await _context.CheckLists.FindAsync(checkListItem.CheckListId);
            if (checkList == null)
            {
                return BadRequest(new { Message = "Invalid CheckListId provided." });
            }

            // Validasi Status (jika enum digunakan)
            if (!Enum.IsDefined(typeof(CheckListItemStatus), checkListItem.Status))
            {
                return BadRequest(new { Message = "Invalid Status provided." });
            }

            // Tambahkan CheckListItem ke database
            _context.CheckListItems.Add(checkListItem);
            await _context.SaveChangesAsync();

            // Mengembalikan respons dengan status sukses dan data yang ditambahkan
            return CreatedAtAction(
                nameof(GetCheckListItemById),
                new { id = checkListItem.Id },
                new
                {
                    Message = "CheckListItem added successfully.",
                    Data = checkListItem
                }
            );
        }


        // DELETE: api/CheckListItem/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCheckListItem(int id)
        {
            var checkListItem = await _context.CheckListItems.FindAsync(id);
            if (checkListItem == null)
            {
                return NotFound(new { Message = "CheckListItem not found." });
            }

            _context.CheckListItems.Remove(checkListItem);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "CheckListItem deleted successfully." });
        }

        [HttpPut("update-status/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCheckListItemStatus(int id, [FromBody] CheckListItemStatus newStatus)
        {
            var checkListItem = await _context.CheckListItems.FindAsync(id);
            if (checkListItem == null)
            {
                return NotFound(new { Message = "CheckListItem not found." });
            }

            // Validasi Status baru (jika menggunakan enum)
            if (!Enum.IsDefined(typeof(CheckListItemStatus), newStatus))
            {
                return BadRequest(new { Message = "Invalid Status provided." });
            }

            // Perbarui status dan waktu update
            checkListItem.Status = newStatus;
            checkListItem.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { Message = "CheckListItem status updated successfully.", Data = checkListItem });
        }

        [HttpPut("rename-item/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RenameCheckListItem(int id, [FromBody] string newName)
        {
            var checkListItem = await _context.CheckListItems.FindAsync(id);
            if (checkListItem == null)
            {
                return NotFound(new { Message = "CheckListItem not found." });
            }

            if (string.IsNullOrWhiteSpace(newName))
            {
                return BadRequest(new { Message = "Invalid name provided." });
            }

            checkListItem.CheckListItemName = newName;
            checkListItem.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { Message = "CheckListItem name updated successfully.", Data = checkListItem });
        }

    }
}
