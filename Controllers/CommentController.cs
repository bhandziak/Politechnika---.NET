using Microsoft.AspNetCore.Mvc;
using CarWorkshopProjekt.Data;
using CarWorkshopProjekt.DTOs;
using Microsoft.AspNetCore.Authorization;
using CarWorkshopProjekt.Mappers;
using Microsoft.EntityFrameworkCore;
using static CarWorkshopProjekt.Controllers.ServiceOrderController;
using Microsoft.AspNetCore.Identity;
using System.Data;
namespace CarWorkshopProjekt.Controllers
{
    [ApiController]
    [Route("api/comment")]
    public class CommentController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly CommentMapper _commentMapper = new(); //Mapperly
        private readonly UserManager<User> _userManager;

        public CommentController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // POST: api/comment/addComment
        [Authorize(Roles = "admin,mechanic,receptionist,user")]
        [HttpPost("addComment")]
        public async Task<IActionResult> AddComment([FromBody] AddComment addCommentDTO)
        {
            var order = await _context.ServiceOrders
                .Include(o => o.Comments)
                .FirstOrDefaultAsync(o => o.ServiceOrderId == Guid.Parse(addCommentDTO.ServiceOrderId));

            if (order == null)
            {
                return NotFound("Zlecenie o podanym ID nie istnieje.");
            }
            
            //stworzenie nowego 'comment' przez mapperly
            var newComment = _commentMapper.MapToEntity(addCommentDTO);
            //Dodanie comment
            _context.Comments.Add(newComment);
            await _context.SaveChangesAsync();

            return Ok("Sukces");
        }
        // GET: api/comment/getAll/{serviceOrderId}
        [Authorize(Roles = "admin,mechanic,receptionist,user")]
        [HttpGet("getAll/{serviceOrderId}")]
        public async Task<IActionResult> GetAllCommentsForServiceOrder(Guid serviceOrderId)
        {
            var serviceOrder = await _context.ServiceOrders
                .Include(so => so.Comments)
                .ThenInclude(c => c.User) //info o autorze komentarza
                .FirstOrDefaultAsync(so => so.ServiceOrderId == serviceOrderId);
            if (serviceOrder == null)
            {
                return NotFound("Zlecenie o podanym ID nie istnieje.");
            }
            //DTO
            var commentsDto = serviceOrder.Comments
                .OrderByDescending(c => c.TimestampComment) //OrderByDescending -  najnowsze pierwsze
                .Select(c => new ReturnComment
                {
                    CommentId = c.CommentId,
                    Content = c.Content,
                    TimestampComment = c.TimestampComment,
                    UserId = c.UserId,
                })
                .ToList();

            return Ok(commentsDto);
        }
    }
}
