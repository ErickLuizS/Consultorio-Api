using Consultorio_Api.DBContext;
using Consultorio_Api.Models;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class PacienteController : ControllerBase
{
    private readonly Context _context;

    public PacienteController(Context context)
    {
        _context = context;
    }

    // Obtém a lista de pacientes
    [HttpGet]
    public IActionResult Get()
    {
        var pacientes = _context.Pacientes.ToList();
        return Ok(pacientes);
    }

    // Cria um novo paciente
    [HttpPost]
    public IActionResult CriarPaciente(Paciente paciente)
    {
        _context.Pacientes.Add(paciente);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetPaciente), new { id = paciente.Id }, paciente);
    }

    // Obtém um paciente por ID
    [HttpGet("{id}")]
    public IActionResult GetPaciente(int id)
    {
        var paciente = _context.Pacientes.FirstOrDefault(p => p.Id == id);
        if (paciente == null)
        {
            return NotFound();
        }
        return Ok(paciente);
    }

    // Atualiza um paciente por ID
    [HttpPut("{id}")]
    public IActionResult AtualizarPaciente(int id, [FromBody] Paciente pacienteAtualizado)
    {
        var pacienteExistente = _context.Pacientes.FirstOrDefault(p => p.Id == id);

        if (pacienteExistente == null)
        {
            return NotFound();
        }

        pacienteExistente.Nome = pacienteAtualizado.Nome;
        pacienteExistente.Idade = pacienteAtualizado.Idade;
        _context.SaveChanges();

        return Ok(pacienteExistente);
    }

    // Exclui um paciente por ID
    [HttpDelete("{id}")]
    public IActionResult DeletarPaciente(int id)
    {
        if (id <= 0)
        {
            return BadRequest("ID inválido");
        }

        var paciente = _context.Pacientes.FirstOrDefault(p => p.Id == id);
        if (paciente == null)
        {
            return NotFound();
        }

        _context.Pacientes.Remove(paciente);
        _context.SaveChanges();

        return Ok("Paciente removido com sucesso.");
    }
}
