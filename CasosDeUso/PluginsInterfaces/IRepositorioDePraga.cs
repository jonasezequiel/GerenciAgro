using CoreBusiness.Entidades;

namespace CasosDeUso;

public interface IRepositorioDePraga
{
    Task<List<Praga>> BuscarPraga(string filtro);
    Task<List<Praga>> BuscarTodasPraga();
    Task AdicionarPraga(Praga praga);
    Task AtualizarPraga(Praga praga);
    Task ExcluirPraga(Praga praga);
    Task<Praga> BuscarPragaPorId(Guid id);
    Task AtualizarPragaAsync(Praga praga);
    Task AdicionarPragaAsync(Praga praga);
    Task ExcluirPragaAsync(Praga praga);
}
