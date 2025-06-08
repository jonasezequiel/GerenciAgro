using CasosDeUso.PluginsInterfaces;
using CoreBusiness.Entidades;
using SQLite;

namespace SqlLite
{
    public class RepositorioPragaSqlLite : IRepositorioDePraga
    {
        private SQLiteAsyncConnection _database;

        public RepositorioPragaSqlLite()
        {
            _database = new SQLiteAsyncConnection(Constantes._databasepath);
            _database.CreateTableAsync<PragaWrapper>().Wait();
        }

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
