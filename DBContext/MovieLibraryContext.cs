using Microsoft.EntityFrameworkCore;
using MovieLibrary.Models;

namespace MovieLibrary.DBContext
{
    internal class MovieLibraryContext : DbContext
    {
        const string connectionString = "Server=(localdb)\\mssqllocaldb;Database=MovieLibraryDB;Trusted_Connection=True;";

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Director> Directors { get; set; }

        public MovieLibraryContext() : base()
        {
            Database.EnsureCreated();
        }

        public MovieLibraryContext(DbContextOptions<MovieLibraryContext> options)
           : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies().UseSqlServer(connectionString, options => options.EnableRetryOnFailure());
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>().HasOne<Director>(d => d.Director).WithMany(m => m.Movies).HasForeignKey(m => m.DirectorId).HasPrincipalKey(m => m.Id);
        }

    }
}
