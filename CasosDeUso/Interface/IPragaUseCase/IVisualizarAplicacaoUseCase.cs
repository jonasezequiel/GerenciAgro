using CoreBusiness.Entidades;

namespace CasosDeUso.Interface.IPragaUseCase;

public interface IVisualizarPragaUseCase
{
    Task<Praga> ExecutaAsync(Guid pragaId);
    Task<List<Praga>> ExecutaListAsync(string filtro);
}
