﻿using Footballers.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Footballers.DataProcessor.ImportDto
    {
    public class ImportTeamsDTO
        {
        [Required]
        [MinLength(3)]
        [MaxLength(40)]
        [RegularExpression(@"^[a-zA-Z0-9 .-]+$")]
        public string Name { get; set; }
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Nationality { get; set; }
        [Required]
        public int Trophies { get; set; }

        public int[] Footballers { get; set; }
    }
    }
