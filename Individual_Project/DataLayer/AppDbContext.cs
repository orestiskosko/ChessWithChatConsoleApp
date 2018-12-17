namespace DataLayer
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Data.Entity.Validation;
    using System.Data.Entity.Infrastructure;

    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
            : base("name=AppDbContext")
        {
        }

        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(e => e.Games)
                .WithRequired(e => e.User_BlackPlayer)
                .HasForeignKey(e => e.BlackPlayer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Games1)
                .WithRequired(e => e.User_WhitePlayer)
                .HasForeignKey(e => e.WhitePlayer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Games2)
                .WithOptional(e => e.User_Winner)
                .HasForeignKey(e => e.Winner);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Games3)
                .WithOptional(e => e.User_Turn)
                .HasForeignKey(e => e.Turn);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Messages)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.Receiver)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Messages1)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.Sender)
                .WillCascadeOnDelete(false);

        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                // Get all error messages from all validation errors
                var ErrorMessages = e.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

                // Join the list to a signle string
                var FullErrorMessage = string.Join(". ", ErrorMessages);

                // Combine the original exception message with the error validation messages
                var ExceptionMessage = string.Concat($"{e.Message}. ", $"The validation errors are: {FullErrorMessage}");

                throw new DbEntityValidationException(ExceptionMessage, e.EntityValidationErrors);
            }
        }
    }
}
