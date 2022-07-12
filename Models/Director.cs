using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieLibrary.Models
{
    public class Director
    {
        [Key]
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public virtual IList<Movie> Movies { get; set; } 

        public Director(string? firstName, string? lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            Movies = new List<Movie>();
        }
    }
}
