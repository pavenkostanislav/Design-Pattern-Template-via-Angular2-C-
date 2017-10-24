# Типовой подход по разработке стандартного решения для сайтов

Цели и задачи к реализации типового подхода
UI
Отображение списка учётом связанности данных
Разделение на страницы
Экономия времени загрузки
Добавление новых данных учитывая обязательность заполнения
Редактирование
Удаление
Быстрее получать новые модули

back-end

Project ASP.NET Core 2.0
Front-end(UI): Angular2+ 
Grid.Control/Grid.Service
Back-end: WebApiController
TableModel/ViewModel/FindModel
Data.Interfaces.IIdModel
Data.Interfaces.IDisplayName
Data.Interfaces.IClearVirtualMethodsModelViewModel
IGridManager/GridManager (generit, async)
IGridController/GridController (generit, async)
Db MS SQL
dbo.Table
Возможность расширения

# Как использовать

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KPMA.Data.Models
{
    [Table("TableEx", Schema = "dbo")]
    public class TableEx :	KPMA.Data.Interfaces.IIdModel, 
							KPMA.Data.Interfaces.IClearVirtualMethodsModel, 
							KPMA.Data.Interfaces.IDisplayName
    {
        /// <summary>
        ///  Первичный ключ
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        ///  Текстовое описание
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        ///  Язык
        /// </summary>
        public int LanguageId { get; set; }
        /// <summary>
        ///  Дата
        /// </summary>
        public DateTime Date { get; set; }
        [ForeignKey("LanguageId")]
        public Language Language { get; set; }
        public void ClearVirtualMethods()
        {
            this.Language = null;
        }
    }
    public class TableExViewModel : TableEx
    {
        /// <summary>
        ///  Наименование языка из коллекции
        /// </summary>
        public string LanguageName
        {
            get
            {
                return this.Language?.DisplayName;
            }
        }
    }
    public class TableExFindModel : TableExViewModel
    {
        /// <summary>
        ///  Начало ограничения периода свойство "Дата"
        /// </summary>
        public DateTime DateStart { get; set; }
        /// <summary>
        ///  Окончание ограничения периода свойство "Дата"
        /// </summary>
        public DateTime DateEnd { get; set; }
    }
}
namespace KPMA.Managers
{
    public class TableExManager : GridManager<Data.Models.TableEx, Data.Models.TableExFindModel>
    {
        public TableExManager(Data.CoreDbContext db) : base(db)
        {
        }
        override public IQueryable<Data.Models.TableEx> GetGridList(int? keyId)
        {
            if (!keyId.HasValue) { return null; }

            var list = db.Set<Data.Models.TableEx>()
                .Include(m => m.Language)
                .Where(m => m.LanguageId == keyId.Value)
                .AsQueryable();

            return list;
        }
    }
}
namespace KPMA.Controllers
{
    public class TableExController : GridController<Data.Models.TableEx, Data.Models.TableExViewModel, Data.Models.TableExFindModel>
    {
        public TableExController(   Managers.IGridManager<Data.Models.TableEx, Data.Models.TableExFindModel> objManager,
                                    Managers.ICoreManager coreManager,
                                    Managers.IAttachmentManager attManager,
                                    Managers.IMetaObjectManager moManager,
                                    Managers.IConstantManager constManager,
                                    Managers.IEmployeeManager employeeManager) : base(  objManager, 
                                                                                        coreManager, 
                                                                                        attManager, 
                                                                                        moManager, 
                                                                                        constManager, 
                                                                                        employeeManager)
        { }
    }
}
<grid [controllerName]="'TableEx'"
	  [name]="'Название заголовка'"
    [urlEditPage]=""
	  [canRead]="true"
	  [canAdd]="true"
	  [canEdit]="true"
	  [canDelete]="true"
	  [canCopy]="true"
	  [canAttach]="true"
	  [isViewOnly]="isViewOnly"
    [keyValue]="idFromViewPage"
	  [keyName]="'languageId'"
	  [listColumn]="[ {name:'Язык', key:'languageId', keyName:'languageName', type:'dropdown', itemType:'Language', minTerm:0, allowClear:true},
					  {name:'Текстовое наименование', key:'DisplayName', type:'text'},
					  {name:'Дата', key:'date', type:'date'}]"    >
</grid>
