using CoreBusiness.Entidades;

namespace CasosDeUso.PluginsInterface;

public interface IRepositorioDeAgrotoxico
{
    Task<List<Agrotoxico>> BuscarAgrotoxico(string filtro);
    Task<List<Agrotoxico>> BuscarTodasAgrotoxico();
    Task AdicionarAgrotoxico(Agrotoxico agrotoxico);
    Task AtualizarAgrotoxico(Agrotoxico agrotoxico);
    Task ExcluirAgrotoxico(Agrotoxico agrotoxico);
    Task<Agrotoxico> BuscarAgrotoxicoPorId(Guid id);
    Task AtualizarAgrotoxicoAsync(Agrotoxico agrotoxico);
    Task AdicionarAgrotoxicoAsync(Agrotoxico agrotoxico);
    Task ExcluirAgrotoxicoAsync(Agrotoxico agrotoxico);
}
