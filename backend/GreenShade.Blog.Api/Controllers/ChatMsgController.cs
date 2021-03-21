using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GreenShade.Blog.DataAccess.Data;
using GreenShade.Blog.Domain.Models;
using GreenShade.Blog.Domain.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using GreenShade.Blog.Domain.Dto;
using GreenShade.Blog.Api.Common;
using GreenShade.Blog.Api.Filters;

namespace GreenShade.Blog.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ChatMsgController : ControllerBase
    {
        private readonly BlogSysContext _context;

        public ChatMsgController(BlogSysContext context)
        {
            _context = context;
        }
        [Authorize]
        [ActionName("msgs")]
        [HttpGet]
        public async Task<ActionResult<ApiResult<MsgDto>>> GetChatMassages(string roomid = "", int status = 0, int pi = 1, int ps = 10)
        {

            MsgDto ret = null;
            try
            {
                if (ret == null)
                {
                    ret = new MsgDto();
                    var artList = await _context.ChatMassages.Include(x => x.User)
                .OrderByDescending(a => a.Status).OrderByDescending(a => a.CreateDate)
                .Where(a => a.RoomId == roomid && a.Status == status).Skip((pi - 1) * ps).Take(ps).ToListAsync();
                    List<MsgItemDto> msgs = new List<MsgItemDto>();
                    string userId = "";
                    if (HttpContext.User.Identity.IsAuthenticated && HttpContext.User.Claims != null)
                    {
                        userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    }
                    artList.ForEach(msg => msgs.Add(new MsgItemDto(msg, userId)));
                    ret.Msgs = msgs;
                    ret.PageTotal = await _context.ChatMassages.Where(a => a.Status == status).CountAsync();
                }

            }
            catch (Exception ex)
            {
                //return BadRequest();
            }
            return ApiResult<MsgDto>.Ok(ret);
        }

        [ActionName("msg_detail")]
        [HttpGet]
        public async Task<ActionResult<ChatMassage>> GetChatMassage(string id)
        {
            var chatMassage = await _context.ChatMassages.FindAsync(id);

            if (chatMassage == null)
            {
                return NotFound();
            }

            return chatMassage;
        }

        // PUT: api/ChatMsg/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChatMassage(string id, ChatMassage chatMassage)
        {
            if (id != chatMassage.Id)
            {
                return BadRequest();
            }

            _context.Entry(chatMassage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChatMassageExists(id))
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
        [Authorize]
        [ActionName("save_msg")]
        [HttpPost]
        [ExceptionHandle("发送失败")]
        public async Task<ActionResult<ApiResult>> PostChatMassage([FromBody]ChatMsgViewModel chatMassage)
        {
            try
            {
                ChatMassage massage = new ChatMassage();
                massage.Content = chatMassage.Content;
                massage.MediaType = 0;
                massage.RoomId = chatMassage.RoomId;
                massage.CreateDate = DateTime.Now;
                if (HttpContext.User.Identity.IsAuthenticated && HttpContext.User.Claims != null)
                {
                    foreach (var item in HttpContext.User.Claims)
                    {
                        if (item.Type == ClaimTypes.NameIdentifier)
                        {
                            massage.UserId = item.Value;
                        }
                    }
                }
                _context.ChatMassages.Add(massage);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return ApiResult.Fail("发送失败");
            }
            return ApiResult.Ok("发送成功");
        }

        // DELETE: api/ChatMsg/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ChatMassage>> DeleteChatMassage(string id)
        {
            var chatMassage = await _context.ChatMassages.FindAsync(id);
            if (chatMassage == null)
            {
                return NotFound();
            }

            _context.ChatMassages.Remove(chatMassage);
            await _context.SaveChangesAsync();

            return chatMassage;
        }

        private bool ChatMassageExists(string id)
        {
            return _context.ChatMassages.Any(e => e.Id == id);
        }
    }
}
