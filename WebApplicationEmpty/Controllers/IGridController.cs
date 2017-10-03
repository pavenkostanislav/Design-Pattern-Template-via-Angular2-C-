
namespace KPMA.Managers
{
    public interface IGridController<T>
    {
        Microsoft.AspNetCore.Mvc.IActionResult GetGridList(int? keyId);
        Microsoft.AspNetCore.Mvc.IActionResult GetGridRowModel(int id, Enums.ModeEnum? mode);
        Microsoft.AspNetCore.Mvc.IActionResult SaveGridRowModel([Microsoft.AspNetCore.Mvc.FromBody] T model);
        Microsoft.AspNetCore.Mvc.IActionResult DeleteGridRowModel(int id);
    }
}

