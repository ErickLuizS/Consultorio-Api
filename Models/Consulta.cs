namespace Consultorio_Api.Models
{
    public class Consulta
    {
        public int Id { get; set; }
        public string Horario { get; set; }
        public DateTime Data { get; set; }
        public int PacienteId { get; set; }
        public virtual Paciente Paciente { get; set; }
    }
}
