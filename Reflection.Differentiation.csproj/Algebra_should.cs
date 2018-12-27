using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Reflection.Differentiation
{
	[TestFixture]
	public class Derivative_shouldBeCorrectFor
	{
		void TestDerivative(Expression<Func<double, double>> function)
		{
			var f = function.Compile();
			double eps = 1e-7;
			var dfunction = Algebra.Differentiate(function);
			var df = dfunction.Compile();
			for (double x = 0; x < 5; x += 0.1)
			{
				Assert.AreEqual(df(x), (f(x + eps) - f(x)) / eps, 1e-5, $"Error on function {function.Body}");
			}
		}

		[Test]
		public void Constant()
		{
			TestDerivative(z => 1);
		}

		[Test]
		public void Parameter()
		{
			TestDerivative(z => z);
		}

		[Test]
		public void Product1()
		{
			TestDerivative(z => z * 5);
		}

		[Test]
		public void Product2()
		{
			TestDerivative(z => z * z * 5);
		}

		[Test]
		public void Sum1()
		{
			TestDerivative(z => z + z);
		}

		[Test]
		public void Sum2()
		{
			TestDerivative(z => 5 * z + z * z);
		}

		[Test]
		public void Sin1()
		{
			TestDerivative(z => Math.Sin(z));
		}

		[Test]
		public void Sin2()
		{
			TestDerivative(z => Math.Sin(z * z + z));
		}

		[Test]
		public void Cos1()
		{
			TestDerivative(z => Math.Cos(z));
		}

		[Test]
		public void Cos2()
		{
			TestDerivative(z => Math.Cos(z * z + z));
		}
	}
}
