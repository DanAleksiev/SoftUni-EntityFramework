﻿using Invoices.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Invoices.DataProcessor.ImportDto
    {
    public class ImportInvoicesDTO
        {
        [Required]
        [Range(1000000000, 1500000000)]
        public int Number { get; set; }
        [Required]
        public string IssueDate { get; set; }
        [Required]
        public string DueDate { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        [EnumDataType(typeof(CurrencyType))]
        public CurrencyType CurrencyType { get; set; }
        [Required]
        public int ClientId { get; set; }
        }
    }
