using CasosDeUso;
using CasosDeUso.Interface;
using CasosDeUso.PluginsInterfaces;
using GerenciaAgro.Views;
using GerenciaAgro.Views.Controls;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace GerenciaAgro
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
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
            //builder.Services.AddSingleton<IRepositorioDeContatos, RepositorioContatosSqlLite>();
            builder.Services.AddSingleton<IRepositorioDeAplicacao, DadosEmMemoria.Dados>();
            builder.Services.AddSingleton<IVisualizarAplicacaoUseCase, VisualizarAplicacaoUseCase>();
            builder.Services.AddSingleton<IApagarAplicacaoUseCase, ApagarAplicacaoUseCase>();
            builder.Services.AddSingleton<IAdicionarAplicacaoUseCase, AdicionarAplicacaoUseCase>();
            builder.Services.AddSingleton<IEditarAplicacaoUseCase, EditarAplicacaoUseCase>();
            builder.Services.AddTransient<AplicacaoControle>();
            #endregion
            builder.Services.AddSingleton<AplicacaoPagina>();
            builder.Services.AddSingleton<SelecaoItemCadastro>();
            builder.Services.AddSingleton<AplicacaoControle>();
            //builder.Services.AddSingleton<EditarContatoPage>();
            //builder.Services.AddSingleton<AdicionarContatoPage>();

            return builder.Build();
        }
    }
}
