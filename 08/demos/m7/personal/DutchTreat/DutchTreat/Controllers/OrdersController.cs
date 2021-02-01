using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
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
        public ActionResult<OrderViewModel> Post([FromBody]OrderViewModel model)
        {
            try
            {
                if (ModelState.IsValid) //model validation check
                {
                    var newOrder = new Order()
                    {
                        OrderDate = model.OrderDate,
                        OrderNumber = model.OrderNumber,
                        Id = model.OrderId
                    };
                    if (newOrder.OrderDate == DateTime.MinValue) //If user didn't specify a date
                    {
                        newOrder.OrderDate = DateTime.Now;
                    }
                        
                    this.repository.AddEntity(newOrder);
                    if (this.repository.SaveAll())
                    {
                        model.OrderId = newOrder.Id;
                        model.OrderDate = newOrder.OrderDate;
                        model.OrderNumber = newOrder.OrderNumber;
                        return Created($"/api/orders/{model.OrderId}", model);
                    }     
                }
                else
                    return BadRequest(ModelState);//Returns what is wrong with the model validation
            }
            catch (Exception e)
            {
                this.logger.LogError($"An Error occured in AddEntity: {e}");
            }
            return BadRequest("Failed to Save Order.");
        }
    }
}
