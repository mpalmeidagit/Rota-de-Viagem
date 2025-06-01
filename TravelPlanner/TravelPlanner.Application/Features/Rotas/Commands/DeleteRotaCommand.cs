using MediatR;

namespace TravelPlanner.Application.Features.Rotas.Commands;
public class DeleteRotaCommand : IRequest<Unit>
{
    public int Id { get; set; }
}