using Consultorio_Api.DBContext;
using Consultorio_Api.Fillters;
using Consultorio_Api.Models;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ConsultaController : ControllerBase
{
    private readonly Context _context;

    public ConsultaController(Context context)
    {
        _context = context;
    }

    // Lista todas as consultas agendadas
    [HttpGet]
    public IActionResult Get()
    {
        var consultas = _context.Consultas.ToList();
        return Ok(consultas);
    }

    // Agenda uma nova consulta
    [HttpPost]
    public IActionResult AgendarConsulta(Consulta consulta)
    {
        // Validação
        ConsultaValidator consultaValidator = new ConsultaValidator();
        var validation = consultaValidator.Validate(consulta);

        if (!validation.IsValid)
        {
            return BadRequest(validation.Errors.Select(error => error.ErrorMessage));
        }

        // Verifica se a data já passou
        if (consulta.Data < DateTime.Now)
        {
            return BadRequest("A data da consulta já passou.");
        }

        // Verifica a disponibilidade do horário
        bool horarioDisponivel = VerificarHorarioDisponivel(consulta.Data, consulta.Horario);

        if (!horarioDisponivel)
        {
            return BadRequest("O horário já está ocupado.");
        }

        // Adiciona a consulta ao contexto do Entity Framework
        _context.Consultas.Add(consulta);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetConsulta), new { id = consulta.Id }, consulta);
    }

    // Verifica se um horário está disponível (simulação simples)
    private bool VerificarHorarioDisponivel(DateTime data, string horario)
    {
        // Verifica se já existe uma consulta agendada para a data e horário especificados
        return !_context.Consultas.Any(c => c.Data == data && c.Horario == horario);
    }

    // Atualiza uma consulta por ID
    [HttpPut("{id}")]
    public IActionResult AtualizarConsulta(int id, [FromBody] Consulta consultaAtualizada)
    {
        // Verifica se a consulta com o ID especificado existe
        var consultaExistente = _context.Consultas.FirstOrDefault(c => c.Id == id);

        if (consultaExistente == null)
        {
            return NotFound();
        }

        // Verifica se a data da consulta já passou
        if (consultaAtualizada.Data < DateTime.Now)
        {
            return BadRequest("A data da consulta já passou.");
        }

        // Verifica a disponibilidade do horário
        bool horarioDisponivel = VerificarHorarioDisponivel(consultaAtualizada.Data, consultaAtualizada.Horario);

        if (!horarioDisponivel)
        {
            return BadRequest("O horário já está ocupado.");
        }

        // Atualiza os dados da consulta existente com os dados da consulta atualizada
        consultaExistente.Data = consultaAtualizada.Data;
        consultaExistente.Horario = consultaAtualizada.Horario;

        _context.SaveChanges();

        return Ok(consultaExistente);
    }

    // Obtém uma consulta por ID
    [HttpGet("{id}")]
    public IActionResult GetConsulta(int id)
    {
        var consulta = _context.Consultas.FirstOrDefault(c => c.Id == id);
        if (consulta == null)
        {
            return NotFound();
        }
        return Ok(consulta);
    }

    // Exclui uma consulta por ID
    [HttpDelete("{id}")]
    public IActionResult DeleteConsulta(int id)
    {
        if (id <= 0)
        {
            return BadRequest("ID inválido");
        }

        var consulta = _context.Consultas.FirstOrDefault(c => c.Id == id);
        if (consulta == null)
        {
            return NotFound();
        }

        _context.Consultas.Remove(consulta);
        _context.SaveChanges();

        return Ok("Consulta removida com sucesso.");
    }
}
