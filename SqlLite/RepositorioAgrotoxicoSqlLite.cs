using CasosDeUso.PluginsInterfaces;
using CoreBusiness.Entidades;
using Microsoft.Maui.Graphics.Text;
using SQLite;

namespace SqlLite
{
    public class RepositorioAgrotoxicoSqlLite : IRepositorioDeAgrotoxico
    {
        private SQLiteAsyncConnection _database;

        public RepositorioAgrotoxicoSqlLite()
        {
            _database = new SQLiteAsyncConnection(Constantes._databasepath);
            _database.CreateTableAsync<AgrotoxicoWrapper>().Wait();
        }

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
}