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
using System.Threading.Tasks;

namespace CarWorkshopProjekt.Controllers
{
    [ApiController]
    [Route("api/serviceOrder")]
    public class ServiceOrderController: ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ServiceOrderMapper _serviceOrderMapper = new();
        private readonly IServiceOrderService _serviceOrderService;
        private readonly UserManager<User> _userManager;
        public ServiceOrderController(
            AppDbContext context,
            ICustomerService customerService, // Services
            IServiceOrderService serviceOrderService,
            UserManager<User> userManager
            )
        {
            _context = context;
            _userManager = userManager;
            _serviceOrderService = serviceOrderService;
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
        public async Task<ActionResult<IEnumerable<ServiceOrder>>> GetAll()
        {
            var result = await _serviceOrderService.GetAllAsync();
            return Ok(result);
        }

        // PUT: api/serviceOrder/createOrder 
        [Authorize(Roles = "admin,receptionist")]
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
        [Authorize(Roles = "mechanic")]
        [HttpGet("getMechanicsServices")]
        public async Task<ActionResult<IEnumerable<ServiceOrder>>> GetMechanicsServicesAsync()
        {
            var appMechanic = await _userManager.GetUserAsync(User);
            if (appMechanic == null)
            {
                return Unauthorized();
            }

            var result = await _serviceOrderService.GetAllAsync();

            var filteredResult = result.Where(so => so.Mechanic != null && so.Mechanic.Id == appMechanic.Id);

            return Ok(filteredResult);
        }


    }
}
