using CasosDeUso.AgrotoxicoCasoDeUso;
using CasosDeUso.AplicacaoCasoDeUso;
using CasosDeUso.CultivoCasoDeUso;
using CasosDeUso.Interface.InterfaceAgrotoxicoUseCase;
using CasosDeUso.Interface.InterfaceAplicacaoUseCase;
using CasosDeUso.Interface.InterfaceCultivoUseCase;
using CasosDeUso.Interface.InterfacePragaUseCase;
using CasosDeUso.PluginsInterfaces;
using CasosDeUso.PragaCasoDeUso;
using GerenciaAgro.Views;
using GerenciaAgro.Views.Controls;
using Microsoft.Extensions.Logging;
using SqlLite;
using System.Globalization;

namespace GerenciaAgro
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            try
            {
                var builder = MauiApp.CreateBuilder();

                var culture = new CultureInfo("pt-BR");
                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;

                builder
                    .UseMauiApp<App>()
                    .ConfigureFonts(fonts =>
                    {
                        fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                        fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    });

#if DEBUG
                builder.Logging.AddDebug();
#endif
                #region injeção de dependências
                builder.Services.AddSingleton<IRepositorioDeAplicacao, RepositorioAplicacaoSqlLite>();

                builder.Services.AddSingleton<IVisualizarAplicacaoUseCase, VisualizarAplicacaoUseCase>();
                builder.Services.AddSingleton<IApagarAplicacaoUseCase, ApagarAplicacaoUseCase>();
                builder.Services.AddSingleton<IAdicionarAplicacaoUseCase, AdicionarAplicacaoUseCase>();
                builder.Services.AddSingleton<IEditarAplicacaoUseCase, EditarAplicacaoUseCase>();
                builder.Services.AddSingleton<IVisualizarCultivoUseCase, VisualizarCultivoUseCase>();
                builder.Services.AddSingleton<IVisualizarAgrotoxicoUseCase, VisualizarAgrotoxicoUseCase>();
                builder.Services.AddSingleton<IVisualizarPragaUseCase, VisualizarPragaUseCase>();
                builder.Services.AddSingleton<IAdicionarAgrotoxicoUseCase, AdicionarAgrotoxicoUseCase>();
                builder.Services.AddSingleton<IAdicionarPragaUseCase, AdicionarPragaUseCase>();
                builder.Services.AddSingleton<IAdicionarCultivoUseCase, AdicionarCultivoUseCase>();
                builder.Services.AddSingleton<IEditarAgrotoxicoUseCase, EditarAgrotoxicoUseCase>();
                builder.Services.AddTransient<AplicacaoControle>();
                #endregion
                builder.Services.AddSingleton<AplicacaoPagina>();
                builder.Services.AddSingleton<SelecaoItemCadastro>();
                builder.Services.AddSingleton<AplicacaoControle>();
                builder.Services.AddSingleton<AgrotoxicoControle>();
                builder.Services.AddSingleton<PragaControle>();
                builder.Services.AddSingleton<CultivoControle>();
                builder.Services.AddSingleton<AgrotoxicoLista>();
                builder.Services.AddSingleton<AplicacaoLista>();
                builder.Services.AddSingleton<PragaLista>();

                return builder.Build();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao criar o aplicativo: {ex.Message}");
                throw;
            }
        }
    }
}
