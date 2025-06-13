using CoreBusiness.Entidades;

namespace CasosDeUso.Interface
{
    public interface IEditarAplicacaoUseCase
    {
        Task ExecutaAsync(Aplicacao aplicacao);
    }
}
