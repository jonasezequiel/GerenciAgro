using CasosDeUso.Interface.IAplicacaoUseCase;
using CasosDeUso.PluginsInterface;
using CoreBusiness.Entidades;

namespace CasosDeUso.AplicacaoUseCase;

public class ApagarAplicacaoUseCase : IApagarAplicacaoUseCase
{
    private readonly IRepositorioDeAplicacao _aplicacaoRepository;

    public ApagarAplicacaoUseCase(IRepositorioDeAplicacao repositorioDeAplicacao)
    {
        _aplicacaoRepository = repositorioDeAplicacao ?? throw new ArgumentNullException(nameof(repositorioDeAplicacao));
    }

    public async Task ExecutaAsync(Aplicacao contato)
    {
        await _aplicacaoRepository.ExcluirAplicacaoAsync(contato);
    }
}
