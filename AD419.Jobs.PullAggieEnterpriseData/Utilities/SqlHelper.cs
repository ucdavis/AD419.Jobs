using System.Data;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace AD419.Jobs.Utilities;

public static class SqlHelper
{
    public static IEnumerable<string> GetBatches(TextReader reader)
    {
        // May be overkill for what amounts to a regex split on GO, but it's safer and doesn't add much complexity
        var parser = new TSql150Parser(initialQuotedIdentifiers: false);

        var fragment = parser.Parse(reader, out var errors);
        if (errors != null && errors.Count > 0)
        {
            throw new Exception("Unable to parse sql script:" + Environment.NewLine +
                string.Join(Environment.NewLine, errors.Select(e => $"Line: {e.Line}, Column: {e.Column}: {e.Message}")));
        }

        var generator = new Sql150ScriptGenerator();
        var sqlScript = fragment as TSqlScript;
        if (sqlScript == null)
        {
            generator.GenerateScript(fragment, out var sqlString);
            yield return sqlString;
        }
        else
        {
            foreach (var sqlBatch in sqlScript.Batches)
            {
                generator.GenerateScript(sqlBatch, out var sqlString);
                yield return sqlString;
            }
        }
    }

    public static IEnumerable<string> GetBatchesFromFile(string fileName)
    {
        using TextReader textReader = File.OpenText(fileName);
        foreach (var batch in GetBatches(textReader))
        {
            yield return batch;
        }
    }

    public static IEnumerable<string> GetBatchesFromString(string script)
    {
        using TextReader textReader = new StringReader(script);
        foreach (var batch in GetBatches(textReader))
        {
            yield return batch;
        }
    }
}