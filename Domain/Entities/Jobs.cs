using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Entities
{
    public class Jobs
    {
        public int Id { get; set; }

        [Required]
        public required string JobName { get; set; }
        [Required]
        [Range(1, 10000)]
        public decimal OfferedFair { get; set; }
        public DateOnly JobPostedDate { get; set; }
    }
}
