using BlazorApp2.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp2.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Adresse> Adressen { get; set; }
}