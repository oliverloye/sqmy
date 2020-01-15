using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Web.Models;

namespace UmbracoWebDev.Models.Account
{
    public class MemberModel : PostRedirectModel
    {
        public int MemberId { get; set; }
    }
}