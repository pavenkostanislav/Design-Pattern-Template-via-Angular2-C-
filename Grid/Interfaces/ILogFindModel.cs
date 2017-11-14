using System;

namespace Grid.Interfaces
{
    public interface ILogFindModel
    {
        string CreatedBy { get; set; }
        string LastUpdatedBy { get; set; }

        DateTime? CreatedDateStart { get; set; }
        DateTime? CreatedDateEnd { get; set; }
        DateTime? LastUpdatedDateStart { get; set; }
        DateTime? LastUpdatedDateEnd { get; set; }
    }
}