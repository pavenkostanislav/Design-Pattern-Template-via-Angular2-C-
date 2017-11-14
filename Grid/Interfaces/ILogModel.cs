using System;

namespace Grid.Interfaces
{
    internal interface ILogModel
    {
        string CreatedBy { get; set; }
        DateTime CreatedDate { get; set; }
        string LastUpdatedBy { get; set; }
        DateTime LastUpdatedDate { get; set; }
    }
}