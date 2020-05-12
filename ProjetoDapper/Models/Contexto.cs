using Microsoft.EntityFrameworkCore;

namespace ProjetoDapper.Models
{
    public class Contexto:  DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {
        }
        public DbSet<Usuario> Usuario {get;set;}
        public DbSet<Produto> Produto {get;set;}


    }
}