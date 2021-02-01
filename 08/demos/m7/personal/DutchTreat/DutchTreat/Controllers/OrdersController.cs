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
    [Route("api/[Controller]")]
    [ApiController]
    [Produces("application/json")]
    public class OrdersController : Controller
    {
        private readonly IDutchRepository repository;
        private readonly ILogger<OrdersController> logger;

        public OrdersController(IDutchRepository repository, ILogger<OrdersController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<IEnumerable<Order>> Get()
        {
            try
            {
                return Ok(this.repository.GetAllOrders());
            }
            catch (Exception e)
            {
                this.logger.LogError($"An Error occured in GetAllOrders: {e}");
                return BadRequest("Failed to Get Orders.");
            }
        }

        [HttpGet("{id:int}")] //Can pass id in the URL
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<Order> Get(int id) //Use id from URL
        {
            try
            {
                var order = this.repository.GetOrderById(id);

                if (order != null)
                    return Ok(order);
                else
                    return NotFound();
            }
            catch (Exception e)
            {
                this.logger.LogError($"An Error occured in GetOrderById: {e}");
                return BadRequest("Failed to Get Order.");
            }
        }

        //put data into the db
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<Order> Post([FromBody]Order model)
        {
            try
            {
                this.repository.AddEntity(model);
                if(this.repository.SaveAll())
                    return Created($"/api/orders/{model.Id}", model);
            }
            catch (Exception e)
            {
                this.logger.LogError($"An Error occured in AddEntity: {e}");
            }
            return BadRequest("Failed to Save Order.");
        }
    }
}
