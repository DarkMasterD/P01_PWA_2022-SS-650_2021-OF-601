using System.ComponentModel.DataAnnotations;

namespace P01_2022_SS_650_2021_OF_601.Models
{
    public class usuario
    {
        [Key]
        public int id_usuario { get; set; }
        public string nombre { get; set; }
        public string telefono { get; set; }
        public string contrasenia { get; set; }
        public int id_rol {  get; set; }
    }
}
