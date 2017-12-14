using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Grid.Tests.TestContext.Models
{
    [Table("TableModelTest", Schema = "dbo")]
    public class TableModel : Grid.Interfaces.ILogModel,
                                Grid.Interfaces.IIdModel, 
                                Grid.Interfaces.IClearVirtualPropertiesModel, 
                                Grid.Interfaces.IDisplayName
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }

        public string Test { get; set; }
        public DateTime Date { get; set; }
        public DateTime? DateNull { get; set; }
        public int UserId { get; set; }
        public int? UserNullId { get; set; }
		public bool IsBool { get; set; }
        public bool? IsBoolNull { get; set; }
        public decimal Decimal { get; set; }
        public decimal? DecimalNull { get; set; }

        public int EmployeeId { get; set; }
        public int? EmployeeNullId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
        [ForeignKey("UserNullId")]
        public User UserNull { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
        [ForeignKey("EmployeeNullId")]
        public Employee EmployeeNull { get; set; }

        public string DisplayName { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string LastUpdatedBy { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        public void ClearVirtualProperties()
        {
            this.User = null;
            this.UserNull = null;
            this.Employee = null;
            this.EmployeeNull = null;
        }
    }
    public class ViewModel : TableModel
    {
        public ViewModel() { }
        public ViewModel(TableModel Companent)
        {
            if (Companent != null)
            {
                this.UserName = new string(Companent.User?.DisplayName?.ToArray());
                this.UserNullName = new string(Companent.UserNull?.DisplayName?.ToArray());
                this.EmployeeName = new string(Companent.Employee?.DisplayName?.ToArray());
                this.EmployeeNullName = new string(Companent.EmployeeNull?.DisplayName?.ToArray());
            }
        }

        public string UserName { get; set; }
        public string UserNullName { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeNullName { get; set; }
    }
    public class FindModel : TableModel, Grid.Interfaces.ILogFindModel
    {
        public DateTime? CreatedDateStart { get; set; }
        public DateTime? CreatedDateEnd { get; set; }

        public DateTime? LastUpdatedDateStart { get; set; }
        public DateTime? LastUpdatedDateEnd { get; set; }


        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }

        public DateTime? DateNullStart { get; set; }
        public DateTime? DateNullEnd { get; set; }
    }
}
