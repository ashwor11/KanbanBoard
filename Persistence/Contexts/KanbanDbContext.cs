using System.Security.Cryptography.X509Certificates;
using Core.Security.Entities;
using Domain.Entities.Abstract;
using Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Contexts;

public class KanbanDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<Board> Boards { get; set; }
    public DbSet<CardFeedback> CardFeedbacks { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<JobFeedback> JobFeedbacks { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<OperationClaim> OperationClaims { get; set; }
    public KanbanDbContext( DbContextOptions options) : base(options )
    {
        Database.EnsureCreated();

    }

    protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(KanbanDbContext).Assembly);

        modelBuilder.Entity<Feedback>().UseTpcMappingStrategy().HasOne(f => f.Person).WithMany()
            .HasForeignKey(x => x.WrittenByPersonId).OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<User>().UseTpcMappingStrategy();


        modelBuilder.Entity<PersonBoard>(personBoard =>
        {
            personBoard.HasKey(x => x.Id);
            personBoard.HasOne(x => x.Board).WithMany(x => x.PersonBoards).HasForeignKey(pb => pb.BoardId).OnDelete(DeleteBehavior.Cascade);
            personBoard.HasOne(pb => pb.Person).WithMany(p => p.PersonBoards).HasForeignKey(pb => pb.PersonId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Card>(card=>
        {
            card.HasKey(pc => pc.Id);
            card.HasOne(c => c.AssignedPerson).WithMany(p => p.AssignedCards).HasForeignKey(pc => pc.AssignedPersonId)
                .IsRequired(false);
            card.HasOne(c => c.Board).WithMany(p => p.Cards).HasForeignKey(pc => pc.BoardId);
            card.HasMany(c => c.Feedbacks).WithOne().HasForeignKey(x => x.CardId);
            card.HasMany(c => c.Jobs).WithOne().HasForeignKey(j => j.CardId);

        });

        modelBuilder.Entity<Job>(job =>
        {
            job.HasMany(j => j.Feedbacks).WithOne().HasForeignKey(feedback => feedback.JobId);
            job.Property(x => x.JobDescription).HasColumnName("JobDescription");
        });

        modelBuilder.Entity<RefreshToken>(refreshToken =>
        {
            refreshToken.HasOne(r => r.User).WithMany(p => p.RefreshTokens).HasForeignKey(r => r.UserId);
        });

        modelBuilder.Entity<UserOperationClaim>(userOperationClaim =>
        {
            userOperationClaim.HasOne(r => r.User).WithMany(p => p.UserOperationClaims)
                .HasForeignKey(user => user.UserId);

            userOperationClaim.HasOne(userOperationClaim => userOperationClaim.OperationClaim).WithMany()
                .HasForeignKey(o => o.OperationClaimId);
        });

        modelBuilder.Entity<Board>(board =>
        {
            board.Property(x => x.CreatorUserId).HasColumnName("CreatorUserId");
        });










    }

    
}