using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UmbracoWebDev.Models
{
    public class SearchModel
    {
        [Required]
        public string Query { get; set; }

        public SearchModel()
        {

        }

        public SearchModel(string query)
        {
            Query = query;
        }

    }
}