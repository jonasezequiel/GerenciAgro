using CasosDeUso.AplicacaoUseCase;
using CasosDeUso.Interface.IAgrotoxicoUseCase;
using CasosDeUso.PluginsInterfaces;
using DadosEmMemoria;
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
            builder.Services.AddSingleton<IApagarAplicacaoUseCase, ApagarPragaUseCase>();
            builder.Services.AddSingleton<IAdicionarAplicacaoUseCase, AdicionarPragaUseCase>();
            builder.Services.AddSingleton<IEditarAplicacaoUseCase, EditarAplicacaoUseCase>();
            builder.Services.AddTransient<AplicacaoControle>();
            #endregion
            builder.Services.AddSingleton<AplicacaoPagina>();
            builder.Services.AddSingleton<SelecaoItemCadastro>();
            builder.Services.AddSingleton<AplicacaoControle>();
            builder.Services.AddSingleton<AgrotoxicoControle>();
            builder.Services.AddSingleton<IRepositorioDeAplicacao, RepositorioAplicacaoSqlLite>();
            //builder.Services.AddSingleton<EditarContatoPage>();
            //builder.Services.AddSingleton<AdicionarContatoPage>();

            return builder.Build();
        }
    }
}
