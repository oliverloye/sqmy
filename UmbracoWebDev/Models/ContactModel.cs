using System.ComponentModel.DataAnnotations;

namespace UmbracoWebDev.Models
{
    public class ContactModel
    {
        [Required]
        [Display(Name = "First Name:")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name:")]
        public string LastName { get; set; }


        [Required]
        [Display(Name = "Subject:")]
        public string Subject { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address:")]
        public string EmailAddress { get; set; }

        [Required]
        [Display(Name = "Message:")]
        public string Message { get; set; }
    }
}