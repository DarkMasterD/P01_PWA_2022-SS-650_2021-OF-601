using Microsoft.EntityFrameworkCore;

namespace P01_2022_SS_650_2021_OF_601.Models
{
    public class parqueoContext : DbContext
    {
        public parqueoContext(DbContextOptions<parqueoContext> options) : base(options)
        {

        }

        public DbSet<reserva> reserva {  get; set; }
        public DbSet<espacios> espacios { get; set; }
        public DbSet<rol> rol { get; set; }
        public DbSet<sucursal> sucursal { get; set; }
        public DbSet<usuario> usuario { get; set; }
    }
}
