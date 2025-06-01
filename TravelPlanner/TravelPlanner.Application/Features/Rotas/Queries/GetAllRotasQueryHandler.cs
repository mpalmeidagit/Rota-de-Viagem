using MediatR;
using Microsoft.EntityFrameworkCore;
using TravelPlanner.Application.Features.Rotas.Queries.ViewModels;
using TravelPlanner.Application.Moldes;
using TravelPlanner.Infrastructure.Data;

namespace TravelPlanner.Application.Features.Rotas.Queries;

public class GetAllRotasQueryHandler : IRequestHandler<GetAllRotasQuery, PaginatedList<RotaViewModel>>
{
    private readonly ApplicationDbContext _context;

    public GetAllRotasQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<RotaViewModel>> Handle(GetAllRotasQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Rotas.AsNoTracking();

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(r => r.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(r => new RotaViewModel
            {
                Id = r.Id,
                Origem = r.Origem,
                Destino = r.Destino,
                Valor = r.Valor
            })
            .ToListAsync(cancellationToken);

        return new PaginatedList<RotaViewModel>(items, totalCount, request.PageNumber, request.PageSize);
    }
}