using Microsoft.AspNetCore.Mvc;

namespace Grid.Controllers
{
    public interface IGridController<GridTableModel, GridViewModel, GridFindModel>
    {
        [Microsoft.AspNetCore.Mvc.HttpPost("list")]
        System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetGridRequestModelAsync([FromBody] Grid.Models.RequestModel<GridFindModel> requestModel);

        [Microsoft.AspNetCore.Mvc.HttpGet("list/{keyId:int?}")]
        System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetGridListAsync(int? keyId);

        [Microsoft.AspNetCore.Mvc.HttpGet("{id:int}")]
        System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetGridModelAsync(int id);

        [Microsoft.AspNetCore.Mvc.HttpGet("{id:int}/new")]
        Microsoft.AspNetCore.Mvc.IActionResult GetGridNewModel(int id);

        [Microsoft.AspNetCore.Mvc.HttpGet("{id:int}/copy")]
        System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetGridCopyModelAsync(int id);

        [Microsoft.AspNetCore.Mvc.HttpGet("{id:int}/find")]
        Microsoft.AspNetCore.Mvc.IActionResult GetGridFindModel(int id);

        [Microsoft.AspNetCore.Mvc.HttpPost()]
        System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> SaveGridRowModelAsync([FromBody] GridTableModel model);
        [Microsoft.AspNetCore.Mvc.HttpPost("savelist")]
        System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> SaveGridListAsync([FromBody] System.Collections.Generic.IList<GridTableModel> listmodel);

        [HttpDelete("{id:int}")]
        System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> DeleteGridRowModelAsync(int id);

        #region Select

        [HttpGet("select/{parentId:int?}/{term?}")]
        System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> SelectGridFindAsync(int? parentId, string term);

        [HttpGet("select/{id:int}")]
        System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetSelectGridRowModelAsync(int id);

        #endregion
    }
}

