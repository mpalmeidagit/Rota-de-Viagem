using MediatR;
using TravelPlanner.Infrastructure.Data;

namespace TravelPlanner.Application.Features.Rotas.Commands;

public class UpdateRotaCommandHandler : IRequestHandler<UpdateRotaCommand, Unit>
{
    private readonly ApplicationDbContext _context;

    public UpdateRotaCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateRotaCommand request, CancellationToken cancellationToken)
    {
        var rota = await _context.Rotas.FindAsync(request.Id);

        if (rota == null)
            throw new KeyNotFoundException($"Rota com ID {request.Id} não encontrada");

        // Atualiza apenas os campos que foram fornecidos
        if (!string.IsNullOrEmpty(request.Origem))
            rota.Origem = request.Origem;

        if (!string.IsNullOrEmpty(request.Destino))
            rota.Destino = request.Destino;

        if (request.Valor.HasValue)
            rota.Valor = request.Valor.Value;

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}