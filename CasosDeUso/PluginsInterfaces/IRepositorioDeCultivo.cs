using CoreBusiness.Entidades;

namespace CasosDeUso.PluginsInterfaces
{
    public interface IRepositorioDeCultivo
    {
        Task<List<Cultivo>> BuscarCultivo(string filtro);
        Task<List<Cultivo>> BuscarTodosCultivos();
        Task AdicionarCultivo(Cultivo cultivo);
        Task AtualizarCultivo(Cultivo cultivo);
        Task ExcluirCultivo(Cultivo cultivo);
        Task<Cultivo> BuscarCultivoPorId(Guid id);
        Task AtualizarCultivoAsync(Cultivo cultivo);
        Task AdicionarCultivoAsync(Cultivo cultivo);
        Task ExcluirCultivoAsync(Cultivo cultivo);
    }
}
