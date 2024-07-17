using System;
using System.ComponentModel.DataAnnotations;

namespace StudentManagerMVC.Models
{
    public class Student
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        [Required]
        [StringLength(50)]
        public string PlaceOfBirth { get; set; }

        [Required]
        [StringLength(20)]
        public string Mobile { get; set; }

        public bool IsGraduated { get; set; }

        public string FullName => $"{LastName} {FirstName}";
    }
}
