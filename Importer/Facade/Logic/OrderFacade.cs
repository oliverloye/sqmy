using ClassLibrary.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Importer.Models;
using AlphaSolutions.Core.Diagnostics.TimeMeasures;

namespace Importer.Facade.Logic
{
    public class OrderFacade
    {
        public Order GetAllOrderInfo(int id)
        {
            Order order = new Order();
            using (var _dbcontext = new DatabaseContext())
            {
                order = _dbcontext.Orders.Find(id);
                _dbcontext.Entry(order).Collection(x => x.OrderDetails).Load();

                order = _dbcontext.Orders.Find(id);
                order.OrderDetails = _dbcontext.Orders
                    .Include("OrderDetails")
                    .Include("OrderDetails.Product")
                    .Include("OrderDetails.Product.Category")
                    .FirstOrDefault(o => o.OrderId == id)
                    .OrderDetails;
            }
            return order;
        }

        public List<Order> GetAllOrdersForUser(string customerId)
        {
            using (var _dbcontext = new DatabaseContext())
            {
                    return _dbcontext.Orders.Where(
                        c => c.CustomerID == customerId).ToList();
            }
        }

        public List<Order> GetListOfAllFullOrdersForUser(string customerId)
        {
            using (var _dbcontext = new DatabaseContext())
            {
                var OrderList = _dbcontext.Orders.Where(
                    c => c.CustomerID == customerId).ToList();

                foreach (var item in OrderList)
                {
                    item.OrderDetails = _dbcontext.Orders
                    .Include("OrderDetails")
                    .Include("OrderDetails.Product")
                    .Include("OrderDetails.Product.Category")
                    .FirstOrDefault(o => o.OrderId == item.OrderId)
                    .OrderDetails;
                }

                return OrderList;
            }
        }

        public void SaveOrderWithDetails(List<OrderDetail> orderDetails)
        {
            using (var _dbcontext = new DatabaseContext())
            {
                try
                {
                    Order o = new Order() { };
                    _dbcontext.Orders.Add(o);
                    _dbcontext.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
            }
        }

        public bool CheckIfPaymentTransactionIdAlreadyExists(string id)
        {
            using (new TimeMeasure("Check if Payment Transaction Id exist in database"))
            {
                try
                {
                    Guid x = Guid.Parse(id);
                    using (var _dbcontext = new DatabaseContext())
                    {
                        var query = _dbcontext.Orders
                               .Where(s => s.PaymentTransactionId == x)
                               .FirstOrDefault<Order>();
                        return query != null ? true : false;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public Guid GeneratePaymentTransactionId()
        {
            using (new TimeMeasure("Generate Payment Transaction Id"))
            {
                Guid id = Guid.NewGuid();
                bool isNotUnique = CheckIfPaymentTransactionIdAlreadyExists(id.ToString());
                if (isNotUnique)
                {
                    GeneratePaymentTransactionId();
                }
                return id;
            }
        }
    }
}
