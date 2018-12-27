using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.Observers
{
	[TestFixture]
	public class ObservableStack_Tests
	{
		[Test]
		public void Log_ShouldBeEmpty_AfterCreation()
		{
			var stack = new ObservableStack<int>();
			var helper = new StackOperationsLogger();
            helper.SubscribeOn(stack);
            Assert.AreEqual("", helper.GetLog());
		}

		[Test]
		public void Log_ShouldContainAllOperations()
		{
			var stack = new ObservableStack<int>();
			var helper = new StackOperationsLogger();
			helper.SubscribeOn(stack);
			stack.Push(1);
			stack.Push(2);
			stack.Pop();
			stack.Push(10);
			Assert.AreEqual("+1+2-2+10", helper.GetLog());

		}
	}
}
