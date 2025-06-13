using CasosDeUso.PluginsInterfaces;
using CoreBusiness.Entidades;
using SQLite;

namespace SqlLite
{
    public class RepositorioCultivoSqlLite : IRepositorioDeCultivo
    {
        private SQLiteAsyncConnection _database;

        public RepositorioCultivoSqlLite()
        {
            _database = new SQLiteAsyncConnection(Constantes._databasepath);
            _database.CreateTableAsync<CultivoWrapper>().Wait();
        }

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
}
