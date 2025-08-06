using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Entities
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public required string Name { get; set; }
        [Required]
        [Range(1, 100)]
        public decimal Price { get; set; }

        //Naviation Property
        [ValidateNever]
        public Genre? Genre { get; set; }

        //Foreign Key
        public int GenreId { get; set; }

        public DateOnly ReleaseDate { get; set; }
    }
}
