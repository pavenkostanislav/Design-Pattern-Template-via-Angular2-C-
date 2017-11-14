namespace Grid.Models
{
    public class ResponseModel<T>
    {
        public int TotalRowCount { get; set; }
        public int CurrentPage { get; set; }
        public System.Collections.Generic.IList<T> List { get; set; }
    }
}
