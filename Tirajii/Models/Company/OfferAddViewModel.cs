﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Tirajii.Data.Models;

namespace Tirajii.Models.Company
{
    public class OfferAddViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public string DueDate { get; set; }

        public int? CompanyId { get; set; }

        [Required]
        [Range(0, 10000)]
        public decimal Payment { get; set; }
        public ICollection<TruckingCategory> Categories { get; set; } = new List<TruckingCategory>();
    }
}