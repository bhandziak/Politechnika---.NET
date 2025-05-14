using CarWorkshopProjekt.Data;
using Microsoft.AspNetCore.Mvc;

namespace CarWorkshopProjekt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly AppDbContext _context;

        public CustomerController(ILogger<CustomerController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("GetCustomers")]
        public ActionResult<IEnumerable<Customer>> GetCustomers()
        {
            var customers = _context.Customers.ToList();
            return Ok(customers);
        }
    }
}
