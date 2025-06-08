using CasosDeUso.AgrotoxicoCasoDeUso;
using CasosDeUso.AplicacaoCasoDeUso;
using CasosDeUso.CultivoCasoDeUso;
using CasosDeUso.Interface.InterfaceAgrotoxicoUseCase;
using CasosDeUso.Interface.InterfaceAplicacaoUseCase;
using CasosDeUso.Interface.InterfaceCultivoUseCase;
using CasosDeUso.Interface.InterfacePragaUseCase;
using CasosDeUso.PluginsInterfaces;
using CasosDeUso.PragaCasoDeUso;
using CoreBusiness.Entidades;
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
            builder.Services.AddSingleton<IRepositorioDeAplicacao, RepositorioAplicacaoSqlLite>();
            builder.Services.AddSingleton<IRepositorioDeAgrotoxico, RepositorioAgrotoxicoSqlLite>();
            builder.Services.AddSingleton<IRepositorioDeCultivo, RepositorioCultivoSqlLite>();
            builder.Services.AddSingleton<IRepositorioDePraga, RepositorioPragaSqlLite>();

            //builder.Services.AddSingleton<IRepositorioDeAplicacao, Dados>();
            builder.Services.AddSingleton<IVisualizarAplicacaoUseCase, VisualizarAplicacaoUseCase>();
            builder.Services.AddSingleton<IApagarAplicacaoUseCase, ApagarAplicacaoUseCase>();
            builder.Services.AddSingleton<IAdicionarAplicacaoUseCase, AdicionarAplicacaoUseCase>();
            builder.Services.AddSingleton<IEditarAplicacaoUseCase, EditarAplicacaoUseCase>();
            builder.Services.AddTransient<IAdicionarAplicacaoUseCase, AdicionarAplicacaoUseCase>();
            builder.Services.AddTransient<IVisualizarCultivoUseCase, VisualizarCultivoUseCase>();
            builder.Services.AddTransient<IVisualizarPragaUseCase, VisualizarPragaUseCase>();
            builder.Services.AddTransient<IVisualizarAgrotoxicoUseCase, VisualizarAgrotoxicoUseCase>();
            builder.Services.AddTransient<AplicacaoControle>();
            #endregion
            builder.Services.AddSingleton<AplicacaoPagina>();
            builder.Services.AddSingleton<SelecaoItemCadastro>();
            builder.Services.AddSingleton<AplicacaoControle>();
            builder.Services.AddSingleton<AgrotoxicoControle>();

            //builder.Services.AddSingleton<EditarContatoPage>();
            //builder.Services.AddSingleton<AdicionarContatoPage>();

            var app = builder.Build();

            SeedDatabase(app.Services);

            return app;
        }

        private static async void SeedDatabase(IServiceProvider services)
        {
            var cultivoRepo = services.GetRequiredService<IRepositorioDeCultivo>();
            var pragaRepo = services.GetRequiredService<IRepositorioDePraga>();
            var agrotoxicoRepo = services.GetRequiredService<IRepositorioDeAgrotoxico>();

            // Exemplo: Cultivos
            var cultivos = await cultivoRepo.BuscarCultivo("");
            if (cultivos.Count == 0)
            {
                await cultivoRepo.AdicionarCultivoAsync(new Cultivo { Nome = "Soja", Id = Guid.NewGuid() });
                await cultivoRepo.AdicionarCultivoAsync(new Cultivo { Nome = "Milho", Id = Guid.NewGuid() });
            }

            // Exemplo: Pragas
            var pragas = await pragaRepo.BuscarPraga("");
            if (pragas.Count == 0)
            {
                await pragaRepo.AdicionarPragaAsync(new Praga { Nome = "Lagarta", Id = Guid.NewGuid() });
                await pragaRepo.AdicionarPragaAsync(new Praga { Nome = "Pulgão", Id = Guid.NewGuid() });
            }

            // Exemplo: Agrotóxicos
            var agrotoxicos = await agrotoxicoRepo.BuscarAgrotoxico("");
            if (agrotoxicos.Count == 0)
            {
                await agrotoxicoRepo.AdicionarAgrotoxico(new Agrotoxico { Nome = "Inseticida X", Lote = "XYYWH2509", Validade = DateTimeOffset.Now.AddMonths(2), Id = Guid.NewGuid() });
                await agrotoxicoRepo.AdicionarAgrotoxico(new Agrotoxico { Nome = "Fungicida Y", Lote = "POIII322", Validade = DateTimeOffset.Now.AddMonths(6), Id = Guid.NewGuid() });
            }
        }

    }
}
