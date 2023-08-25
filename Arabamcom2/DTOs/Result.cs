using FluentValidation.Results;

namespace Arabamcom2.DTOs
{
    public class Result<T>
    {
        public int StatusCode { get; set; }
        public T Data { get; set; }
        public ValidationResult Error { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }

    public class ResultList<T>
    {
        public int StatusCode { get; set; }
        public IEnumerable<T> Data { get; set; }
        public ValidationResult Error { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }

}
