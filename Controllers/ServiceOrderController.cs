using CarWorkshopProjekt.Data;
using CarWorkshopProjekt.Helpers;
using CarWorkshopProjekt.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using CarWorkshopProjekt.Services;
using CarWorkshopProjekt.Mappers;

namespace CarWorkshopProjekt.Controllers
{
    [ApiController]
    [Route("api/serviceOrder")]
    public class ServiceOrderController: ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ServiceOrderMapper _serviceOrderMapper = new(); //Mapperly vehicle
        private readonly UserManager<User> _userManager;
        public ServiceOrderController(
            AppDbContext context,
            ICustomerService customerService, // Services
            UserManager<User> userManager
            )
        {
            _context = context;
            _userManager = userManager;
        }
        //Enum ze stanami ServiceOrder
        public enum OrderStatus
        {
            Nowe,
            WTrakcie,
            Zakończone,
            Anulowane
        }

        // GET: api/serviceOrder/getAll
        [Authorize(Roles = "admin,receptionist,user,mechanic")]
        [HttpGet("getAll")]
        public ActionResult<IEnumerable<ServiceOrder>> GetAll()
        {
            var serviceOrders = _context.ServiceOrders.ToList(); // pobranie encji
            var returnDtos = _serviceOrderMapper.ToReturnDtoList(serviceOrders); // mapowanie do DTO

            return Ok(returnDtos);
        }

        // PUT: api/serviceOrder/createOrder 
        [Authorize(Roles = "admin,receptionist")] //SPRAWDZIĆ ROLE!!
        [HttpPut("createOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateServiceOrder serviceOrderDTO)
        {
            var order = await _context.ServiceOrders.FindAsync(serviceOrderDTO.serviceOrderID);

            if (order == null)
            {
                return NotFound("Zlecenie o podanym ID nie istnieje.");
            }
            //Service
            //stworzenie (update) ServiceOrder przez mapperly
            _serviceOrderMapper.UpdateServiceOrder(serviceOrderDTO, order);

            //ręczna zmiana statusu ServiceOrder
            order.StatusOrder = OrderStatus.Nowe.ToString();        
          
            await _context.SaveChangesAsync();
            return Ok();
        }

        // GET: api/serviceOrder/getMechanicsServices
        [Authorize(Roles = "admin,receptionist,mechanic")] //SPRAWDZIĆ ROLE!!
        [HttpGet("getMechanicsServices")]
        public async Task<ActionResult<IEnumerable<ServiceOrder>>> GetMechanicsServicesAsync()
        {
            var appUser = await _userManager.GetUserAsync(User);
            if (appUser == null)
            {
                return Unauthorized();
            }

            var serviceOrders = await _context.ServiceOrders
                .Where(so => so.UserId == appUser.Id && so.StatusOrder != null)
                .ToListAsync();
            var returnDtos = _serviceOrderMapper.ToReturnDtoList(serviceOrders); // mapowanie do DTO

            return Ok(returnDtos);
        }


    }
}
