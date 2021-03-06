# Типовой подход по разработке стандартного решения для сайтов

Краткое описание применения.

## Установка и настрока

### back-end

* Скачиваем библиотеки и добавляем [в свой созданный проект ASP.NET Core](https://www.microsoft.com/net/core#windowscmd) - ссылка на базовый проект
* Проеряем корректность написания namespace
* компилируем

### front-end (UI)

* Внедряем в [рабочий проект Angular 2](https://angular.io/guide/quickstart) - ссылка на базовый проект
* Скачивает вторую папк с клиентской частью
* Интегрирует в архитектуру вашего проекта
* настраиваем [пути к файлам](https://angular.io/tutorial/toh-pt5) и подключаем наши файлы

## Цели и задачи к реализации типового подхода

требования к разработке с возможностями расширения

### front-end (UI)

* Отображение списка учётом связанности данных
* Разделение на страницы
* Экономия времени загрузки
* Добавление новых данных учитывая обязательность заполнения
* Редактирование
* Удаление
* Быстрее получать новые модули

### back-end

* Project ASP.NET Core
* Front-end(UI): Angular2+ 
* Grid.Control/Grid.Service
* Back-end: WebApiController
* TableModel/ViewModel/FindModel
* TEST.Interfaces.IIdModel
* TEST.Interfaces.IDisplayName
* TEST.Interfaces.IClearVirtualMethodsModelViewModel
* IGridManager/GridManager (generit, async)
* IGridController/GridController (generit, async)

### db MS SQL

* dbo.Table


## Описание примера модели

```
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace TEST.Models
{
    [Table("TableEx", Schema = "dbo")]
    public class TableEx :	TEST.Interfaces.IIdModel, 
				TEST.Interfaces.IClearVirtualMethodsModel, 
				TEST.Interfaces.IDisplayName
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
```


## ASP.NET Core Manager

```
namespace TEST.Managers
{
    public class TableExManager : GridManager<TEST.Models.TableEx, TEST.Models.TableExFindModel>
    {
        public TableExManager(DbContext db) : base(db)
        {
        }
        override public IQueryable<TEST.Models.TableEx> GetGridList(int? keyId, TEST.Models.TableExFindModel findModel)
        {
            if (!keyId.HasValue) { 
	    	return db.GetGridAllList();
	    }
	    else {
	    	return db.GetGridAllList(m => m.LanguageId == keyId.Value);
	    }
        }
    }
}
```


## ASP.NET Core Controller

```
namespace TEST.Controllers
{
    public class TableExController : GridController<Data.Models.TableEx, Data.Models.TableExViewModel, Data.Models.TableExFindModel>
    {
        public TableExController(IGridManager<TEST.Models.TableEx, TEST.Models.TableExFindModel> objManager) : base(objManager)
        { }
    }
}
```

## Angular 2 HTML

```
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
```
