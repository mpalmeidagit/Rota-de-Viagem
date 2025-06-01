using MediatR;
using TravelPlanner.Domain.Entities;
using TravelPlanner.Infrastructure.Data;

namespace TravelPlanner.Application.Features.Rotas.Commands;

public class CreateRotaCommandHandler : IRequestHandler<CreateRotaCommand, int>
{
    private readonly ApplicationDbContext _context;

    public CreateRotaCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateRotaCommand request, CancellationToken cancellationToken)
    {
        var rota = new Rota
        {
            Origem = request.Origem?.ToUpper(),
            Destino = request.Destino?.ToUpper(),
            Valor = request.Valor
        };

        _context.Rotas.Add(rota);
        await _context.SaveChangesAsync(cancellationToken);

        return rota.Id;
    }
}