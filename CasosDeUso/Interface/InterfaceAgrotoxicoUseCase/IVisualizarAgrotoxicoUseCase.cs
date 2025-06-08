using CoreBusiness.Entidades;

namespace CasosDeUso.Interface.InterfaceAgrotoxicoUseCase
{
    public interface IVisualizarAgrotoxicoUseCase
    {
        Task<Agrotoxico> ExecutaAsync(Guid agrotoxicoId);
        Task<List<Agrotoxico>> ExecutaListAsync(string filtro);
    }
}
