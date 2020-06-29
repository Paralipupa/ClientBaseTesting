using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Templates
{
    class SQLTemplates
    {
        public static string SELECT_COMMON_SIMPLE = "select * from {0}";
        public static string SELECT_COMMON_WHERE = "select * from {0} where {1}";
        public static string SELECT_COMMON_WHERE_ORDER = "select * from {0} where {1} order by {2}";
        public static string SELECT_COMMON_ORDER = "select * from {0} order by {1}";
        public static string SELECT_PRICELIST_PRODUCT = "SELECT DISTINCT `Прайслист-Номенклатура`.КодПрайслист, `Прайслист-Номенклатура`.КодНоменклатура, Номенклатура.Наименование FROM Прайслист INNER JOIN(Номенклатура INNER JOIN `Прайслист-Номенклатура` ON Номенклатура.Код = `Прайслист-Номенклатура`.КодНоменклатура) ON Прайслист.Код = `Прайслист-Номенклатура`.КодПрайслист  WHERE (((`Прайслист-Номенклатура`.КодПрайслист)={0}))";
        public static string SELECT_PRICELIST_PRODUCT_COST = "SELECT`Прайслист-Номенклатура`.КодПрайслист, `Прайслист-Номенклатура`.КодНоменклатура, `Прайслист-Номенклатура`.КодЦена, Цена.КодТипЦены, Цена.Сумма, Цена.Количество, Цена.Копия, `Тип цены`.Наименование FROM `Тип цены` INNER JOIN(Цена INNER JOIN`Прайслист-Номенклатура` ON Цена.Код = `Прайслист-Номенклатура`.КодЦена) ON `Тип цены`.Код = Цена.КодТипЦены WHERE(((`Прайслист-Номенклатура`.КодПрайслист)={0}) AND((`Прайслист-Номенклатура`.КодНоменклатура)={1}) AND((Цена.КодТипЦены)= {2}))";

    }
}
