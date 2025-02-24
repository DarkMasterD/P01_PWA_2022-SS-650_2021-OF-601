using Microsoft.EntityFrameworkCore;

namespace P01_2022_SS_650_2021_OF_601.Models
{
    public class parqueoContext : DbContext
    {
        public parqueoContext(DbContextOptions<parqueoContext> options) : base(options)
        {

        }
    }
}
