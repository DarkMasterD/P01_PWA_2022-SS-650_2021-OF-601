using System.ComponentModel.DataAnnotations;

namespace P01_2022_SS_650_2021_OF_601.Models
{
    public class rol
    {
        [Key]
        public int id_rol { get; set; }
        public string nombre {  get; set; }
    }
}
