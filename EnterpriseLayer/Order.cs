﻿namespace EnterpriseLayer
{
    public class Order
    {
        public int Id { get; }
        public string CustomerDNI { get; }
        public string UserName { get; }
        public string CustomerEmail { get; set; }
        public string CreatedByUserId { get; }
        public string CreatedByUserName { get; }
        public int StatusId { get; set; }
        public string StatusName { get; }
        public int PaymentMethodId { get; }
        public string PaymentMethodName { get; }
        public decimal Total { get; }
        public DateTime CreatedAt { get; }
        public List<OrderDetail> OrderDetails { get; }

        // Para el listado de pedidos (en una tabla)
        public Order(int id, string userName, string createdByUserName, string statusName, string paymentMethodName, decimal total, DateTime createdAt)
        {
            Id = id;
            UserName = userName;
            CreatedByUserName = createdByUserName;
            StatusName = statusName;
            PaymentMethodName = paymentMethodName;
            Total = total;
            CreatedAt = createdAt;
        }

        public Order(string CustomerDNI, string customerEmail , string createdByUserId, int statusId, int paymentMethodId, DateTime createdAt, List<OrderDetail> orderDetails)
        {
            this.CustomerDNI = CustomerDNI;
            CustomerEmail = customerEmail;
            CreatedByUserId = createdByUserId;
            StatusId = statusId;
            PaymentMethodId = paymentMethodId;
            CreatedAt = createdAt;
            OrderDetails = orderDetails;
            Total = GetTotal();
        }

        public Order(int id, string userId, string createdByUserId, int statusId, int paymentMethodId, DateTime createdAt, List<OrderDetail> orderDetails)
        {
            Id = id;
            CustomerDNI = userId;
            CreatedByUserId = createdByUserId;
            StatusId = statusId;
            PaymentMethodId = paymentMethodId;
            CreatedAt = createdAt;
            OrderDetails = orderDetails;
            Total = GetTotal();
        }

        private decimal GetTotal()
            => OrderDetails.Sum(od => od.TotalPrice);

        public bool IsCreatedBySameUserName() => UserName.Equals(CreatedByUserName);

    }
}
