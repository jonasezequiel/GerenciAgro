using CoreBusiness.Entidades;

namespace CasosDeUso.Interface.IAgrotoxicoUseCase;

public interface IVisualizarAgrotoxicoUseCase
{
    Task<Agrotoxico> ExecutaAsync(Guid agrotoxicoId);
    Task<List<Agrotoxico>> ExecutaListAsync(string filtro);
}
