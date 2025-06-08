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
        private readonly ILogger<CommentController> _logger; //logger
        private readonly UserManager<User> _userManager;

        public CommentController(AppDbContext context, UserManager<User> userManager, ILogger<CommentController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
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
                _logger.LogInformation($"Nie znaleziono zasobu o ID {addCommentDTO.ServiceOrderId}"); //logger
                return NotFound("Zlecenie o podanym ID nie istnieje.");
            }
            
            //stworzenie nowego 'comment' przez mapperly
            var newComment = _commentMapper.MapToEntity(addCommentDTO);
            //Dodanie comment
            _context.Comments.Add(newComment);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Dodanie komentarza do zlecenia {order.ServiceOrderId}");//logger
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
            var commentsDto = new List<ReturnComment>();

            foreach (var comment in serviceOrder.Comments.OrderByDescending(c => c.TimestampComment))
            {
                var roles = await _userManager.GetRolesAsync(comment.User);
                var role = roles.FirstOrDefault() ?? "none";

                var dto = _commentMapper.MapToDto(comment, role);
                commentsDto.Add(dto);
            }

            _logger.LogInformation($"Pobranie komentarzy do zlecenia {serviceOrderId}");//logger
            return Ok(commentsDto);
        }
    }
}
