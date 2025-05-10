using Microsoft.AspNetCore.Mvc;
using OrderManagementAPI.Models;

namespace OrderManagementAPI.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private static List<Product> _productStore = new List<Product>
        {
            new Product { ProductId = 1, Name = "Laptop", Price = 999, Category = "Electronics" },
            new Product { ProductId = 2, Name = "Book", Price = 20, Category = "Education" }
        };

        private static List<Order> _orderStore = new List<Order>();

        [HttpGet]
        public ActionResult<IEnumerable<Order>> GetAllOrders()
        {
            return Ok(_orderStore);
        }

        [HttpGet("{id}")]
        public ActionResult<Order> GetOrderById(int id)
        {
            var order = _orderStore.FirstOrDefault(o => o.OrderId == id);
            if (order == null)
                return NotFound($"Order with ID {id} not found.");

            return Ok(order);
        }

        [HttpPost]
        public ActionResult CreateOrder([FromBody] Order newOrder)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var validatedProducts = new List<Product>();

            foreach (var product in newOrder.Products)
            {
                var existingProduct = _productStore.FirstOrDefault(p => p.ProductId == product.ProductId);
                if (existingProduct == null)
                    return BadRequest($"Product with ID {product.ProductId} does not exist.");

                validatedProducts.Add(existingProduct);
            }

            if (validatedProducts.Count == 0)
                return BadRequest("At least one valid product is required.");

            newOrder.OrderId = _orderStore.Count > 0 ? _orderStore.Max(o => o.OrderId) + 1 : 1;
            newOrder.Products = validatedProducts;
            _orderStore.Add(newOrder);

            return CreatedAtAction(nameof(GetOrderById), new { id = newOrder.OrderId }, newOrder);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateOrder(int id, [FromBody] Order updatedOrder)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingOrder = _orderStore.FirstOrDefault(o => o.OrderId == id);
            if (existingOrder == null)
                return NotFound($"Order with ID {id} not found.");

            var validatedProducts = new List<Product>();
            foreach (var product in updatedOrder.Products)
            {
                var existingProduct = _productStore.FirstOrDefault(p => p.ProductId == product.ProductId);
                if (existingProduct == null)
                    return BadRequest($"Product with ID {product.ProductId} does not exist.");
                validatedProducts.Add(existingProduct);
            }

            existingOrder.CustomerName = updatedOrder.CustomerName;
            existingOrder.OrderDate = updatedOrder.OrderDate;
            existingOrder.TotalAmount = updatedOrder.TotalAmount;
            existingOrder.Products = validatedProducts;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteOrder(int id)
        {
            var order = _orderStore.FirstOrDefault(o => o.OrderId == id);
            if (order == null)
                return NotFound($"Order with ID {id} not found.");

            _orderStore.Remove(order);
            return NoContent();
        }
    }
}
