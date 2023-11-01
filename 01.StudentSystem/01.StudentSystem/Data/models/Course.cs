
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace P01_StudentSystem.Data.Models
    {
    public class Course
        {
        public Course(ICollection<StudentCourse> studentsCourses, ICollection<Resource> resources, ICollection<Homework> homeworks)
            {
            StudentsCourses = studentsCourses;
            Resources = resources;
            Homeworks = homeworks;
            }

        public int CourseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
        public ICollection<StudentCourse> StudentsCourses { get; set; }
        public ICollection<Resource> Resources { get; set; }
        public ICollection<Homework> Homeworks { get; set; }
        }
    }
