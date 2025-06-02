using Microsoft.AspNetCore.Mvc;
using CarWorkshopProjekt.Data;
using CarWorkshopProjekt.DTOs;
using Microsoft.AspNetCore.Authorization;
using CarWorkshopProjekt.Mappers;
using Microsoft.EntityFrameworkCore;
namespace CarWorkshopProjekt.Controllers
{
    [ApiController]
    [Route("api/part")]
    public class PartController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PartMapper _partMapper = new(); //Mapperly


        public PartController(AppDbContext context)
        {
            _context = context;
        }


        // GET: api/part/getAll
        [Authorize(Roles = "admin,mechanic")]
        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<PartDTO>>> GetAll()
        {
            var parts = await _context.Parts.ToListAsync(); // pobranie encji
            var result = _partMapper.ToReturnDtoList(parts); //mapowanie na dto

            return Ok(result);
        }


    }
}
