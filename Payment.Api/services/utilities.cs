namespace Payment.Api.Utilities
{
    public class Response200
    {
        public string Title { get; set; } = "";
        public readonly int Status = 200;
        public object Data { get; set; }

    }

    public class Response201
    {
        public string Title { get; set; } = "";
        public readonly int Status = 201;
        public object Data { get; set; }

    }
    public class Response404
    {
        public string Title { get; set; } = "";
        public readonly int Status = 404;

    }
}