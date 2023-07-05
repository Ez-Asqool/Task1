﻿using System.ComponentModel.DataAnnotations;

namespace Task1.ViewModel.Student
{
    public class DeleteStudentVM
    {
        [Required]
        public string IdNumber { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string FatherName { get; set; }

        [Required]
        public string GrandFatherName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string PhotographName { get; set; }

        [Required]
        public string IdPhotoName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string MobileNumber { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public DateTime DateOfEnrollmentSchool { get; set; }

        [Required]
        public string Level { get; set; }

        [Required]
        public double EvaluateLastYear { get; set; }
    }
}
