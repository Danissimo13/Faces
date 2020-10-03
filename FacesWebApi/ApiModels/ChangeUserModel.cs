using System.ComponentModel.DataAnnotations;

namespace FacesWebApi.ApiModels
{
    public class ChangeUserModel
    {
        [Required(ErrorMessage = "You didn't enter your nickname.")]
        [MinLength(5, ErrorMessage = "Minimal lenght for nickname is 5.")]
        public string Nickname { get; set; }

        [Required(ErrorMessage = "You didn't enter your email address.")]
        [EmailAddress(ErrorMessage = "Email address had a non-valid format.")]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
