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
            _database = new SQLiteAsyncConnection(Constantes._databasepath);
            _database.CreateTableAsync<AplicacaoWrapper>().Wait();
        }

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
                await _database.DeleteAsync(aplicacaoExcluir);
        }
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
}
