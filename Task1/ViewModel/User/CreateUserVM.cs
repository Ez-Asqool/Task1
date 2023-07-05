using System.ComponentModel.DataAnnotations;

namespace Task1.ViewModel.User
{
    public class CreateUserVM
    {
        [Required]
        public int state { get; set; }


        [Required]
        public string StudentName { get; set; }

        [Required]
        public string Password { get; set; }

    }

    
}
