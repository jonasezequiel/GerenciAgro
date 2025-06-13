using CoreBusiness.Entidades;

namespace CasosDeUso.Interface.InterfaceAplicacaoUseCase
{
    public interface IEditarAplicacaoUseCase
    {
        Task ExecutaAsync(Aplicacao aplicacao);
    }
}
