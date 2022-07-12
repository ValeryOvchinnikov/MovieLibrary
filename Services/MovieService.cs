using LinqKit;
using MovieLibrary.Interfaces;
using MovieLibrary.Models;
using MovieLibrary.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibrary.Services
{
    internal class MovieService: IMovieService
    {
        private IRepository<Director> _directorRepository;
        private IRepository<Movie> _movieRepository;

        public MovieService(IRepository<Director> directorRepository, IRepository<Movie> movieRepository)
        {
            _directorRepository = directorRepository;
            _movieRepository = movieRepository;
        }

        public ObservableCollection<Movie> GetAllMovies()
        {
            var movies = _movieRepository.GetAll();

            return new ObservableCollection<Movie>(movies as List<Movie>);
        }

        public ObservableCollection<Movie> GetFilteredMovies(ExpressionStarter<Movie> filter)
        {
            var movies = _movieRepository.GetFiltered(filter);

            return new ObservableCollection<Movie>(movies as List<Movie>);
        }
    }
}
