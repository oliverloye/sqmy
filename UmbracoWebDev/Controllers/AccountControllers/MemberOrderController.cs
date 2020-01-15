using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using System.Data.SqlClient;
using WebControllers.Helpers;
using UmbracoWebDev.Models.Account;
using System.Web.Security;
using WebControllers.Models.Order;
using WebControllers.Controllers;
using Importer.Facade.Logic;

namespace UmbracoWebDev.Controllers.AccountControllers
{


    public class MemberOrderController : SurfaceController
    {
        OrderFacade orderFacade = new OrderFacade();
        public ActionResult GetAllOrdersForMember()
        {
            MembershipUser user = Membership.GetUser();
            var result = orderFacade.GetListOfAllFullOrdersForUser(user.ProviderUserKey.ToString());
            return PartialView(result);
        }
    }
}