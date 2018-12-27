using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generics.Tables
{
    [TestFixture]
    public class OpenIndex_should
    {
        [Test]
        public void AddColumnsAndRows()
        {
            var table = new Table<int, string, double>();
            table.Open[1, "2"] = 3;
            Assert.AreEqual(3, table.Open[1, "2"]);
            Assert.AreEqual(1, table.Rows.Count());
            Assert.AreEqual(1, table.Columns.Count());
        }

        [Test]
        public void DontAddExistedColumnsAndRows1()
        {
            var table = new Table<int, string, double>();
            table.AddRow(1);
            table.AddColumn("2");
            table.Open[1, "2"] = 3;
            Assert.AreEqual(3, table.Open[1, "2"]);
            Assert.AreEqual(1, table.Rows.Count());
            Assert.AreEqual(1, table.Columns.Count());
        }

        [Test]
        public void DontAddExistedColumnsAndRows2()
        {
            var table = new Table<int, string, double>();
            table.Open[1, "2"] = 3;
            table.AddRow(1);
            table.AddColumn("2");

            Assert.AreEqual(3, table.Open[1, "2"]);
            Assert.AreEqual(1, table.Rows.Count());
            Assert.AreEqual(1, table.Columns.Count());
        }

        [Test]
        public void ReturnDefaultIfNoValueSet1()
        {
            var table = new Table<int, string, double>();
            Assert.AreEqual(0, table.Open[1, "2"]);
            Assert.AreEqual(0, table.Rows.Count());
            Assert.AreEqual(0, table.Columns.Count());
        }

        [Test]
        public void ReturnDefaultIfNoValueSet2()
        {
            var table = new Table<int, string, double>();
            table.AddRow(1);
            table.AddColumn("2");
            Assert.AreEqual(0, table.Open[1, "2"]);
            Assert.AreEqual(1, table.Rows.Count());
            Assert.AreEqual(1, table.Columns.Count());
        }
    }

    [TestFixture]
    public class ExistedIndex_should
    {
        [Test]
        public void FailIfGettingFromNonExistingRow()
        {
            var table = new Table<string, int, double>();
            table.AddColumn(1);
            Assert.Throws(typeof(ArgumentException), () => Console.Write(table.Existed["1", 1]));
        }
        [Test]
        public void FailIfGettingFromNonExistingColumn()
        {
            var table = new Table<string, int, double>();
            table.AddRow("1");
            Assert.Throws(typeof(ArgumentException), () => Console.Write(table.Existed["1", 1]));
        }
        [Test]
        public void FailIfSettingToNonExistingRow()
        {
            var table = new Table<string, int, double>();
            table.AddColumn(1);
            Assert.Throws(typeof(ArgumentException), () => table.Existed["1", 1] = 1);
        }
        [Test]
        public void FailIfSettingToNonExistingColumn()
        {
            var table = new Table<string, int, double>();
            table.AddRow("1");
            Assert.Throws(typeof(ArgumentException), () => table.Existed["1", 1] = 1);
        }

        [Test]
        public void SetToExistedRowAndColumn()
        {
            var table = new Table<string, int, double>();
            table.AddRow("1");
            table.AddColumn(1);
            table.Existed["1", 1] = 1;
            Assert.AreEqual(1, table.Existed["1", 1]);
        }

        [Test]
        public void GetDefaultIfNoValueSet()
        {
            var table = new Table<string, int, double>();
            table.AddRow("1");
            table.AddColumn(1);
            Assert.AreEqual(0, table.Existed["1", 1]);
        }
    }

    [TestFixture]
    public class BothIndices_should
    {
        [Test]
        public void WorkTogether1()
        {
            var table = new Table<string, int, double>();
            table.AddRow("1");
            table.AddColumn(1);
            table.Existed["1", 1] = 2;
            Assert.AreEqual(2, table.Open["1", 1]);
        }

        [Test]
        public void WorkTogether2()
        {
            var table = new Table<string, int, double>();
            table.Open["1", 1] = 2;
            Assert.AreEqual(2, table.Existed["1", 1]);
        }
    }
}
