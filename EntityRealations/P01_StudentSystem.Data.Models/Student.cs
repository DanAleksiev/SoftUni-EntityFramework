using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace P01_StudentSystem.Data.Models
    {
    public class Students
        {
        public int StudentId { get; set; }
        [MaxLength(80)]
        [Unicode]
        [Required]
        public string Name { get; set; }
        [MaxLength(10)]
        public string Phonenumber { get; set; }
        [Required]
        public DateTime RegisteredOn { get; set; }
        }
    }
