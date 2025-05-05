using System.Linq.Expressions;

namespace Hotels.Presentation.Interfaces;

public interface IFilterModel
{
    void Reset();
}

public interface IFilterModel<TModel> : IFilterModel
{
    /// <summary>
    /// Выполняет фильтрацию, генерируя SQL-запрос к БД <br/><br/>
    /// <b>Прим:</b><br/> 
    /// Обязательно установить атрибут `[JsonIgnore]`<br/>
    /// Не использовать `[NotMapped]`-свойства, так как они не сохраняются в БД
    /// </summary>
    Expression<Func<TModel, bool>> FilterExpression { get; }
}
