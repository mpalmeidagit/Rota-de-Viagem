using MediatR;
using TravelPlanner.Application.Features.Rotas.Queries.ViewModels;

namespace TravelPlanner.Application.Features.Rotas.Queries;

public class GetMelhorRotaQuery : IRequest<MelhorRotaViewModel>
{
    public string Origem { get; set; } = string.Empty;
    public string Destino { get; set; } = string.Empty;
}