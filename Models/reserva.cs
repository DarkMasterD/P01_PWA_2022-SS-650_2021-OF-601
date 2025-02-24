using System.ComponentModel.DataAnnotations;

namespace P01_2022_SS_650_2021_OF_601.Models
{
    public class reserva
    {
        [Key]
        public int id_reserva { get; set; }
        public DateTime fecha { get; set; }
        public DateTime hora_reserva { get; set; }
        public int horas { get; set; }
        public bool estado { get; set; }
        public int id_usuario { get; set; }
    }
}
