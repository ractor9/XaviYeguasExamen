using Microsoft.EntityFrameworkCore;

namespace Cendres;

public class CendresContext:DbContext
{
    public CendresContext(){}
    //Mapegem els DBsets com mapejaven les taules a hibernate
    public DbSet<Zona> Zona { get; set; }
    public DbSet<Foguera> Foguera { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(!optionsBuilder.IsConfigured) optionsBuilder.UseSqlite("Data Source=cendres.db").UseLazyLoadingProxies();
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Zona>()
            .Property(z => z.Bioma)
            .HasConversion<string>();
        
        modelBuilder.Entity<Zona>()
            .HasOne(z => z.Regio)
            .WithMany(z => z.Subzones)
            .HasForeignKey("RegioID")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Zona>()
            .HasOne(z => z.Foguera)
            .WithOne(f => f.Zona)
            .HasForeignKey<Zona>("FogueraID")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        
        
    }
}