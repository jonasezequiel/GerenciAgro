using CoreBusiness.Entidades;

namespace CasosDeUso.PluginsInterfaces;

public interface IRepositorioDeAplicacao
{
    #region Interface Repositorio de Aplicacao
    Task<List<Aplicacao>> BuscarAplicacao(string filtro);
    Task<List<Aplicacao>> BuscarTodasAplicacao();
    Task AdicionarAplicacao(Aplicacao aplicacao);
    Task AtualizarAplicacao(Aplicacao aplicacao);
    Task ExcluirAplicacao(Aplicacao aplicacao);
    Task<Aplicacao> BuscarAplicacaoPorId(Guid id);
    Task AtualizarAplicacaoAsync(Aplicacao aplicacao);
    Task AdicionarAplicacaoAsync(Aplicacao aplicacao);
    Task ExcluirAplicacaoAsync(Aplicacao aplicacao);
    #endregion

    #region Interface Repositorio de Agrotoxico
    Task<List<Agrotoxico>> BuscarAgrotoxico(string filtro);
    Task<List<Agrotoxico>> BuscarTodosAgrotoxicos();
    Task AdicionarAgrotoxico(Agrotoxico agrotoxico);
    Task AtualizarAgrotoxico(Agrotoxico agrotoxico);
    Task ExcluirAgrotoxico(Agrotoxico agrotoxico);
    Task<Agrotoxico> BuscarAgrotoxicoPorId(Guid id);
    Task AtualizarAgrotoxicoAsync(Agrotoxico agrotoxico);
    Task AdicionarAgrotoxicoAsync(Agrotoxico agrotoxico);
    Task ExcluirAgrotoxicoAsync(Agrotoxico agrotoxico);
    #endregion

    #region Interface Repositorio de Cultivo
    Task<List<Cultivo>> BuscarCultivo(string filtro);
    Task<List<Cultivo>> BuscarTodosCultivos();
    Task AdicionarCultivo(Cultivo cultivo);
    Task AtualizarCultivo(Cultivo cultivo);
    Task ExcluirCultivo(Cultivo cultivo);
    Task<Cultivo> BuscarCultivoPorId(Guid id);
    Task AtualizarCultivoAsync(Cultivo cultivo);
    Task AdicionarCultivoAsync(Cultivo cultivo);
    Task ExcluirCultivoAsync(Cultivo cultivo);
    #endregion

    #region Interface Repositorio De Praga
    Task<List<Praga>> BuscarPraga(string filtro);
    Task<List<Praga>> BuscarTodasPragas();
    Task AdicionarPraga(Praga praga);
    Task AtualizarPraga(Praga praga);
    Task ExcluirPraga(Praga praga);
    Task<Praga> BuscarPragaPorId(Guid id);
    Task AtualizarPragaAsync(Praga praga);
    Task AdicionarPragaAsync(Praga praga);
    Task ExcluirPragaAsync(Praga praga);
    #endregion
}
