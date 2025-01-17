using System.Text;

namespace Deniz.ExceptionParser
{
    public class ExceptionParser
    {
        private string? _fullText;

        public ExceptionParser(Exception exception)
        {
            Exception = exception;
            if (exception is AggregateException aggregateException
                && aggregateException.InnerExceptions.Any())
            {
                Message = aggregateException.InnerExceptions.First().Message;
            }
            else
            {
                Message = exception.Message;
            }
        }

        public Exception Exception { get; }

        public string Message { get; }

        public string FullText => _fullText ??= $"{Message}\r\n{GetFullText(Exception)}";

        public override string ToString()
        {
            return FullText;
        }

        private string GetFullText(Exception ex)
        {
            var textBuilder = new StringBuilder();
            textBuilder.AppendLine($"{ex.GetType().FullName}: {ex.Message}");
            //TODO: Handle EntityEception and HttpClient Exception here based upon additional ExceptionParser-Hendlers
            //if (ex is EntityException entityException)
            //{
            //    foreach (var detail in easyException.Details)
            //    {
            //        textBuilder.AppendLine(detail);
            //    }
            //}

            //TODO: use Ben.Demisifier here
            //textBuilder.AppendLine(this.getStackTrace == null ? ex.StackTrace : this.getStackTrace(ex));
            textBuilder.AppendLine(ex.StackTrace);
            if (ex.InnerException != null)
            {
                textBuilder.AppendLine("----------[InnerException]----------");
                textBuilder.AppendLine(GetFullText(ex.InnerException));
            }
            return textBuilder.ToString();
        }
    }
}
