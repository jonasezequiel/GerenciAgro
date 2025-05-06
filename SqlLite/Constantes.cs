namespace SqlLite
{
    // All the code in this file is included in all platforms.
    public class Constantes
    {
        public const string _databasefilename = "AplicacaoSqlLite.db3";
        public static string _databasepath => Path.Combine(FileSystem.AppDataDirectory, _databasefilename);
    }
}
