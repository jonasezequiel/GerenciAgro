using CasosDeUso.Interface.IAgrotoxicoUseCase;
using CasosDeUso.PluginsInterfaces;
using CoreBusiness.Entidades;

namespace CasosDeUso.AplicacaoUseCase;

public class ApagarPragaUseCase : IApagarAplicacaoUseCase
{
    private readonly IRepositorioDeAplicacao _aplicacaoRepository;

    public ApagarPragaUseCase(IRepositorioDeAplicacao repositorioDeAplicacao)
    {
        _aplicacaoRepository = repositorioDeAplicacao ?? throw new ArgumentNullException(nameof(repositorioDeAplicacao));
    }

    public async Task ExecutaAsync(Aplicacao contato)
    {
        await _aplicacaoRepository.ExcluirAplicacaoAsync(contato);
    }
}
