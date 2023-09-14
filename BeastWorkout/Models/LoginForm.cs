using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BeastWorkout.Models
{
    public class LoginForm
    {
        [DisplayName("Login : ")]
        [Required(ErrorMessage = "Le login est requis.")]
        public string Login { get; set; }
        [DisplayName("Mot de passe : ")]
        [Required(ErrorMessage = "Le mot de passe est requis.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
