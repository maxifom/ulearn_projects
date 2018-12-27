using NUnit.Framework;
using System;
using System.ComponentModel;
using System.Linq;

namespace Reflection.Randomness
{
	public class T1
	{
		[FromDistribution(typeof(NormalDistribution), 1, 2)]
		public double A { get; set; }
	}

	public class T2
	{
		[FromDistribution(typeof(NormalDistribution), -1, 2)]
		public double A { get; set; }

		[FromDistribution(typeof(ExponentialDistribution), 4)]
		[DisplayName("B-value")]
		public double B { get; set; }

		// ReSharper disable ArrangeAccessorOwnerBody
		public double С { get { return 42; } }
		// ReSharper restore ArrangeAccessorOwnerBody

		public double D { get; set; }

		[FromDistribution(typeof(NormalDistribution))]
		public double E { get; set; }
	}

	public class T3
	{
		[FromDistribution(typeof(NormalDistribution), 1, 2, 3)]
		public double WrongDistributionArguments { get; set; }
	}

	[TestFixture]
	public class Generator_should
	{
		private const int seed = 123;
		private static readonly ExponentialDistribution defaultBDistribution = new ExponentialDistribution(4);
		private static readonly NormalDistribution defaultADistribution = new NormalDistribution(-1, 2);
		private static readonly NormalDistribution defaultEDistribution = new NormalDistribution();

		[Test]
		public void GenerateT1()
		{
			var rnd = new Random(seed);
			var e = new Generator<T1>().Generate(rnd);
			AssertPropertyFilledWithDistribution(e.A, new NormalDistribution(1, 2));
		}

		[Test]
		public void GenerateT1TwiceGivesUniqueObjects()
		{
			var rnd = new Random(seed);
			var generator = new Generator<T1>();
			var e1 = generator.Generate(rnd);
			var e2 = generator.Generate(rnd);
			Assert.AreNotSame(e1, e2);
		}

		[Test]
		public void GenerateT2()
		{
			var rnd = new Random(seed);
			var e = new Generator<T2>().Generate(rnd);
			AssertPropertyFilledWithDistribution(e.A, defaultADistribution);
			AssertPropertyFilledWithDistribution(e.B, defaultBDistribution);
			Assert.AreEqual(0.0, e.D, 1e-3, "property without attrubutes should not be changed");
			AssertPropertyFilledWithDistribution(e.E, defaultEDistribution);
		}

		[Test]
		public void NotAllowForAfterFor()
		{
			// generator.For(z => z.A).For(z => z.B).Set(d) ← is not valid!
			var forResult = new Generator<T2>().For(z => z.A);
			var forMethod = forResult.GetType().GetMethods().FirstOrDefault(m => m.Name == "For");
			Assert.That(forMethod, Is.Null);
		}

		[Test]
		public void ReplaceGeneratorFor1Field()
		{
			var newDistribution = new NormalDistribution(10, 1);
			var generator = new Generator<T2>()
				.For(z => z.A)
				.Set(newDistribution);
			var e = generator.Generate(new Random(seed));
			AssertPropertyFilledWithDistribution(e.A, newDistribution);
			AssertPropertyFilledWithDistribution(e.B, defaultBDistribution);
		}

		[Test]
		public void ReplaceGeneratorFor2Fields()
		{
			var rnd = new Random(seed);
			var newADistr = new NormalDistribution(0, 1);
			var newBDistr = new NormalDistribution(1, 1);
			var generator = new Generator<T2>()
				.For(z => z.A)
				.Set(newADistr)
				.For(z => z.B)
				.Set(newBDistr);
			var e = generator.Generate(rnd);
			AssertPropertyFilledWithDistribution(e.A, newADistr);
			AssertPropertyFilledWithDistribution(e.B, newBDistr);
		}

		[Test]
		public void SetGeneratorForFieldWithoutAttributes()
		{
			var rnd = new Random(seed);
			var generator = new Generator<T2>()
				.For(z => z.D)
				.Set(new NormalDistribution());
			var e = generator.Generate(rnd);
			AssertPropertyFilledWithDistribution(e.D, new NormalDistribution());
		}

		[Test]
		public void FailWithInformativeMessage_OnIncorrectAttributeUsage()
		{
			// ReSharper disable once ObjectCreationAsStatement
			var ex = Assert.Throws<ArgumentException>(() => new Generator<T3>().Generate(new Random(seed)));
			Assert.That(ex.Message, Contains.Substring("NormalDistribution"),
				"Exception message should be informative and contain at least the name of problematic type");
		}

		[Test]
		public void FailWithInformativeMessage_OnIncorrectForMethodCall()
		{
			var generator = new Generator<T1>();
			Assert.Throws<ArgumentException>(
				() => generator.For(t => 42));
			Assert.Throws<ArgumentException>(
				() => generator.For(t => Math.Sin(t.A)));
			Assert.Throws<ArgumentException>(
				() => generator.For(t => X));
		}

		// ReSharper disable once UnassignedGetOnlyAutoProperty
		public double X { get; }

		private void AssertPropertyFilledWithDistribution(double actualValue, IContinousDistribution distribution)
		{
			var random = new Random(seed);
			var sequenceStart = new[] { distribution.Generate(random), distribution.Generate(random), distribution.Generate(random), distribution.Generate(random), distribution.Generate(random)};
			random = new Random(seed);
			random.NextDouble();
			var sequence = sequenceStart.Concat(new[] { distribution.Generate(random), distribution.Generate(random), distribution.Generate(random), distribution.Generate(random), distribution.Generate(random) });
			Assert.That(sequence, Does.Contain(actualValue));
		}
	}
}
