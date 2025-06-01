using MediatR;
using TravelPlanner.Application.Features.Rotas.Queries.ViewModels;
using TravelPlanner.Application.Moldes;

namespace TravelPlanner.Application.Features.Rotas.Queries;

public class GetAllRotasQuery : IRequest<PaginatedList<RotaViewModel>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}