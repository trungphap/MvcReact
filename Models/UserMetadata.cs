using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReactMvc.Models
{
    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {
    }
    public class UserMetadata
    {
        [EmailAddress(ErrorMessage = "Email invalide")]
        [Display(Name = "Email :")]
        [Required(ErrorMessage="Obligatoire")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Obligatoire")]
        [Display(Name = "User :")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Obligatoire")]
        [Display(Name = "Password :")]
        public string Password { get; set; }
    }
}