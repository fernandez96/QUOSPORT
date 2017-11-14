namespace Base.Common
{
    public class PaginationParameter<T>
    {
        public string OrderBy { get; set; }
        public T Start { get; set; }
        public T CurrentPage { get; set; }
        public T AmountRows { get; set; }
        public string WhereFilter { get; set; }
        public string WhereFilterI { get; set; }

        public string WhereFilterS { get; set; }

        public string WhereFilterP { get; set; }

    }
}
