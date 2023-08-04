using System.ComponentModel.DataAnnotations;

namespace LevRobinGuss.VIewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string MetaMuskNumber { get; set; }

        public bool Agree { get; set; }

    }
}
