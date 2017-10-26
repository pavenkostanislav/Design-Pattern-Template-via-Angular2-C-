namespace KPMA.Models
{
    public class RequestModel<GridFindModel>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int? KeyId { get; set; }
        public GridFindModel FindModel { get; set; }
    }
}
