using CoreBusiness.Entidades;

namespace CasosDeUso.PluginsInterfaces
{
    public interface IRepositorioDeAgrotoxico
    {
        Task<List<Agrotoxico>> BuscarAgrotoxico(string filtro);
        Task<List<Agrotoxico>> BuscarTodosAgrotoxicos();
        Task AdicionarAgrotoxico(Agrotoxico agrotoxico);
        Task AtualizarAgrotoxico(Agrotoxico agrotoxico);
        Task ExcluirAgrotoxico(Agrotoxico agrotoxico);
        Task<Agrotoxico> BuscarAgrotoxicoPorId(Guid id);
        Task AtualizarAgrotoxicoAsync(Agrotoxico agrotoxico);
        Task AdicionarAgrotoxicoAsync(Agrotoxico agrotoxico);
        Task ExcluirAgrotoxicoAsync(Agrotoxico agrotoxico);
    }
}
