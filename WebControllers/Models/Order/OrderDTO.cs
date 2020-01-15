using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebControllers.Models.Order
{
    public class OrderDTO
    {
      

        public int Order_ID { get; set; }
        public int Member_ID { get; set; }
        public string Order_Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

     

        public OrderDTO()
        {
        }



        public OrderDTO(int member_ID, string order_Status, DateTime createdOn, DateTime updatedOn)
        {
            Member_ID = member_ID;
            Order_Status = order_Status;
            CreatedOn = createdOn;
            UpdatedOn = updatedOn;
        }

        public OrderDTO(int order_Id, string order_Status, DateTime createdOn)
        {
            Order_ID = order_Id;
            Order_Status = order_Status;
            CreatedOn = createdOn;
        }


        public OrderDTO(int member_ID, string order_Status)
        {
            Member_ID = member_ID;
            Order_Status = order_Status;
        }

        public OrderDTO(int order_ID, int member_ID, string order_Status, DateTime createdOn)
        {
            Order_ID = order_ID;
            Member_ID = member_ID;
         
            Order_Status = order_Status;
            CreatedOn = createdOn;
        }
    }
}