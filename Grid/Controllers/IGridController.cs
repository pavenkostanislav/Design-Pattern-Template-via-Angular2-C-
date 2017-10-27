namespace Grid.Managers
{
    public interface IGridController<T>
    {
        [Microsoft.AspNetCore.Mvc.HttpGet("list/{keyId:int?}")]
        System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetGridListAsync(int? keyId);

        [Microsoft.AspNetCore.Mvc.HttpGet("{id:int}/{mode?}")]
        System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetGridRowModelAsync(int id, Grid.Enums.Cmd? mode);
        
        [Microsoft.AspNetCore.Mvc.HttpPost()]
        System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> SaveGridRowModelAsync([Microsoft.AspNetCore.Mvc.FromBody] T model);

        [Microsoft.AspNetCore.Mvc.HttpDelete("{id:int}")]
        System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> DeleteGridRowModelAsync(int id);

        [Microsoft.AspNetCore.Mvc.HttpGet("select/{parentId:int?}/{term?}")]
        System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> SelectGridFindAsync(int? parentId, string term);

        [Microsoft.AspNetCore.Mvc.HttpGet("select/{id:int}")]
        System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetSelectGridRowModelAsync(int id);
        
    }
}

