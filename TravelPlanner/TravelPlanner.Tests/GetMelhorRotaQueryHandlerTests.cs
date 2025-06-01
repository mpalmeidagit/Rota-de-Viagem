using Microsoft.EntityFrameworkCore;
using TravelPlanner.Application.Features.Rotas.Queries;
using TravelPlanner.Domain.Entities;
using TravelPlanner.Infrastructure.Data;

namespace TravelPlanner.Tests;

public class GetMelhorRotaQueryHandlerTests
{
    private readonly ApplicationDbContext _context;
    private readonly GetMelhorRotaQueryHandler _handler;

    public GetMelhorRotaQueryHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new ApplicationDbContext(options);
        _handler = new GetMelhorRotaQueryHandler(_context);

        // 👈 Dados de teste
        if (!_context.Rotas.Any())
        {
            _context.Rotas.AddRange(new List<Rota>
                {
                    new Rota { Origem = "GRU", Destino = "BRC", Valor = 10 },
                    new Rota { Origem = "BRC", Destino = "SCL", Valor = 5 },
                    new Rota { Origem = "GRU", Destino = "CDG", Valor = 75 },
                    new Rota { Origem = "GRU", Destino = "SCL", Valor = 20 },
                    new Rota { Origem = "GRU", Destino = "ORL", Valor = 56 },
                    new Rota { Origem = "ORL", Destino = "CDG", Valor = 5 },
                    new Rota { Origem = "SCL", Destino = "ORL", Valor = 20 }
                });
            _context.SaveChanges();
        }
    }

    [Fact]
    public async Task Retornar_a_melhor_rota_de_GRU_para_CDG()
    {
        // Arrange
        var query = new GetMelhorRotaQuery { Origem = "GRU", Destino = "CDG" };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal("GRU - BRC - SCL - ORL - CDG", result.Rota);
        Assert.Equal(40, result.ValorTotal);
    }

    [Fact]
    public async Task Retornar_a_melhor_rota_BRC_para_SCL()
    {
        // Arrange
        var query = new GetMelhorRotaQuery { Origem = "BRC", Destino = "SCL" };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal("BRC - SCL", result.Rota);
        Assert.Equal(5, result.ValorTotal);
    }
}
