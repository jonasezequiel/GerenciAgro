using CoreBusiness.Entidades;

namespace CasosDeUso.Interface.ICultivoUseCase;

public interface IVisualizarCultivoUseCase
{
    Task<Cultivo> ExecutaAsync(Guid CultivoId);
    Task<List<Cultivo>> ExecutaListAsync(string filtro);
}
