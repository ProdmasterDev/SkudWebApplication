using Microsoft.Extensions.Localization;
using MudBlazor;

namespace SkudWebApplication
{
    public class RusMudLocalizer : MudLocalizer
    {
        private Dictionary<string, string> _localization;

        public RusMudLocalizer()
        {
            _localization = new()
            {
                {"MudDataGrid.Value", "Значение" },
                {"MudDataGrid.Unsort","Убрать сортировку" },
                {"MudDataGrid.Ungroup", "Убрать группировку"},
                {"MudDataGrid.True", "Истина"},
                {"MudDataGrid.starts with", "начинается с"},
                {"MudDataGrid.Sort", "Сортировать"},
                {"MudDataGrid.ShowAll", "Показать всё"},
                {"MudDataGrid.Save", "Сохранить"},
                {"MudDataGrid.RefreshData", "Обновить данные"},
                {"MudDataGrid.Operator", "Оператор"},
                {"MudDataGrid.not equals", "не равняется"},
                {"MudDataGrid.not contains", "не содержит" },
                {"MudDataGrid.MoveUp", "Двигаться вверх"},
                {"MudDataGrid.MoveDown", "Двигаться вниз"},
                {"MudDataGrid.is on or before", "до этого включительно"},
                {"MudDataGrid.is on or after", "после этого включительно"},
                {"MudDataGrid.is not empty", "не пусто" },
                {"MudDataGrid.is not", "не"},
                {"MudDataGrid.is empty", "пусто"},
                {"MudDataGrid.is before", "до"},
                {"MudDataGrid.is after", "после"},
                {"MudDataGrid.is", "равно"},
                {"MudDataGrid.HideAll", "Скрыть всё"},
                {"MudDataGrid.Hide", "Скрыть"},
                {"MudDataGrid.Group", "Сгруппировать"},
                {"MudDataGrid.FilterValue", "Значение фильтра"},
                {"MudDataGrid.Filter", "Фильтр"},
                {"MudDataGrid.False", "Ложь"},
                {"MudDataGrid.ExpandAllGroups", "Развернуть все группы"},
                {"MudDataGrid.equals", "равняется"},
                {"MudDataGrid.ends with", "заканчивается на"},
                {"MudDataGrid.contains", "содержит" },
                {"MudDataGrid.Columns", "Колонки"},
                {"MudDataGrid.Column", "Колонка"},
                {"MudDataGrid.CollapseAllGroups", "Свернуть все группы"},
                {"MudDataGrid.Clear", "Очистить"},
                {"MudDataGrid.Cancel", "Отмена"},
                {"MudDataGrid.Apply", "Подтвердить"},
                {"MudDataGrid.AddFilter", "Добавить фильтр"},
            };
        }

        public override LocalizedString this[string key]
        {
            get
            {
                var currentCulture = Thread.CurrentThread.CurrentUICulture.Parent.TwoLetterISOLanguageName;
                if (currentCulture.Equals("ru", StringComparison.InvariantCultureIgnoreCase)
                && _localization.TryGetValue(key, out var res))
                {
                    return new(key, res);
                }
                else
                {
                    return new(key, key, true);
                }
            }
        }
    }
}
