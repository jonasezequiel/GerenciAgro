using CasosDeUso.PluginsInterfaces;
using CoreBusiness.Entidades;
using SQLite;
using System.ComponentModel.DataAnnotations;

namespace SqlLite
{
    public class RepositorioAplicacaoSqlLite : IRepositorioDeAplicacao
    {
        private SQLiteAsyncConnection _database;

        public RepositorioAplicacaoSqlLite()
        {
            System.Diagnostics.Debug.WriteLine($"[SQLite] Database path: {Constantes._databasepath}");
            _database = new SQLiteAsyncConnection(Constantes._databasepath, Constantes.Flags);
            _database.CreateTableAsync<AplicacaoWrapper>().Wait();
            _database.CreateTableAsync<AgrotoxicoWrapper>().Wait();
            _database.CreateTableAsync<CultivoWrapper>().Wait();
            _database.CreateTableAsync<PragaWrapper>().Wait();
        }

        #region Repositorio Aplicacao SqlLite
        public Task AdicionarAplicacao(Aplicacao aplicacao)
        {
            return Task.FromResult(AdicionarAplicacaoAsync(aplicacao));
        }

        public async Task AdicionarAplicacaoAsync(Aplicacao aplicacao)
        {
            aplicacao.Id = Guid.NewGuid();
            await _database.InsertAsync(new AplicacaoWrapper(aplicacao));
        }

        public Task AtualizarAplicacao(Aplicacao aplicacao)
        {
            return Task.FromResult(AtualizarAplicacaoAsync(aplicacao));
        }

        public async Task AtualizarAplicacaoAsync(Aplicacao aplicacao)
        {
            var colunasAfetadas = await _database.UpdateAsync(new AplicacaoWrapper(aplicacao));
            if (colunasAfetadas <= 0)
                throw new InvalidOperationException("Erro ao atualizar Aplicacao");
        }

        public Task<Aplicacao> BuscarAplicacaoPorId(Guid id)
        {
            return BuscarAplicacaoPorIdAsync(id);
        }

        public async Task<Aplicacao> BuscarAplicacaoPorIdAsync(Guid id)
        {
            var idString = id.ToString("D");
            var wrapper = await _database.Table<AplicacaoWrapper>()
                .Where(x => x.Id == idString).FirstOrDefaultAsync();
            return wrapper?.ToAplicacao();
        }

        public Task<List<Aplicacao>> BuscarAplicacao(string filtro)
        {
            return BuscarAplicacaoAsync(filtro);
        }

        public async Task<List<Aplicacao>> BuscarAplicacaoAsync(string filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro))
            {
                var wrappers = await _database.Table<AplicacaoWrapper>().ToListAsync();
                return wrappers.Select(wrapper => wrapper.ToAplicacao()).ToList();
            }

            var queryResult = await this._database.QueryAsync<AplicacaoWrapper>
                (
                    @"
                    SELECT * FROM AplicacaoWrapper
                    WHERE 
                        Observacao LIKE ? OR 
                        Agrotoxico LIKE ? OR 
                        Cultivo LIKE ? OR 
                        DataAplicacao LIKE ?",
                    $"{filtro}%",
                    $"{filtro}%",
                    $"{filtro}%",
                    $"{filtro}%"
                );

            return queryResult.Select(wrapper => wrapper.ToAplicacao()).ToList();
        }

        public Task<List<Aplicacao>> BuscarTodasAplicacao()
        {
            return BuscarTodasAplicacaoAsync();
        }

        public async Task<List<Aplicacao>> BuscarTodasAplicacaoAsync()
        {
            return await _database.Table<Aplicacao>().ToListAsync();
        }

        public Task ExcluirAplicacao(Aplicacao aplicacao)
        {
            return Task.FromResult(ExcluirAplicacaoAsync(aplicacao));
        }

        public async Task ExcluirAplicacaoAsync(Aplicacao aplicacao)
        {
            var aplicacaoExcluir = await BuscarAplicacaoPorIdAsync(aplicacao.Id);
            if (aplicacaoExcluir != null && aplicacao.Id.Equals(aplicacaoExcluir.Id))
                await _database.DeleteAsync(new AplicacaoWrapper(aplicacaoExcluir));
        }
        #endregion

        #region Repositorio Agrotoxico SqlLite
        public Task AdicionarAgrotoxico(Agrotoxico agrotoxico)
        {
            return Task.FromResult(AdicionarAgrotoxicoAsync(agrotoxico));
        }

        public async Task AdicionarAgrotoxicoAsync(Agrotoxico agrotoxico)
        {
            await _database.InsertAsync(new AgrotoxicoWrapper(agrotoxico));
        }

        public Task AtualizarAgrotoxico(Agrotoxico agrotoxico)
        {
            return Task.FromResult(AtualizarAgrotoxicoAsync(agrotoxico));
        }

        public async Task AtualizarAgrotoxicoAsync(Agrotoxico agrotoxico)
        {
            var colunasAfetadas = await _database.UpdateAsync(new AgrotoxicoWrapper(agrotoxico));
            if (colunasAfetadas <= 0)
                throw new InvalidOperationException("Erro ao atualizar Agrotoxico");
        }

        public Task<List<Agrotoxico>> BuscarAgrotoxico(string filtro)
        {
            return BuscarAgrotoxicoAsync(filtro);
        }

        public async Task<List<Agrotoxico>> BuscarAgrotoxicoAsync(string filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro))
            {
                var wrappers = await _database.Table<AgrotoxicoWrapper>().ToListAsync();
                return wrappers.Select(wrapper => wrapper.ToAgrotoxico()).ToList();
            }

            var queryResult = await _database.QueryAsync<AgrotoxicoWrapper>
                (
                    @"
                            SELECT * FROM AgrotoxicoWrapper
                            WHERE 
                                Nome LIKE ? OR 
                                Lote LIKE ? OR 
                                Validade LIKE ?",
                    $"{filtro}%",
                    $"{filtro}%",
                    $"{filtro}%"
                );

            return queryResult.Select(wrapper => wrapper.ToAgrotoxico()).ToList();
        }

        public Task<Agrotoxico> BuscarAgrotoxicoPorId(Guid id)
        {
            return BuscarAgrotoxicoPorIdAsync(id);
        }

        public async Task<Agrotoxico> BuscarAgrotoxicoPorIdAsync(Guid id)
        {
            var idString = id.ToString("D");
            var wrapper = await _database.Table<AgrotoxicoWrapper>()
                .Where(x => x.Id == idString).FirstOrDefaultAsync();
            return wrapper?.ToAgrotoxico();
        }

        public async Task<List<Agrotoxico>> BuscarTodosAgrotoxicos()
        {
            var wrappers = await _database.Table<AgrotoxicoWrapper>().ToListAsync();
            return wrappers.Select(wrapper => wrapper.ToAgrotoxico()).ToList();
        }

        public Task ExcluirAgrotoxico(Agrotoxico agrotoxico)
        {
            return Task.FromResult(ExcluirAgrotoxicoAsync(agrotoxico));
        }

        public async Task ExcluirAgrotoxicoAsync(Agrotoxico agrotoxico)
        {
            var agrotoxicoExcluir = await BuscarAgrotoxicoPorIdAsync(agrotoxico.Id);

            if (agrotoxicoExcluir != null && agrotoxico.Id.Equals(agrotoxicoExcluir.Id))
                await _database.DeleteAsync(AdicionarAgrotoxicoAsync(agrotoxicoExcluir));
        }
        #endregion

        #region Repositorio Cultivo SqlLite
        public Task AdicionarCultivo(Cultivo cultivo)
        {
            return Task.FromResult(AdicionarCultivoAsync(cultivo));
        }

        public async Task AdicionarCultivoAsync(Cultivo cultivo)
        {
            await _database.InsertAsync(new CultivoWrapper(cultivo));
        }

        public Task AtualizarCultivo(Cultivo cultivo)
        {
            return Task.FromResult(AtualizarCultivoAsync(cultivo));
        }

        public async Task AtualizarCultivoAsync(Cultivo cultivo)
        {
            var colunasAfetadas = await _database.UpdateAsync(new CultivoWrapper(cultivo));
            if (colunasAfetadas <= 0)
                throw new InvalidOperationException("Erro ao atualizar cultivo");
        }

        public Task<List<Cultivo>> BuscarCultivo(string filtro)
        {
            return BuscarCultivoAsync(filtro);
        }

        public async Task<List<Cultivo>> BuscarCultivoAsync(string filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro))
            {
                var wrappers = await _database.Table<CultivoWrapper>().ToListAsync();
                return wrappers.Select(wrapper => wrapper.ToCultivo()).ToList();
            }

            var queryResult = await _database.QueryAsync<CultivoWrapper>
                (
                    @"
                            SELECT * FROM CultivoWrapper
                            WHERE 
                                Nome LIKE ?",
                    $"{filtro}%"
                );

            return queryResult.Select(wrapper => wrapper.ToCultivo()).ToList();
        }

        public Task<Cultivo> BuscarCultivoPorId(Guid id)
        {
            return BuscarCultivoPorIdAsync(id);
        }

        public async Task<Cultivo> BuscarCultivoPorIdAsync(Guid id)
        {
            var idString = id.ToString("D");
            var wrapper = await _database.Table<CultivoWrapper>()
                .Where(x => x.Id == idString).FirstOrDefaultAsync();

            return wrapper?.ToCultivo();
        }

        public async Task<List<Cultivo>> BuscarTodosCultivos()
        {
            var wrappers = await _database.Table<CultivoWrapper>().ToListAsync();
            return wrappers.Select(wrapper => wrapper.ToCultivo()).ToList();
        }

        public Task ExcluirCultivo(Cultivo cultivo)
        {
            return Task.FromResult(ExcluirCultivoAsync(cultivo));
        }

        public async Task ExcluirCultivoAsync(Cultivo cultivo)
        {
            var cultivoExcluir = await BuscarCultivoPorIdAsync(cultivo.Id);

            if (cultivoExcluir != null && cultivo.Id.Equals(cultivoExcluir.Id))
                await _database.DeleteAsync(new CultivoWrapper(cultivoExcluir));
        }
        #endregion

        #region Repositorio Praga SqlLite
        public Task AdicionarPraga(Praga praga)
        {
            return Task.FromResult(AdicionarPragaAsync(praga));
        }

        public async Task AdicionarPragaAsync(Praga praga)
        {
            await _database.InsertAsync(new PragaWrapper(praga));
        }

        public Task AtualizarPraga(Praga praga)
        {
            return Task.FromResult(AtualizarPragaAsync(praga));
        }

        public async Task AtualizarPragaAsync(Praga praga)
        {
            var colunasAfetadas = await _database.UpdateAsync(new PragaWrapper(praga));
            if (colunasAfetadas <= 0)
                throw new InvalidOperationException("Erro ao atualizar praga");
        }

        public Task<List<Praga>> BuscarPraga(string filtro)
        {
            return BuscarPragaAsync(filtro);
        }

        public async Task<List<Praga>> BuscarPragaAsync(string filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro))
            {
                var wrappers = await _database.Table<PragaWrapper>().ToListAsync();
                return wrappers.Select(wrapper => wrapper.ToPraga()).ToList();
            }

            var queryResult = await _database.QueryAsync<PragaWrapper>
                (
                    @"
                            SELECT * FROM PragaWrapper
                            WHERE 
                                Nome LIKE ? OR ",
                    $"{filtro}%"
                );

            return queryResult.Select(wrapper => wrapper.ToPraga()).ToList();
        }

        public Task<Praga> BuscarPragaPorId(Guid id)
        {
            return BuscarPragaPorIdAsync(id);
        }

        public async Task<Praga> BuscarPragaPorIdAsync(Guid id)
        {
            var idString = id.ToString("D");
            var wrapper = await _database.Table<PragaWrapper>()
                .Where(x => x.Id == idString).FirstOrDefaultAsync();
            return wrapper?.ToPraga();
        }

        public async Task<List<Praga>> BuscarTodasPragas()
        {
            var wrappers = await _database.Table<PragaWrapper>().ToListAsync();
            return wrappers.Select(wrapper => wrapper.ToPraga()).ToList();
        }

        public Task ExcluirPraga(Praga praga)
        {
            return Task.FromResult(ExcluirPragaAsync(praga));
        }

        public async Task ExcluirPragaAsync(Praga praga)
        {
            var pragaExcluir = await BuscarPragaPorIdAsync(praga.Id);

            if (pragaExcluir != null && praga.Id.Equals(pragaExcluir.Id))
                await _database.DeleteAsync((new PragaWrapper(pragaExcluir)));
        }
        #endregion
    }

    public class AplicacaoWrapper
    {
        [PrimaryKey]
        public string Id { get; set; }
        [Required]
        public string AgrotoxicoId { get; set; }
        [Required]
        public string CultivoId { get; set; }
        public string DataAplicacao { get; set; }
        [Required]
        public string PragasAlvos { get; set; }
        public string? Observacao { get; set; }

        public AplicacaoWrapper() { }

        public AplicacaoWrapper(Aplicacao aplicacao)
        {
            Id = aplicacao.Id.ToString();
            AgrotoxicoId = aplicacao.AgrotoxicoId.ToString();
            CultivoId = aplicacao.CultivoId.ToString();
            DataAplicacao = aplicacao.DataAplicacao.ToString();
            PragasAlvos = string.Join(",", aplicacao.PragasAlvos.Select(g => g.ToString()));
            Observacao = aplicacao.Observacao;
        }


        public Aplicacao ToAplicacao()
        {
            return new Aplicacao
            {
                Id = Guid.TryParse(Id, out var id) ? id : Guid.Empty,
                AgrotoxicoId = Guid.TryParse(AgrotoxicoId, out var agrotoxicoId) ? agrotoxicoId : Guid.Empty,
                CultivoId = Guid.TryParse(CultivoId, out var cultivoId) ? cultivoId : Guid.Empty,
                DataAplicacao = DateTime.TryParse(DataAplicacao, out var dataAplicacao) ? dataAplicacao : default,
                PragasAlvos = PragasAlvos.Split(',')
                                          .Select(guidString => Guid.TryParse(guidString, out var guid) ? guid : Guid.Empty)
                                          .Where(guid => guid != Guid.Empty),
                Observacao = Observacao
            };
        }
    }

    public class AgrotoxicoWrapper
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Lote { get; set; }
        public string Validade { get; set; }

        public AgrotoxicoWrapper() { }

        public AgrotoxicoWrapper(Agrotoxico agrotoxico)
        {
            Id = agrotoxico.Id.ToString();
            Nome = agrotoxico.Nome;
            Lote = agrotoxico.Lote;
            Validade = agrotoxico.Validade.ToString();
        }

        public Agrotoxico ToAgrotoxico()
        {
            return new Agrotoxico
            {
                Id = Guid.TryParse(Id, out var guid) ? guid : Guid.Empty,
                Nome = Nome,
                Lote = Lote,
                Validade = DateTime.TryParse(Validade, out var validade) ? validade : default
            };
        }
    }

    public class CultivoWrapper
    {
        public string Id { get; set; }
        public string Nome { get; set; }

        public CultivoWrapper() { }

        public CultivoWrapper(Cultivo cultivo)
        {
            Id = cultivo.Id.ToString();
            Nome = cultivo.Nome;
        }

        public Cultivo ToCultivo()
        {
            return new Cultivo
            {
                Id = Guid.TryParse(Id, out var guid) ? guid : Guid.Empty,
                Nome = Nome
            };
        }
    }

    public class PragaWrapper
    {
        public string Id { get; set; }
        public string Nome { get; set; }

        public PragaWrapper() { }

        public PragaWrapper(Praga praga)
        {
            Id = praga.Id.ToString();
            Nome = praga.Nome;
        }

        public Praga ToPraga()
        {
            return new Praga
            {
                Id = Guid.TryParse(Id, out var guid) ? guid : Guid.Empty,
                Nome = Nome,
            };
        }
    }
}
