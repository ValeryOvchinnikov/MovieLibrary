using LinqKit;
using Microsoft.EntityFrameworkCore;
using MovieLibrary.DBContext;
using MovieLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibrary.Repositories
{
    internal class MovieRepository : IRepository<Movie>
    {
        private readonly MovieLibraryContext _mlContext;

        public MovieRepository(MovieLibraryContext movieLibraryContext)
        {
            _mlContext = movieLibraryContext;
        }
        public void Create(Movie entity)
        {
            _mlContext.Movies.Add(entity);
            _mlContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var movie = _mlContext.Movies.Find(id);
            _mlContext.Movies.Remove(movie);
        }

        public List<Movie> GetAll()
        {
            var movies = _mlContext.Movies;
            return  movies.ToList();
        }

        public Movie GetById(int id)
        {
            var movie = _mlContext.Movies.Find(id);
            return movie;
        }

        public List<Movie> GetFiltered(ExpressionStarter<Movie> condition)
        {
            var movies = _mlContext.Movies.Where(condition).Select(m => m);
            return movies.ToList();
        }

        public void Update(Movie entity)
        {
            _mlContext.Movies.Update(entity);
            _mlContext.SaveChanges();
        }
    }
}
