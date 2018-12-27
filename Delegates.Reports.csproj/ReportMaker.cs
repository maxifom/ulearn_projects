using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.Reports
{
    /// <summary>
    /// Создатель отчетов
    /// </summary>
    public abstract class ReportMaker
    {
        protected Func<string, string> MakeCaption;
        protected Func<string> BeginList;
        protected Func<string, string, string> MakeItem;
        protected Func<string> EndList;
        protected Func<IEnumerable<double>, object> MakeStatistics;

        /// <summary>
        /// Загаловок отчета
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Создает отчет
        /// </summary>
        /// <param name="measurements">
        /// Измерения
        /// </param>
        /// <returns>
        /// Отчет
        /// </returns>
        public string MakeReport(IEnumerable<Measurement> measurements)
        {
            var data = measurements.ToList();
            var result = new StringBuilder();
            result.Append(MakeCaption(Caption));
            result.Append(BeginList());
            result.Append(MakeItem("Temperature", MakeStatistics(data.Select(z => z.Temperature)).ToString()));
            result.Append(MakeItem("Humidity", MakeStatistics(data.Select(z => z.Humidity)).ToString()));
            result.Append(EndList());
            return result.ToString();
        }
    }

    /// <summary>
    /// Помощник для создания
    /// отчетов
    /// </summary>
    public class ReportHelper
    {
        /// <summary>
        /// Считает среднее и 
        /// стандартное отклонение
        /// </summary>
        /// <param name="_data">
        /// Данные для рассчетов
        /// </param>
        /// <returns>
        /// Среднее и стандартное отклонение
        /// </returns>
        public static object GetMeanAndStd(IEnumerable<double> originalData)
        {
            var data = originalData.ToList();
            var mean = data.Average();
            var std = Math.Sqrt(data.Select(z => Math.Pow(z - mean, 2)).Sum() / (data.Count - 1));

            return new MeanAndStd
            {
                Mean = mean,
                Std = std
            };
        }

        /// <summary>
        /// Считает медиану
        /// </summary>
        /// <param name="data">
        /// Данные для рассчетов
        /// </param>
        /// <returns>
        /// Медиану
        /// </returns>
        public static object GetMedian(IEnumerable<double> data)
        {
            var list = data.OrderBy(z => z).ToList();
            if (list.Count % 2 == 0)
                return (list[list.Count / 2] + list[list.Count / 2 - 1]) / 2;
            else
                return list[list.Count / 2];
        }
    }

    /// <summary>
    /// Cоздатель отчетов по 
    /// среднему и стандартному
    /// отклонению в формате html
    /// </summary>
    public class MeanAndStdHtmlReportMaker : ReportMaker
    {
        /// <summary>
        /// Конструктор создателя отчетов по 
        /// среднему и стандартному
        /// отклонению в формате html
        /// </summary>
        public MeanAndStdHtmlReportMaker()
        {
            Caption = "Mean and Std";
            MakeCaption = (caption) => $"<h1>{caption}</h1>";
            BeginList = () => "<ul>";
            EndList = () => "</ul>";
            MakeItem = (valueType, entry) => $"<li><b>{valueType}</b>: {entry}";
            MakeStatistics = (originalData) => ReportHelper.GetMeanAndStd(originalData);
        }
    }

    /// <summary>
    /// Создатель отчетов по 
    /// медиане в формате markdown
    /// </summary>
    public class MedianMarkdownReportMaker : ReportMaker
    {
        /// <summary>
        /// Конструктор создателя отчетов по 
        /// медиане в формате markdown
        /// </summary>
        public MedianMarkdownReportMaker()
        {
            Caption = "Median";
            MakeCaption = (caption) => $"## {caption}\n\n";
            BeginList = () => "";
            EndList = BeginList;
            MakeItem = (valueType, entry) => $" * **{valueType}**: {entry}\n\n";
            MakeStatistics = (data) => ReportHelper.GetMedian(data);
        }
    }

    /// <summary>
    /// Создатель отчетов по 
    /// среднему и стандартному
    /// отклонению в формате markdown
    /// </summary>
    public class MeanAndStdMarkdownReportMaker : ReportMaker
    {
        /// <summary>
        /// Конструктор создателя отчетов по 
        /// среднему и стандартному
        /// отклонению в формате markdown
        /// </summary>
        public MeanAndStdMarkdownReportMaker()
        {
            Caption = "Mean and Std";
            BeginList = () => "";
            EndList = BeginList;
            MakeCaption = (caption) => $"## {caption}\n\n";
            MakeItem = (valueType, entry) => $" * **{valueType}**: {entry}\n\n";
            MakeStatistics = (originalData) => ReportHelper.GetMeanAndStd(originalData);
        }
    }

    /// <summary>
    /// Создатель отчетов по 
    /// медиане в формате html
    /// </summary>
    public class MedianHtmlReportMaker : ReportMaker
    {
        /// <summary>
        /// Конструктор создателя отчетов по 
        /// медиане в формате html
        /// </summary>
        public MedianHtmlReportMaker()
        {
            Caption = "Median";
            MakeCaption = (caption) => $"<h1>{caption}</h1>";
            BeginList = () => "<ul>";
            EndList = () => "</ul>";
            MakeItem = (valueType, entry) => $"<li><b>{valueType}</b>: {entry}";
            MakeStatistics = (data) => ReportHelper.GetMedian(data);
        }
    }

    /// <summary>
    /// Создает различные отчеты
    /// </summary>
    public static class ReportMakerHelper
    {
        /// <summary>
        /// Создает отчет по 
        /// среднему и стандартному
        /// отклонению в формате html
        /// </summary>
        /// <param name="data">
        /// Данные для расчетов
        /// </param>
        /// <returns>
        /// Отчет в формате html
        /// </returns>
        public static string MeanAndStdHtmlReport(IEnumerable<Measurement> data)
        {
            return new MeanAndStdHtmlReportMaker().MakeReport(data);
        }

        /// <summary>
        /// Создает отчет по 
        /// медиане в формате markdown
        /// </summary>
        /// <param name="data">
        /// Данные для расчетов
        /// </param>
        /// <returns>
        /// Отчет в формате markdown
        /// </returns>
        public static string MedianMarkdownReport(IEnumerable<Measurement> data)
        {
            return new MedianMarkdownReportMaker().MakeReport(data);
        }

        /// <summary>
        /// Создает отчет по 
        /// среднему и стандартному
        /// отклонению в формате markdown
        /// </summary>
        /// <param name="data">
        /// Данные для расчетов
        /// </param>
        /// <returns>
        /// Отчет в формате markdown
        /// </returns>
        public static string MeanAndStdMarkdownReport(IEnumerable<Measurement> measurements)
        {
            return new MeanAndStdMarkdownReportMaker().MakeReport(measurements);
        }

        /// <summary>
        /// Создает отчет по 
        /// медиане в формате html
        /// </summary>
        /// <param name="data">
        /// Данные для расчетов
        /// </param>
        /// <returns>
        /// Отчет в формате html
        /// </returns>
        public static string MedianHtmlReport(IEnumerable<Measurement> measurements)
        {
            return new MedianHtmlReportMaker().MakeReport(measurements);
        }
    }
}
