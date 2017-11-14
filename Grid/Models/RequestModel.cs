namespace Grid.Models
{
    public class RequestModel<GridFindModel>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int? KeyId { get; set; }
        public System.Collections.Generic.List<string> OrderNamesList { get; set; }
        public GridFindModel FindModel { get; set; }
    }
}
