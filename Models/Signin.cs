using System.ComponentModel.DataAnnotations;

namespace Exam.Models
{
    public class Signin
    {
        [Required]
        [EmailAddress]
        [Display(Name="Email")]
        public string SigninEmail {get;set;}
        [Required]
        [DataType(DataType.Password)]
        [Display(Name="Password")]
        public string SigninPassword {get;set;}
    }
}