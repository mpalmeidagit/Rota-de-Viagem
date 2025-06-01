using FluentValidation;
using TravelPlanner.Application.Features.Rotas.Commands;

namespace TravelPlanner.Application.Features.Validators;

public class UpdateRotaCommandValidator : AbstractValidator<UpdateRotaCommand>
{
    public UpdateRotaCommandValidator()
    {
        // Origem - só valida se foi fornecido 👈
        When(x => !string.IsNullOrEmpty(x.Origem), () =>
        {
            RuleFor(x => x.Origem)
                .NotEmpty().WithMessage("Origem é obrigatória")
                .NotNull().WithMessage("Origem não pode ser nula")
                .NotEqual("string").WithMessage("Origem não pode ser 'string'");
        });

        // Destino - só valida se foi fornecido 👈
        When(x => !string.IsNullOrEmpty(x.Destino), () =>
        {
            RuleFor(x => x.Destino)
                .NotEmpty().WithMessage("Destino é obrigatório")
                .NotNull().WithMessage("Destino não pode ser nulo")
                .NotEqual("string").WithMessage("Destino não pode ser 'string'");
        });

        // Valida se ambos Origem e Destino foram fornecidos e são iguais
        RuleFor(x => x)
            .Must(x => string.IsNullOrEmpty(x.Origem) ||
                      string.IsNullOrEmpty(x.Destino) ||
                      x.Origem != x.Destino)
            .WithMessage("Origem e Destino não podem ser iguais");

        // Valor - só valida se foi fornecido
        When(x => x.Valor.HasValue, () =>
        {
            RuleFor(x => x.Valor!.Value)
                .GreaterThan(0).WithMessage("Valor deve ser maior que zero");               
        });

        // 👉 Pelo menos um campo deve ser fornecido para atualização
        RuleFor(x => x)
            .Must(x => !string.IsNullOrEmpty(x.Origem) ||
                      !string.IsNullOrEmpty(x.Destino) ||
                      x.Valor.HasValue)
            .WithMessage("Pelo menos um campo deve ser fornecido para atualização");
    }
}