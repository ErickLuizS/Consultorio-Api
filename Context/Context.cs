using Consultorio_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Consultorio_Api.DBContext
{
    public class Context : DbContext
    {
        public DbSet<Consulta> Consultas { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //String de conexão SQLite
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "consultas.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Consulta>()
              .HasKey(c => c.Id);

            modelBuilder.Entity<Paciente>()
             .HasKey(p => p.Id);// Define a chave primária para a entidade Paciente

            modelBuilder.Entity<Paciente>()
                .HasMany(p => p.Consultas)
                .WithOne(c => c.Paciente)
                .HasForeignKey(c => c.PacienteId);
        }


    }
}
