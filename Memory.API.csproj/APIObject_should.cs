using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Memory.API
{
	[TestFixture]
	public class APIObject_should
	{
		[Test]
		public void FreeResourcesWhenDisposeCalled()
		{
			var q = new APIObject(1);
			Assert.True(MagicAPI.Contains(1));
			q.Dispose();
			Assert.False(MagicAPI.Contains(1));
		}

		[Test]
		public void FreeResourcesInUsing()
		{
			using (var q = new APIObject(2))
			{
				Assert.True(MagicAPI.Contains(2));

			}
			Assert.False(MagicAPI.Contains(2));
		}

		[Test]
		public void DontFailWhenTwoDisposeAreCalled()
		{
			var q = new APIObject(3);
			Assert.True(MagicAPI.Contains(3));
			q.Dispose();
			q.Dispose();
			Assert.False(MagicAPI.Contains(3));
		}

		void CreateApiObject(int n)
		{
			var q = new APIObject(n);
		}

		[Test]
		public void HaveFinalizer()
		{
			CreateApiObject(4);
			GC.Collect();
			Thread.Sleep(1000);
			Assert.False(MagicAPI.Contains(4));
		}

		void CreateAndDisposeApiObject(int n)
		{
			var q = new APIObject(n);
			q.Dispose();
		}

		[Test]
		public void DisposeDontCrashFinalizer()
		{
			CreateAndDisposeApiObject(42);
			GC.Collect();
			Thread.Sleep(1000);
			Assert.False(MagicAPI.Contains(42));
			Assert.False(MagicAPI.Contains(100500));
		}
	}
}
