using MainBot.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace MainBot.Database;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; init; }
    public DbSet<Event> Events { get; init; }
    public DbSet<EventToUser> EventToUsers { get; init; }
    private IConfiguration _configuration;
    

    public ApplicationContext(IConfiguration configuration)
    {
        _configuration = configuration;
        Database.EnsureCreated();
    }



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration["database:connection_string"]);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(x => x.Uid);
            entity.Property(x => x.Uid).HasDefaultValueSql("uuid_generate_v4()"); // 
            entity.Property(x => x.JoinDate).HasDefaultValueSql("now()");
            
            entity.Property(x => x.Uid).HasColumnName("uid");
            entity.Property(x => x.DiscordId).HasColumnName("discord_id");
            entity.Property(x => x.Discriminant).HasColumnName("disciminant");
            entity.Property(x => x.Name).HasColumnName("name");
            entity.Property(x => x.Discriminant).HasColumnName("disciminant");
        });

        modelBuilder.Entity<EventToUser>(entity =>
        {
            entity.HasKey(x => x.Uid);
            entity.Property(x => x.Uid).HasDefaultValueSql("uuid_generate_v4()"); //
            entity.Property(x => x.CreatedAt).HasDefaultValueSql("now()");
        });

       

        base.OnModelCreating(modelBuilder);
    }
}