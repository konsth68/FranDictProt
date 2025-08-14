using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;


namespace FranDictProt;

public static class Connection
{
    public static IDbConnection? GetConnection()
    {
        //string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        //string path = appData + "\\FranDict\\FranDict.db";
        //string path = ".\\FranDict.db";
        string path = "/var/www/franch/FranDict.db";
        string conn = $"Data Source={path}; Mode=ReadOnly";

        if (conn != string.Empty)
        {
            return new SqliteConnection(conn);
        }
        else
        {
            return null;
        }
    }
}

public interface IWordKeyRepository
{
    public List<WordKey> GetAll();

    public WordKey GetId(long id);

    public List<WordKey> GetFilter(string filter);

    public List<WordKey> GetWord(string word);

}

public class WordKeyRepository : IWordKeyRepository
{
    public  List<WordKey> GetAll()
    {
        using var db = Connection.GetConnection();
        if (db == null) return [];

        var sql = "SELECT WordId,Word FROM WordKey";

        var res = db.Query<WordKey>(sql).ToList();

        return res;
    }        

    public  WordKey GetId(long id)
    {
        using var db = Connection.GetConnection();
        if (db == null) return new WordKey();

        var sql = $"SELECT WordId,Word FROM WordKey WHERE WordId = {id}";

        var res = db.QuerySingleOrDefault<WordKey>(sql) ?? new WordKey();

        return res;
    }

    public  List<WordKey> GetFilter(string filter)
    {
        using var db = Connection.GetConnection();
        if (db == null) return [];

        var sql = $"SELECT WordId,Word FROM WordKey WHERE {filter}";

        var res = db.Query<WordKey>(sql).ToList();

        return res;
    }

    public  List<WordKey> GetWord(string word)
    {
        using var db = Connection.GetConnection();
        if (db == null) return [];

        var sql = $"SELECT WordId,Word FROM WordKey WHERE Word like \'%{word}%\'";

        var res = db.Query<WordKey>(sql).ToList();

        return res;
    }

}

public interface IParagraphRepository
{
    public List<Paragraph> GetAll();

    public Paragraph GetId(long id);

    public List<Paragraph> GetFilter(string filter);

    public List<Paragraph> GetWordId(long wordId);
    
    public List<Paragraph> GetWordId(long[] wordId);
}    

public class ParagraphRepository : IParagraphRepository
{

    private   string _allField = "ParagraphId,WordKeyId,Word,IndexPar,ParagraphStr,ReplaceStr";

    public   List<Paragraph> GetAll()
    {
        using var db = Connection.GetConnection();
        if (db == null) return [];

        var sql = $"SELECT {_allField} FROM Paragraph";

        var res = db.Query<Paragraph>(sql).ToList();

        return res;
    }

    public   Paragraph GetId(long id)
    {
        using var db = Connection.GetConnection();
        if (db == null) return new Paragraph();

        var sql = $"SELECT {_allField} FROM Paragraph WHERE ParagraphId = {id}";

        var res = db.QuerySingleOrDefault<Paragraph>(sql) ?? new Paragraph();

        return res;
    }

    public   List<Paragraph> GetFilter(string filter)
    {
        using var db = Connection.GetConnection();
        if (db == null) return [];

        var sql = $"SELECT {_allField} FROM Paragraph WHERE {filter}";

        var res = db.Query<Paragraph>(sql).ToList();

        return res;
    }

    public   List<Paragraph> GetWordId(long wordId)
    {
        using var db = Connection.GetConnection();
        if (db == null) return [];

        var sql = $"SELECT {_allField} FROM Paragraph WHERE WordKeyId = {wordId}";

        var res = db.Query<Paragraph>(sql).ToList();

        return res;
    }

    public   List<Paragraph> GetWordId(long[] wordId)
    {
        using var db = Connection.GetConnection();
        if (db == null) return [];

        var inStr = string.Join(",", wordId);
        
        var sql = $"SELECT {_allField} FROM Paragraph WHERE WordKeyId IN ( {inStr} )";

        var res = db.Query<Paragraph>(sql).ToList();

        return res;
    }
    
}

