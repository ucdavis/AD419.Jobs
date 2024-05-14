
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

/// <summary>
/// Extension of <seealso cref="CsvReader"/> that ensures all columns are mapped
/// </summary>
public class FullMapCsvReader : CsvReader {

   public FullMapCsvReader(IParser parser) : base(parser) {
   }

   public FullMapCsvReader(TextReader reader, CsvConfiguration configuration) : base(reader, configuration) {
   }

   public FullMapCsvReader(TextReader reader, CultureInfo culture, bool leaveOpen = false) : base(reader, culture, leaveOpen) {
   }

   protected override void ValidateHeader(ClassMap map, List<InvalidHeader> invalidHeaders) {

      base.ValidateHeader(map, invalidHeaders);

      // We'll only run our validation if the base validation did not find any problems (otherwise we would need to throw
      // a single exception signalling both kinds of problems, which is hard to implement in a subclass)
      if (!invalidHeaders.Any()) {
         var unexpectedHeaders = new List<string>();
         for (var i = 0; i < HeaderRecord?.Length; i++) {
            var header = HeaderRecord[i];
            if (!isHeaderMapped(map, header, i)) {
               unexpectedHeaders.Add(header);
            }
         }
         if (unexpectedHeaders.Any()) {
            // Adding headers to `invalidHeaders` causes a HeaderValidationException to be thrown later with a message that
            // implies that expected headers were not found, which is not the case (it's actually the other way around). Thus,
            // we throw a custom exception instead. Note that this will "escape" any custom handling set on `Configuration.HeaderValidated`
            throw new UnexpectedHeadersException(Context, unexpectedHeaders);
         }
      }
   }

   private bool isHeaderMapped(ClassMap map, string header, int index) {
      var headerName = Configuration.PrepareHeaderForMatch(new PrepareHeaderForMatchArgs(header, index));

      foreach (var parameter in map.ParameterMaps) {
         foreach (var name in parameter.Data.Names) {
            if (Configuration.PrepareHeaderForMatch(new PrepareHeaderForMatchArgs(name, index)) == headerName) {
               return true;
            }
         }
      }

      foreach (var memberMap in map.MemberMaps) {
         foreach (var name in memberMap.Data.Names) {
            if (Configuration.PrepareHeaderForMatch(new PrepareHeaderForMatchArgs(name, index)) == headerName) {
               return true;
            }
         }
      }

      // Not sure whether we should iterate `map.ReferenceMaps`

      return false;
   }
}

public class UnexpectedHeadersException : ValidationException {

   public List<string> UnexpectedHeaders { get; }

   public UnexpectedHeadersException(CsvContext context, List<string> unexpectedHeaders) : base(context, $"Unexpected headers: {string.Join(", ", unexpectedHeaders.Select(h => $"'{h}'"))}" ) {
      this.UnexpectedHeaders = unexpectedHeaders;
   }
}