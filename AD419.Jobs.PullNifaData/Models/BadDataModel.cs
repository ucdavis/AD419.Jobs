using AD419.Jobs.PullNifaData.Attributes;
using CsvHelper;

namespace AD419.Jobs.PullNifaData.Models;

public class BadDataModel
{
    [DbColumnOrder]
    public string ErrorMessage { get; set; } = "";
    [DbColumnOrder]
    public string FileName { get; set; } = "";
    [DbColumnOrder]
    public DateTime ProcessedDate { get; set; }
    [DbColumnOrder]
    public int ColumnIndex { get; set; }
    [DbColumnOrder]
    public int RowIndex { get; set; }
    [DbColumnOrder]
    public string ColumnName { get; set; } = "";
    [DbColumnOrder]
    public string Field { get; set; } = "";
    [DbColumnOrder]
    public string RawRecord { get; set; } = "";


    public static BadDataModel From(string fileName, DateTime processedDate, BadDataFoundArgs args)
    {
        var context = args.Context;
        return new BadDataModel
        {
            ErrorMessage = "Bad Data",
            FileName = fileName,
            ProcessedDate = processedDate,
            Field = args.Field,
            RawRecord = args.RawRecord,
            ColumnIndex = context.Reader.CurrentIndex,
            ColumnName = context.Reader.HeaderRecord?[context.Reader.CurrentIndex] ?? "",
            RowIndex = context.Parser.RawRow,
        };
    }

    public static BadDataModel From(string fileName, DateTime processedDate, ReadingExceptionOccurredArgs args)
    {
        var context = args.Exception.Context;
        return new BadDataModel
        {
            ErrorMessage = args.Exception.ToString(),
            FileName = fileName,
            ProcessedDate = processedDate,
            Field = context.Reader.GetField(context.Reader.CurrentIndex) ?? "",
            RawRecord = context.Parser.RawRecord,
            ColumnIndex = context.Reader.CurrentIndex,
            ColumnName = context.Reader.HeaderRecord?[context.Reader.CurrentIndex] ?? "",
            RowIndex = context.Parser.RawRow,
        };
    }
}
