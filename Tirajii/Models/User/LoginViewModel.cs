using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tirajii.Models.User
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
