using CasosDeUso.Interface.IAgrotoxicoUseCase;
using CasosDeUso.PluginsInterfaces;
using CoreBusiness.Entidades;

namespace CasosDeUso.AplicacaoUseCase;

public class AdicionarPragaUseCase : IAdicionarAplicacaoUseCase
{
    private readonly IRepositorioDeAplicacao _aplicacaoRepository;

    public AdicionarPragaUseCase(IRepositorioDeAplicacao aplicacaoRepository)
    {
        _aplicacaoRepository = aplicacaoRepository ?? throw new ArgumentNullException(nameof(aplicacaoRepository));
    }

    public async Task ExecutaAsync(Aplicacao aplicacao)
    {
        await _aplicacaoRepository.AdicionarAplicacao(aplicacao);
    }
}
