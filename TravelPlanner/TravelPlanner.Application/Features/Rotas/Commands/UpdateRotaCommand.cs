using MediatR;

namespace TravelPlanner.Application.Features.Rotas.Commands;

public class UpdateRotaCommand : IRequest<Unit>
{
    public int Id { get; set; }
    public string? Origem { get; set; }
    public string? Destino { get; set; }
    public decimal? Valor { get; set; }
}