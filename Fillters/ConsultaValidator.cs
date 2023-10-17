using Consultorio_Api.Models;
using FluentValidation;

namespace Consultorio_Api.Fillters
{
    public class ConsultaValidator : AbstractValidator<Consulta>
    {
        public ConsultaValidator()
        {
            RuleFor(consulta => consulta.Data)
                .NotEmpty()
                .WithMessage("A data da consulta é obrigatória.")
                .Must(BeAValidDate)
                .WithMessage("A data da consulta deve ser uma data válida.");

            RuleFor(consulta => consulta.Horario)
                .NotEmpty()
                .WithMessage("O horário da consulta é obrigatório.");
        }

        // Verifica se a data é válida
        private bool BeAValidDate(DateTime date)
        {
            return date != default(DateTime);
        }
    }
}

