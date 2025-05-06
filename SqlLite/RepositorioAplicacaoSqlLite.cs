using CasosDeUso.PluginsInterfaces;
using CoreBusiness.Entidades;
using SQLite;

namespace SqlLite
{
    public class RepositorioAplicacaoSqlLite : IRepositorioDeAplicacao
    {
        private SQLiteAsyncConnection _database;

        public RepositorioAplicacaoSqlLite()
        {
            _database = new SQLiteAsyncConnection(Constantes._databasepath);
            _database.CreateTableAsync<Aplicacao>().Wait();
        }

        public Task AdicionarAplicacao(Aplicacao aplicacao)
        {
            return Task.FromResult(AdicionarAplicacaoAsync(aplicacao));
        }

        public async Task AdicionarAplicacaoAsync(Aplicacao aplicacao)
        {
            await _database.InsertAsync(aplicacao);
        }

        public Task AtualizarAplicacao(Aplicacao aplicacao)
        {
            return Task.FromResult(AtualizarAplicacaoAsync(aplicacao));
        }

        public async Task AtualizarAplicacaoAsync(Aplicacao aplicacao)
        {
            var colunasAfetadas = await _database.UpdateAsync(aplicacao);
            if (colunasAfetadas <= 0)
                throw new InvalidOperationException("Erro ao atualizar contato");
        }

        public Task<Aplicacao> BuscarAplicacaoPorId(Guid id)
        {
            return BuscarAplicacaoPorIdAsync(id);
        }

        public async Task<Aplicacao> BuscarAplicacaoPorIdAsync(Guid id)
        {
            return await _database.Table<Aplicacao>()
                .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public Task<List<Aplicacao>> BuscarAplicacao(string filtro)
        {
            return BuscarAplicacaoAsync(filtro);
        }

        public async Task<List<Aplicacao>> BuscarAplicacaoAsync(string filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro))
                return await _database.Table<Aplicacao>().ToListAsync();

            return await this._database.QueryAsync<Aplicacao>
                (
                    @"
                    SELECT * FROM Aplicacao
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
}
