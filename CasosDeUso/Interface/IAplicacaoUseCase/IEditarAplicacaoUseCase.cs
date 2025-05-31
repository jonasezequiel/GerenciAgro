using CoreBusiness.Entidades;

namespace CasosDeUso.Interface.IAplicacaoUseCase
{
    public interface IEditarAplicacaoUseCase
    {
        Task ExecutaAsync(Aplicacao aplicacao);
    }
}
