using MediatR;
using TravelPlanner.Infrastructure.Data;

namespace TravelPlanner.Application.Features.Rotas.Commands;

public class DeleteRotaCommandHandler : IRequestHandler<DeleteRotaCommand, Unit>
{
    private readonly ApplicationDbContext _context;

    public DeleteRotaCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRotaCommand request, CancellationToken cancellationToken)
    {
        var rota = await _context.Rotas.FindAsync(request.Id);

        if (rota == null)
            throw new KeyNotFoundException($"Rota com ID {request.Id} não encontrada");

        _context.Rotas.Remove(rota);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}