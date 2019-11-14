using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    //https://github.com/thiago-vivas/Articles/tree/master/NetCoreDataAnnotation
    //https://www.c-sharpcorner.com/article/using-net-core-data-annotation/
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50,ErrorMessage ="Name cannot exceed 50 characters")]
        public string Name { get; set; }
        //searched on internet
        //[Required(ErrorMessage = "The email address is required")]
        //[EmailAddress(ErrorMessage ="Invalid Email Format")]
        //or
        //Reference: https://stackoverflow.com/questions/16712043/email-address-validation-using-asp-net-mvc-data-type-attributes
        //[RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", 
        //ErrorMessage = "E-mail is not valid")]
        //or
        //Kudvenkat specified the below lines
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
        ErrorMessage ="Invalid Email Format")]
        [Display(Name="Office Email")]
        public string Email { get; set; }
        [Required]
        public Dept? Department { get; set; }
        public string PhotoPath { get; set; }
    }
}
