namespace TravelPlanner.Application.Features.Rotas.Queries.ViewModels;

public class RotaViewModel
{
    public int Id { get; set; }
    public string? Origem { get; set; }
    public string? Destino { get; set; }
    public decimal? Valor { get; set; }
}