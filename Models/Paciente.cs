namespace Consultorio_Api.Models
{
    public class Paciente
    {
     public int Id { get; set; }
     public string Nome { get; set; }
     public int Idade { get; set; }
     public virtual ICollection<Consulta> Consultas { get; set; }

    }
}
