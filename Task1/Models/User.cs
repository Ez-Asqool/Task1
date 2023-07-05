using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task1.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public int state { get; set; }

        public DateTime TimeOfRegistrationOnSystem { get; set; }

        public string Password { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public Student Student { get; set; }


    }
}
