﻿using System.Xml.Serialization;

namespace ProductShop.DTOs.Export
    {
    [XmlType("Category")]
    public class ExportCategoryProductDTO
        {
        [XmlElement("name")]
        public string? Name { get; set; }

        [XmlElement("count")]
        public int Count { get; set; }

        [XmlElement("averagePrice")]
        public decimal AvaragePrice { get; set; }

        [XmlElement("totalRevenue")]
        public decimal TotalRevenue { get; set; }

        }

    }
