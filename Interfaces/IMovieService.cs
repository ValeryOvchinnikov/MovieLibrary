using LinqKit;
using MovieLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibrary.Interfaces
{
    internal interface IMovieService
    {
        ObservableCollection<Movie> GetAllMovies();
        ObservableCollection<Movie> GetFilteredMovies(ExpressionStarter<Movie> filter);

    }
}
