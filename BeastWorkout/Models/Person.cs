using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BeastWorkout.Models
{
    public class Person
    {
        public int Id { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        [EmailAddress]
        public string Mail { get; set; }
        [Required]
        public string Login { get; set; }
        [Required(ErrorMessage = "Please enter your password.")]
        //[DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please confirm your password.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        //[DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
        public string? Password_reset_token { get; set; }
        public string? Auth_key { get; set; }
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        public bool? Is_validate { get; set; }
        public int? Id_type_person { get; set; }
        public int Id_country { get; set; }

        public virtual TypePerson? TypePerson { get; set; }
        public virtual Country? Country { get; set; }

        public string? Token { get; set; }
    }
}
