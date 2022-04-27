using iText.Html2pdf;
using System.Text;

namespace PDF_Report
{
    public class StandartMonitoringReport : MonitoringReport
    {
        private static readonly string _htmlTemplate = "<!DOCTYPE html><html lang='en'><head> <meta charset='UTF-8'> <title>Standart</title></head><style type='text/css'>html{height: 100%;}body{min-height: 100%; font-family: Arial, sans-serif; font-size: 12px; font-weight: normal; line-height: 1.3; margin-top: 1cm; margin-bottom: 3cm; margin-left: 2cm; margin-right: 1cm;}.hl{text-align: center; font-size: 18px;}.t1{font-weight: bold;}.tbl{border-collapse: collapse; text-align: center; border-spacing: 0; width: 100%;}.tbl td{border-color: black; border-style: solid; border-width: 1px; font-family: Arial, sans-serif; font-size: 12px; overflow: hidden; padding: 10px 5px; word-break: normal;}.ft{text-align: center; height: 100px;}.ft td{width: 50%;}@page{size: A4 portrait; /* set page margins */ margin: 0.5cm; @top-center{content: element(header);}@bottom-center{content: element(footer);}}</style><body> <table> <tr> <p class='hl'><b>Протокол мониторинга характеристик потока ионов сеанса</b></p></tr><tr> <p style='text-align: right;'>[ABB]/[YEAR]-[ION_NAME]-[PRTCL_NUM]-[SESSION_NUM]</p></tr><tr> <table class='t1'> <tr> <td>1. Общие сведения о сеансе:</td><td style='width: 70%; text-align: right;'>Сеанс № [SESSION_NUM]</td></tr></table> </tr><tr> <p>Испытательный ионный комплекс : [BENCH]</p></tr><tr> <table class='tbl'> <tr style='font-weight: bold;'> <td>Название организации</td><td>Шифр или наименование работы</td><td>Облучаемое изделие</td><td>Время начала облучения</td><td>Длительность</td></tr><tr> <td>[ORG_NAME]</td><td>[CIPER]</td><td>[IRR_PRODUCT]</td><td>[START_DATA] [START_TIME]</td><td>[DURATION]</td></tr></table> </tr><tr> <p class='t1'>2. Условия эксперемента: [EXP_CONDITION]</p></tr><tr> <table class='tbl'> <tr style='font-weight: bold;'> <td>Угол</td><td>Температура,°C</td><td>Материал дегрейдора</td><td>Толщина, мкм</td></tr><tr> <td>[ANGLE]</td><td>[TEMP]</td><td>[DEG_MATERIAL]</td><td>[THICKNESS]</td></tr></table> </tr><tr> <p class='t1'>3.Характеристики потока ионов:</p></tr><tr> <p>Характеристики иона:</p><table class='tbl'> <tr style='font-weight: bold;'> <td>Тип иона</td><td>Энергия Е на поверхности, МэВ/н</td><td>Пробег, R [Si], мкм</td><td>Линейные потери энергии ЛПЭ, МэВ×см2/мг [Si]</td></tr><tr> <td>[ION_NAME] <sup><small>[ION_NUM]</small></sup></td><td>[E_VALUE]±[E_MEASURE]</td><td>[MILEAGE_VALUE]±[MILEAGE_MEASURE]</td><td>[LINE_LOSS_VALUE]±[LINE_LOSS_MEASURE]</td></tr></table> </tr><tr> <table> <tr> <td style='width: 30%;'>Данные по пропорциональным счетчикам:</td><td style='text-align: right;'>Расчетный коэффициент К=[K_VALUE] ± [K_MEASURE]</td></tr></table> </tr><tr> <table style='width: 100%;'> <tr> <td style='width: 50%;'> <table class='tbl'> <tr style='font-weight: bold;'> <td>1</td><td>2</td><td>3</td><td>4</td><td style='font-weight: 400;'>Среднее значение</td></tr><tr> <td>[COUNTER_1_DATA]</td><td>[COUNTER_2_DATA]</td><td>[COUNTER_3_DATA]</td><td>[COUNTER_4_DATA]</td><td>[AVG_DATA_VALUE]</td></tr></table> </td><td style='text-align: right;'> <table> <tr> <p>(протокол допуска № [ADM_PRTCL_NUM])</p></tr><tr> <p>Фактический коэффициент К=[ACTUAL_KOEF]</p></tr></table> </td></tr></table> </tr><tr> <p>Данные по трековым мембранам из лавсановой пленки:</p></tr><tr> <table class='tbl' style='width: 100%;'> <tr style='font-weight: bold;'> <td>Детектор 1</td><td>Детектор 2</td><td style='font-weight: 400;'>Неоднородность, %</td></tr><tr> <td>[DTCTR_1_DATA]</td><td>[DTCTR_2_DATA]</td><td>[HTRGNT]</td></tr></table> </tr><tr> <br><br><br><table class='ft'> <tr> <td>Ответственный за проведение испытаний в испытательную смену от ООО \"НПП \"Детектор\"</td><td>Технический директор ООО \"НПП \"Детектор\"</td></tr><tr> <td>_____________________ (&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;)</td><td>_____________________ (&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;)</td></tr></table> </tr></table></body></html>";

        private StringBuilder _htmlDefault = new StringBuilder(_htmlTemplate);
        private StringBuilder _html = new StringBuilder(_htmlTemplate);

        public StandartMonitoringReport()
        {
            ParamsReplace =new Dictionary<string, string>()
            {
                {"[ABB]", "0" },
                {"[YEAR]", "0" },
                {"[ION_NAME]", "0" },
                {"[ION_NUM]", "0" },
                {"[PRTCL_NUM]", "0" },
                {"[SESSION_NUM]", "0" },
                {"[BENCH]", "0" },
                {"[ORG_NAME]", "0" },
                {"[CIPER]", "0" },
                {"[IRR_PRODUCT]", "0" },
                {"[START_DATA]", "0" },
                {"[START_TIME]", "0" },
                {"[DURATION]", "0" },
                {"[EXP_CONDITION]", "0" },
                {"[ANGLE]", "0" },
                {"[TEMP]", "0" },
                {"[DEG_MATERIAL]", "0" },
                {"[THICKNESS]", "0" },
                {"[E_VALUE]", "0" },
                {"[E_MEASURE]", "0" },
                {"[MILEAGE_VALUE]", "0" },
                {"[MILEAGE_MEASURE]", "0" },
                {"[LINE_LOSS_VALUE]", "0" },
                {"[LINE_LOSS_MEASURE]", "0" },
                {"[K_VALUE]", "0" },
                {"[K_MEASURE]", "0" },
                {"[COUNTER_1_DATA]", "0" },
                {"[COUNTER_2_DATA]", "0" },
                {"[COUNTER_3_DATA]", "0" },
                {"[COUNTER_4_DATA]", "0" },
                {"[AVG_DATA_VALUE]", "0" },
                {"[ADM_PRTCL_NUM]", "0" },
                {"[ACTUAL_KOEF]", "0" },
                {"[DTCTR_1_DATA]", "0" },
                {"[DTCTR_2_DATA]", "0" },
                {"[HTRGNT]", "0" }
            };
            _htmlDefault = Rename(_htmlDefault);
        }

        private StringBuilder Rename(StringBuilder html)
        {
            foreach (var p in ParamsReplace)
            {
                html.Replace(p.Key, p.Value);
            }

            return html;
        }

        public void ConvertToPDF(string xPath)
        {
            using (FileStream fs = new FileStream(xPath, FileMode.OpenOrCreate))
            {
                HtmlConverter.ConvertToPdf(GetHtml(), fs);
            }
        }

        /// <summary>
        /// Возвращает преобразованный html протокол.<br/>
        /// Если поля не были проинициализированны, то они будут заполенены значениями по умолчанию.
        /// </summary>
        public override string GetHtml()
        {
            _html = Rename(_html);

            return _html.ToString();
        }

        /// <summary>
        /// Возвращает непреобразованный шаблон html протокола.
        /// </summary>
        public string GetHtmlTemplate() => _htmlTemplate;
    }
}
