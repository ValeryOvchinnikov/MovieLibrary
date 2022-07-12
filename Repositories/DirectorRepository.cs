using LinqKit;
using Microsoft.EntityFrameworkCore;
using MovieLibrary.DBContext;
using MovieLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibrary.Repositories
{
    internal class DirectorRepository : IRepository<Director>
    {
        private readonly MovieLibraryContext _mlContext;

        public DirectorRepository(MovieLibraryContext movieLibraryContext)
        {
            _mlContext = movieLibraryContext;
        }
        public void Create(Director entity)
        {
            _mlContext.Directors.Add(entity);
            _mlContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var director = _mlContext.Directors.Find(id);
            _mlContext.Directors.Remove(director);
        }

        public List<Director> GetAll()
        {
            var movies = _mlContext.Directors;
            return movies.ToList();
        }

        public Director GetById(int id)
        {
            var director = _mlContext.Directors.Find(id);
            return director;
        }

        public List<Director> GetFiltered(ExpressionStarter<Director> condition)
        {
            throw new NotImplementedException();
        }

        public void Update(Director entity)
        {
            _mlContext.Directors.Update(entity);
            _mlContext.SaveChanges();
        }
    }
}
