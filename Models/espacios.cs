using System.ComponentModel.DataAnnotations;

namespace P01_2022_SS_650_2021_OF_601.Models
{
    public class espacios
    {
        [Key]
        public int id_espacios { get; set; }
        public string numero { get; set; }
        public string ubicacion { get; set; }
        public int costo_hora { get; set; }
        public bool estado { get; set; }
        public int id_sucursal { get; set; }
    }
}
