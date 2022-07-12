using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MovieLibrary.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }

        [ForeignKey("Director")]
        public int DirectorId { get; set; }
        public virtual Director? Director { get; set; }
        public int Year { get; set; }
        public int Rating { get; set; }
        
        public Movie()
        {

        }

        public Movie(string? name,int year,  int rating)
        {
            Name = name;
            Rating = rating;
            Year = year;
        }
    }
}
