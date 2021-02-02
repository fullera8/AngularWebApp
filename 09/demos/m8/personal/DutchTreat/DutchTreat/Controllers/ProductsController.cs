using DutchTreat.Data;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
    //Responsible for API calls
    [Route("api/[Controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductsController : Controller
    {
        private readonly IDutchRepository repositry;
        private readonly ILogger<ProductsController> logger;

        public ProductsController(IDutchRepository repositry, ILogger<ProductsController> logger)
        {
            this.repositry = repositry;
            this.logger = logger;
        }

        //Lets API know this is a "get" method. I action result is flexible to return type and bad request if error
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<IEnumerable<Product>> Get()
        {
            try
            {
                return Ok(this.repositry.GetAllProducts());
            }
            catch (Exception e)
            {
                this.logger.LogError($"An Error occured in GetAllProducts: {e}");
                return BadRequest("Failed to Get Products.");
            }
            
        }
    }
}
