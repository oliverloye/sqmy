  
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Models;

namespace UmbracoWebDev.Models.Account
{
    public class LoginModel : PostRedirectModel
    {


        [System.Runtime.Serialization.DataMemberAttribute(Name = "username", IsRequired = true)]
        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        public string Username { get; set; }
        [System.Runtime.Serialization.DataMemberAttribute(Name = "password", IsRequired = true)]
        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        [System.ComponentModel.DataAnnotations.StringLengthAttribute(256)]
        public string Password { get; set; }
    }
}