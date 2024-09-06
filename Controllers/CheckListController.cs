using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CheckListAPI.Data;
using CheckListAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CheckListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CheckListController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CheckListController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/CheckList
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCheckList(
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10,
            [FromQuery] string? filter = "")
        {
            var query = _context.CheckLists.AsQueryable();

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(b => b.ChecklistName.Contains(filter));
            }

            var total = await query.CountAsync();
            var checkLists = await query
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

            return Ok(new { Metadata = metadata, CheckLists = checkLists });
        }

        // GET: api/checklist/3
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCheckListById(int id)
        {
            var checkList = await _context.CheckLists.FindAsync(id);
            if (checkList == null)
            {
                return NotFound(new { Message = "Checklist not found" });
            }

            return Ok(checkList);
        }

        // POST: api/CheckList
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CheckList>> CreateNewCheckList(CheckList checkList)
        {
            _context.CheckLists.Add(checkList);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCheckListById), new { id = checkList.Id }, new { Message = "Checklist added successfully", Data = checkList });
        }

        // DELETE: api/CheckList/1
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCheckList(int id)
        {
            var checkList = await _context.CheckLists.FindAsync(id);
            if (checkList == null)
            {
                return NotFound(new { Message = "Checklist not found" });
            }

            _context.CheckLists.Remove(checkList);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Checklist deleted successfully" });
        }
    }
}
