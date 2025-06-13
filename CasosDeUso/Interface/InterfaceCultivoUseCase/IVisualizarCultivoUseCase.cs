using CoreBusiness.Entidades;

namespace CasosDeUso.Interface.InterfaceCultivoUseCase
{
    public interface IVisualizarCultivoUseCase
    {
        Task<Cultivo> ExecutaAsync(Guid cultivoId);
        Task<List<Cultivo>> ExecutaListAsync(string filtro);
    }
}
