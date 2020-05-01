using Microsoft.EntityFrameworkCore;
using zMovies.Core.Entities;

namespace zMovies.Infrastructure.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options) {}

        public  DataContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Many to Many relationship for MovieCollectionItem table
            modelBuilder.Entity<MovieCollectionItem>()
            .HasKey(mc => new {mc.MovieId, mc.MovieCollectionId});

            modelBuilder.Entity<MovieCollectionItem>()
            .HasOne(mc => mc.MovieCollection)
            .WithMany(mg => mg.MovieCollectionItems)
            .HasForeignKey(mg => mg.MovieCollectionId)
            .OnDelete(DeleteBehavior.Cascade);

            // Many to Many relationship for MovieRating
            modelBuilder.Entity<MovieRating>()
            .HasKey(mr => new {mr.MovieId, mr.RatingId, mr.UserId});

            modelBuilder.Entity<MovieRating>()
            .HasOne(mr => mr.Rating)
            .WithMany(mr => mr.MovieRatings)
            .HasForeignKey(mr => mr.RatingId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MovieRating>()
            .HasOne(mr => mr.User)
            .WithMany(mr => mr.MovieRatings)
            .HasForeignKey(mr => mr.UserId)
            .OnDelete(DeleteBehavior.Cascade);

            // One to many relationship for User to MovieCollection
            modelBuilder.Entity<User>()
            .HasMany(u => u.MovieCollections)
            .WithOne(u => u.User)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();   
        }

        public DbSet<Rating> Ratings { get; set; }
        public DbSet<MovieCollection> MovieCollections { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<MovieCollectionItem> MovieCollectionItems { get; set; }
        public DbSet<MovieRating> MovieRatings { get; set; }
    }
}