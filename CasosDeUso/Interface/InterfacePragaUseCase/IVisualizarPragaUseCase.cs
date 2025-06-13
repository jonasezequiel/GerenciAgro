using CoreBusiness.Entidades;

namespace CasosDeUso.Interface.InterfacePragaUseCase
{
    public interface IVisualizarPragaUseCase
    {
        Task<Praga> ExecutaAsync(Guid pragaId);
        Task<List<Praga>> ExecutaListAsync(string filtro);
    }
}
