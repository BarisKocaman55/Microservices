using FreeCourse.Services.Order.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Domain.Order.Aggragate
{
    // EF Core Features
    // --Owned Types
    // --Shadow Property
    // --Backing Field
    // Bir Aggregate Root bir entity kullanıyorsa, o entity başka bir Aggrageta Root tarafından kullanılmamalıdır.
    public class Order : Entity, IAggragateRoot
    {
        private readonly List<OrderItem> _orderItems;

        public DateTime CreatedDate { get; private set; }
        public Address Address { get; private set; }
        public string BuyerId { get; private set; }
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;
        
        // Lazy loading yapılmak istendiğinde entity üzerinde bir default constructer olmalıdır.
        public Order()
        {

        }

        public Order(string buyerId, Address address)
        {
            Address = address;
            BuyerId = buyerId;
            CreatedDate = DateTime.Now;
            _orderItems = new List<OrderItem>();
        }

        public void AddOrderItem(string productId, string productName, Decimal price, string pictureUrl)
        {
            var existsProduct = _orderItems.Any(i => i.ProductId == productId);
            if(!existsProduct)
            {
                var newOrderItem = new OrderItem(productId, productName, pictureUrl, price);
                _orderItems.Add(newOrderItem);
            }
        }

        public Decimal GetTotalPrice => _orderItems.Sum(i => i.Price);
    }
}
