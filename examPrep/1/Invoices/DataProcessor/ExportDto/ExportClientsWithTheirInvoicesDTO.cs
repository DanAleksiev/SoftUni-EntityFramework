﻿using System.Xml.Serialization;

namespace Invoices.DataProcessor.ExportDto
    {
    [XmlType("Client")]
    public class ExportClientsWithTheirInvoicesDTO
        {
        [XmlAttribute("InvoicesCount")]
        public int InvoicesCount { get; set; }

        [XmlElement("ClientName")]
        public string ClientName { get; set; }

        [XmlElement("VatNumber")]
        public string VatNumber { get; set; }

        [XmlArray("Invoices")]
        public AllInvoices[] Invoices { get; set; }

        }
    [XmlType("Invoice")]
    public class AllInvoices
        {
        [XmlElement("InvoiceNumber")]
        public int InvoiceNumber { get; set; }

        [XmlElement("InvoiceAmount")]
        public decimal InvoiceAmount { get; set; }

        [XmlElement("DueDate")]
        public string DueDate { get; set; }

        [XmlElement("Currency")]
        public string Currency { get; set; }
        //[XmlIgnore]
        //public DateTime IssueDate { get; set; }
        }

    }
