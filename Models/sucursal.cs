using System.ComponentModel.DataAnnotations;

namespace P01_2022_SS_650_2021_OF_601.Models
{
    public class sucursal
    {
        [Key]
        public int id_sucursal { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public string telefono { get; set; }
        public string administrador { get; set; }
        public int numero_espacios { get; set; }
    }
}
