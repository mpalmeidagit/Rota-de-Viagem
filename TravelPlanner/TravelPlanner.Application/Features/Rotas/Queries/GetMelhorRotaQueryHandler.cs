using MediatR;
using Microsoft.EntityFrameworkCore;
using TravelPlanner.Application.Features.Rotas.Queries.ViewModels;
using TravelPlanner.Domain.Entities;
using TravelPlanner.Infrastructure.Data;

namespace TravelPlanner.Application.Features.Rotas.Queries;
public class GetMelhorRotaQueryHandler : IRequestHandler<GetMelhorRotaQuery, MelhorRotaViewModel>
{
    private readonly ApplicationDbContext _context;

    public GetMelhorRotaQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MelhorRotaViewModel> Handle(GetMelhorRotaQuery request, CancellationToken cancellationToken)
    {
        var rotas = await _context.Rotas.ToListAsync(cancellationToken);

        // Verifica se existem rotas cadastradas
        if (!rotas.Any())
        {
            return new MelhorRotaViewModel
            {
                Rota = "Nenhuma rota cadastrada no sistema",
                ValorTotal = 0
            };
        }

        var (caminho, custo) = EncontrarMelhorRota(rotas, request.Origem, request.Destino);

        // Verifica se encontrou um caminho válido
        if (!caminho.Any())
        {
            return new MelhorRotaViewModel
            {
                Rota = $"Nenhuma rota encontrada de {request.Origem} para {request.Destino}",
                ValorTotal = 0
            };
        }

        return new MelhorRotaViewModel
        {
            Rota = string.Join(" - ", caminho),
            ValorTotal = custo
        };
    }

    private (List<string> caminho, decimal custo) EncontrarMelhorRota(List<Rota> rotas, string origem, string destino)
    {
        var grafo = ConstruirGrafo(rotas);

        // Verifica se origem e destino existem no grafo
        if (!grafo.ContainsKey(origem) || !grafo.ContainsKey(destino))
        {
            return (new List<string>(), 0);
        }

        return TheBestPricePerRoute(grafo, origem, destino);
    }

    private Dictionary<string, Dictionary<string, decimal>> ConstruirGrafo(List<Rota> rotas)
    {
        var grafo = new Dictionary<string, Dictionary<string, decimal>>();

        foreach (var rota in rotas)
        {
            // Garante que o nó de origem existe
            if (!grafo.ContainsKey(rota.Origem))
            {
                grafo[rota.Origem] = new Dictionary<string, decimal>();
            }

            // Se já existe uma rota entre esses pontos, mantém a mais barata
            if (grafo[rota.Origem].ContainsKey(rota.Destino))
            {
                if (rota.Valor < grafo[rota.Origem][rota.Destino])
                {
                    grafo[rota.Origem][rota.Destino] = rota.Valor;
                }
            }
            else
            {
                grafo[rota.Origem][rota.Destino] = rota.Valor;
            }

            // Garante que o nó de destino existe (mesmo sem rotas de saída)
            if (!grafo.ContainsKey(rota.Destino))
            {
                grafo[rota.Destino] = new Dictionary<string, decimal>();
            }
        }

        return grafo;
    }

    // 👈 TheBestPricePerRoute para encontrar o melhor preço é da rota
    private (List<string> caminho, decimal custo) TheBestPricePerRoute(Dictionary<string, Dictionary<string, decimal>> grafo, string origem, string destino)
    {
        var precos = new Dictionary<string, decimal>();
        var pais = new Dictionary<string, string>();
        var visitados = new HashSet<string>();
        var fila = new PriorityQueue<string, decimal>();

        // Inicialização
        foreach (var no in grafo.Keys)
        {
            precos[no] = no == origem ? 0 : decimal.MaxValue;
            fila.Enqueue(no, precos[no]);
        }

        while (fila.Count > 0)
        {
            var atual = fila.Dequeue();
            if (visitados.Contains(atual)) continue;
            visitados.Add(atual);

            // Se chegamos ao destino, podemos terminar
            if (atual == destino) break;

            foreach (var vizinho in grafo[atual])
            {
                var novoPreco = precos[atual] + vizinho.Value;
                if (novoPreco < precos[vizinho.Key])
                {
                    precos[vizinho.Key] = novoPreco;
                    pais[vizinho.Key] = atual;
                    fila.Enqueue(vizinho.Key, novoPreco);
                }
            }
        }

        // Reconstruir o caminho
        var caminho = new List<string>();
        if (!pais.ContainsKey(destino) && origem != destino)
        {
            return (caminho, 0);
        }

        var noAtual = destino;
        while (noAtual != null)
        {
            caminho.Insert(0, noAtual);
            pais.TryGetValue(noAtual, out noAtual);
        }

        return (caminho, precos[destino]);
    }
}