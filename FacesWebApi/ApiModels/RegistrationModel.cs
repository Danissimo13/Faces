using System.ComponentModel.DataAnnotations;

namespace FacesWebApi.ApiModels
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "You didn't enter your nickname.")]
        [MinLength(5, ErrorMessage = "Minimal lenght for nickname is 5.")]
        public string Nickname { get; set; }

        [Required(ErrorMessage = "You didn't enter your email.")]
        [EmailAddress(ErrorMessage = "Email address had a non-valid format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "You didn't enter your password.")]
        [MinLength(8, ErrorMessage = "Minimal lenght for password is 8.")]
        public string Password { get; set; }
    }
}
