using FluentValidation;
using TravelPlanner.Application.Features.Rotas.Commands;

namespace TravelPlanner.Application.Features.Validators;

public class CreateRotaCommandValidator : AbstractValidator<CreateRotaCommand>
{
    public CreateRotaCommandValidator()
    {
        RuleFor(x => x.Origem)
            .NotEmpty().WithMessage("Origem é obrigatória")
            .NotNull().WithMessage("Origem não pode ser nula")
            .NotEqual("string").WithMessage("Origem não pode ser 'string'")
            .NotEqual("").WithMessage("Origem não pode ser vazia");

        RuleFor(x => x.Destino)
            .NotEmpty().WithMessage("Destino é obrigatório")
            .NotNull().WithMessage("Destino não pode ser nulo")
            .NotEqual(x => x.Origem).WithMessage("Origem e Destino não podem ser iguais")
            .NotEqual("string").WithMessage("Destino não pode ser 'string'")
            .NotEqual("").WithMessage("Destino não pode ser vazio");

        RuleFor(x => x.Valor)
            .GreaterThan(0).WithMessage("Valor deve ser maior que zero")
            .LessThan(10000).WithMessage("Valor máximo permitido é 9999.99") // 👈 Opcional, ajuste conforme necessário
            .NotEqual(0).WithMessage("Valor não pode ser zero");
    }
}
