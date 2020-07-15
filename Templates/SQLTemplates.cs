using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientBaseTesting.Templates
{
    class SQLTemplates
    {
        public static string SELECT_COMMON_SIMPLE = "select * from {0}";
        public static string SELECT_COMMON_WHERE = "select * from {0} where {1}";
        public static string SELECT_COMMON_WHERE_ORDER = "select * from {0} where {1} order by {2}";
        public static string SELECT_COMMON_ORDER = "select * from {0} order by {1}";

        public static string SELECT_GRUPPA_NOMENCLATURE = "SELECT `Прайслист-Номенклатура`.Код, `Прайслист-Номенклатура`.КодПрайслист, ГруппаПрайслистНоменклатура.КодГруппаНоменклатура, `Прайслист-Номенклатура`.КодНоменклатура, Номенклатура.Наименование FROM Номенклатура INNER JOIN(`Прайслист-Номенклатура` INNER JOIN ГруппаПрайслистНоменклатура ON `Прайслист-Номенклатура`.Код = ГруппаПрайслистНоменклатура.КодПрайслистНоменклатура) ON Номенклатура.Код = `Прайслист-Номенклатура`.КодНоменклатура WHERE (((`Прайслист-Номенклатура`.КодПрайслист)={0}) AND((ГруппаПрайслистНоменклатура.КодГруппаНоменклатура)= {1}))";
        public static string SELECT_PRICELIST_NOMENCLATURE = "SELECT `Прайслист-Номенклатура`.Код, `Прайслист-Номенклатура`.КодНоменклатура, `Прайслист-Номенклатура`.КодПрайслист,Номенклатура.Наименование FROM Номенклатура INNER JOIN`Прайслист-Номенклатура` ON Номенклатура.Код = `Прайслист-Номенклатура`.КодНоменклатура WHERE(((`Прайслист-Номенклатура`.КодПрайслист)={0}))";
        public static string SELECT_NOMENCLATURE_COST = "SELECT * FROM Цена WHERE КодПрайслистНоменклатура={0} AND КодТипЦены={1} ORDER BY Копия, Сумма";
        public static string SELECT_PRICELIST_GRUPPA = "SELECT DISTINCT ГруппаНоменклатура.Код, ГруппаНоменклатура.Наименование FROM `Прайслист-Номенклатура` INNER JOIN (ГруппаНоменклатура INNER JOIN ГруппаПрайслистНоменклатура ON ГруппаНоменклатура.Код = ГруппаПрайслистНоменклатура.КодГруппаНоменклатура) ON `Прайслист-Номенклатура`.Код = ГруппаПрайслистНоменклатура.КодПрайслистНоменклатура WHERE(((`Прайслист-Номенклатура`.КодПрайслист)={0}))";
    }
}
