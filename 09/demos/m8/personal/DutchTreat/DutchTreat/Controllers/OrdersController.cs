using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
    //Specify the login is required and cookies will not be used
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
    public class OrdersController : Controller
    {
        private readonly IDutchRepository repository;
        private readonly ILogger<OrdersController> logger;
        private readonly IMapper mapper;

        public OrdersController(IDutchRepository repository, ILogger<OrdersController> logger, IMapper mapper)
        {
            this.repository = repository;
            this.logger = logger;
            this.mapper = mapper;
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
                    return Ok(this.mapper.Map<Order, OrderViewModel>(order));
                else
                    return NotFound();
            }
            catch (Exception e)
            {
                this.logger.LogError($"An Error occured in GetOrderById: {e}");
                return BadRequest("Failed to Get Order.");
            }
        }

        [HttpGet] //Include order Items
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<Order> Get(bool includeItems = true) //Use id from URL
        {
            try
            {
                var results = this.repository.GetAllOrders(includeItems);
                return Ok(this.mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(results));
            }
            catch (Exception e)
            {
                this.logger.LogError($"An Error occured in GetAllOrders: {e}");
                return BadRequest("Failed to Get Orders.");
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
                    var newOrder = this.mapper.Map<OrderViewModel, Order>(model);
                    if (newOrder.OrderDate == DateTime.MinValue) //If user didn't specify a date
                        newOrder.OrderDate = DateTime.Now;
                        
                    this.repository.AddEntity(newOrder);
                    if (this.repository.SaveAll())
                        return Created($"/api/orders/{model.OrderId}", this.mapper.Map<Order, OrderViewModel>(newOrder)); 
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
