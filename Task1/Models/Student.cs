using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task1.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        //[Index(IsUnique = true)]
        public string IdNumber { get; set; }

        public string FirstName { get; set; }

        public string FatherName { get; set; }
        
        public string GrandFatherName { get; set; }   
        
        public string LastName { get; set; }

        public string Gender { get; set; }

        public string Photograph { get; set; }

        public string IdPhoto { get; set; }

        public string Address { get; set; }

        public string MobileNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime DateOfEnrollmentSchool { get; set; }

        public string Level { get; set; }

        public double EvaluateLastYear { get; set; }

        public DateTime TimeOfRegistrationOnSystem { get; set; }

        public User User { get; set; }


    }
}
