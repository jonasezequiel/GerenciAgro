namespace SqlLite
{
    // All the code in this file is included in all platforms.
    public class Constantes
    {
        public const string _databasefilename = "AplicacaoSqlLite.db3";
        public const SQLite.SQLiteOpenFlags Flags =
        // open the database in read/write mode
        SQLite.SQLiteOpenFlags.ReadWrite |
        // create the database if it doesn't exist
        SQLite.SQLiteOpenFlags.Create |
        // enable multi-threaded database access
        SQLite.SQLiteOpenFlags.SharedCache;
        public static string _databasepath => Path.Combine(FileSystem.Current.AppDataDirectory, _databasefilename);
    }
}
