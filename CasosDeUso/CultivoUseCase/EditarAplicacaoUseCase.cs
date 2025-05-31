using CasosDeUso.Interface.IAgrotoxicoUseCase;
using CasosDeUso.PluginsInterfaces;
using CoreBusiness.Entidades;

namespace CasosDeUso.AplicacaoUseCase;

public class EditarAplicacaoUseCase : IEditarAplicacaoUseCase
{
    private readonly IRepositorioDeAplicacao _aplicacaoRepository;
    public EditarAplicacaoUseCase(IRepositorioDeAplicacao repositorioDeAplicacao)
    {
        _aplicacaoRepository = repositorioDeAplicacao ?? throw new ArgumentNullException(nameof(repositorioDeAplicacao));
    }
    public async Task ExecutaAsync(Aplicacao aplicacao)
    {
        Aplicacao aplicacaoSelecinada = await _aplicacaoRepository.BuscarAplicacaoPorId(aplicacao.Id);

        if (aplicacaoSelecinada is null)
        {
            return;
        }

        aplicacaoSelecinada.Agrotoxico = aplicacao.Agrotoxico;
        aplicacaoSelecinada.Cultivo = aplicacao.Cultivo;
        aplicacaoSelecinada.DataAplicacao = aplicacao.DataAplicacao;
        aplicacaoSelecinada.PragasAlvos = aplicacao.PragasAlvos;
        await _aplicacaoRepository.AtualizarAplicacaoAsync(aplicacaoSelecinada);
    }
}
