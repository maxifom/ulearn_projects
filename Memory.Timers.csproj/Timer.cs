using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Memory.Timers
{
    /// <summary>
    /// Таймер
    /// </summary>
    public class Timer : IDisposable
    {
        private static string[] reports;

        /// <summary>
        /// Отчет таймера
        /// </summary>
        public static string Report => string.Join("", reports) + MakeRest();

        /// <summary>
        /// Сделать отчет по 
        /// оставшемуся времени
        /// </summary>
        /// <returns>
        /// Отчет по оставшемуся времени
        /// </returns>
        private static string MakeRest()
        {
            var rest = "";
            var reports = Timer.reports.Where(x => x != null).ToList();
            var allTime = Convert.ToInt32(Regex.Replace(reports[0], @"\D", ""));
            for (int i = reports.Count - 1; i > 0; i--)
            {
                var time = 0;
                var splittedReports = reports[i].Split('\n').Where(x => x.Length > 0);
                foreach (var report in splittedReports)
                    time += Convert.ToInt32(Regex.Replace(report, @"\D", ""));

                rest += MakeReport(i * 4, "Rest", allTime - time);
            }
            return rest;
        }

        private static readonly List<string> activeTimers = new List<string>();

        private readonly Stopwatch stopwatch;

        private readonly string name;

        private bool isDisposed = false;

        /// <summary>
        /// Конструктор таймера
        /// </summary>
        /// <param name="name">
        /// Имя таймера
        /// </param>
        public Timer(string name)
        {
            this.name = name;
            this.stopwatch = new Stopwatch();
            this.stopwatch.Start();
        }

        /// <summary>
        /// Запустить таймер
        /// </summary>
        /// <param name="timerName">
        /// Имя таймера
        /// </param>
        /// <returns>
        /// Новый таймер
        /// </returns>
        public static IDisposable Start(string timerName = "*")
        {
            if (activeTimers.Count == 0)
                reports = new string[10];

            activeTimers.Add(timerName);
            return new Timer(timerName);
        }

        /// <summary>
        /// Финализатор Таймера
        /// </summary>
        ~Timer() => Dispose(false);

        /// <summary>
        /// Осовободить память,
        /// выделенную таймером
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Освободить память,
        /// выделенную таймером
        /// </summary>
        /// <param name="fromDisposeMethod">
        /// Флаг для указания
        /// откуда была вызвана функция:
        /// true - явным образом
        /// false - из финализатора
        /// </param>
        protected virtual void Dispose(bool fromDisposeMethod)
        {
            if (!isDisposed)
            {
                if (fromDisposeMethod)
                {
                    var index = activeTimers.IndexOf(this.name);
                    this.stopwatch.Stop();
                    var offset = 4 * index;
                    reports[index] += MakeReport(offset, this.name, this.stopwatch.ElapsedMilliseconds);
                    activeTimers.Remove(this.name);
                }
                isDisposed = true;
            }
        }

        /// <summary>
        /// Сделать отчет
        /// </summary>
        /// <param name="offset">
        /// Смещение в строке отчета
        /// </param>
        /// <param name="name">
        /// Имя отчета
        /// </param>
        /// <param name="time">
        /// Затраченное время
        /// </param>
        /// <returns>
        /// Отчет
        /// </returns>
        private static string MakeReport(int offset, string name, long time)
        {
            var report = new StringBuilder();
            report.Append(' ', offset);
            report.Append(name);
            report.Append(' ', 20 - name.Length - offset);
            report.Append(": " + time + '\n');
            return report.ToString();
        }
    }
}