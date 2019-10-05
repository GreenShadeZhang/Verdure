using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GreenShade.Blog.DataAccess.Data;
using GreenShade.Blog.Domain.Models;

namespace GreenShade.Blog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatGroupsController : ControllerBase
    {
        private readonly ChatContext _context;

        public ChatGroupsController(ChatContext context)
        {
            _context = context;
        }

        // GET: api/ChatGroups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChatGroup>>> GetGroups()
        {
            return await _context.Groups.ToListAsync();
        }

        // GET: api/ChatGroups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChatGroup>> GetChatGroup(string id)
        {
            var chatGroup = await _context.Groups.FindAsync(id);

            if (chatGroup == null)
            {
                return NotFound();
            }

            return chatGroup;
        }

        // PUT: api/ChatGroups/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChatGroup(string id, ChatGroup chatGroup)
        {
            if (id != chatGroup.Id)
            {
                return BadRequest();
            }

            _context.Entry(chatGroup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChatGroupExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ChatGroups
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<ChatGroup>> PostChatGroup(ChatGroup chatGroup)
        {
            _context.Groups.Add(chatGroup);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChatGroup", new { id = chatGroup.Id }, chatGroup);
        }

        // DELETE: api/ChatGroups/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ChatGroup>> DeleteChatGroup(string id)
        {
            var chatGroup = await _context.Groups.FindAsync(id);
            if (chatGroup == null)
            {
                return NotFound();
            }

            _context.Groups.Remove(chatGroup);
            await _context.SaveChangesAsync();

            return chatGroup;
        }

        private bool ChatGroupExists(string id)
        {
            return _context.Groups.Any(e => e.Id == id);
        }
    }
}
