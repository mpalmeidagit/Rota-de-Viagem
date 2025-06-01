using MediatR;

namespace TravelPlanner.Application.Features.Rotas.Commands;

public class CreateRotaCommand : IRequest<int>
{
    public string? Origem { get; set; }
    public string? Destino { get; set; }
    public decimal Valor { get; set; }
}