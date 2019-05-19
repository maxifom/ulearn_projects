using System;
using System.Linq;
using Ddd.Infrastructure;
using NUnit.Framework;

namespace Ddd.Taxi.Domain
{
	[TestFixture]
	public class TaxiOrder_Should
	{
		private readonly Type taxiOrderType = typeof(TaxiOrder);

		private ITaxiApi<TaxiOrder> CreateApi()
		{
			return new TaxiApi(new DriversRepository(), () => time);
		}

		private DateTime time = DateTime.MinValue;

		[Test]
		public void AssignDriver()
		{
			var taxiApi = CreateApi();
			time = new DateTime(2017, 1, 1);
			var order = taxiApi.CreateOrderWithoutDestination("John", "Doe", "Street1", "building1");
			time = new DateTime(2017, 1, 2);
			taxiApi.AssignDriver(order, 15);
			Assert.AreEqual(
				"OrderId: 0 Status: WaitingCarArrival Client: John Doe Driver: Drive Driverson From: Street1 building1 To:  LastProgressTime: 2017-01-02 00:00:00",
				taxiApi.GetShortOrderInfo(order));
			Assert.AreEqual(
				"Id: 15 DriverName: Drive Driverson Color: Baklazhan CarModel: Lada sedan PlateNumber: A123BT 66",
				taxiApi.GetDriverFullInfo(order));
		}

		[Test]
		public void BeInitialized_AfterCreation()
		{
			var taxiApi = CreateApi();
			time = new DateTime(2017, 1, 1);
			var order = taxiApi.CreateOrderWithoutDestination("John", "Doe", "Street1", "building1");
			Assert.AreEqual(
				"OrderId: 0 Status: WaitingForDriver Client: John Doe Driver:  From: Street1 building1 To:  LastProgressTime: 2017-01-01 00:00:00",
				taxiApi.GetShortOrderInfo(order));
		}

		[Test]
		public void BeRichModel()
		{
			taxiOrderType.AssertHasMethod("AssignDriver");
			taxiOrderType.AssertHasMethod("Cancel");
			taxiOrderType.AssertHasMethod("UpdateDestination");
			taxiOrderType.AssertHasMethod("StartRide");
			taxiOrderType.AssertHasMethod("FinishRide");
		}

		[Test]
		public void CancelRide()
		{
			var taxiApi = CreateApi();
			var order = taxiApi.CreateOrderWithoutDestination("John", "Doe", "Street1", "building1");
			time = new DateTime(2000, 01, 30);
			taxiApi.Cancel(order);
			Assert.AreEqual(
				"OrderId: 0 Status: Canceled Client: John Doe Driver:  From: Street1 building1 To:  LastProgressTime: 2000-01-30 00:00:00",
				taxiApi.GetShortOrderInfo(order));
		}

		[Test]
		public void FinishRide()
		{
			var taxiApi = CreateApi();
			var order = taxiApi.CreateOrderWithoutDestination("John", "Doe", "Street1", "building1");
			taxiApi.UpdateDestination(order, "far far away", "42");
			taxiApi.AssignDriver(order, 15);
			taxiApi.StartRide(order);
			time = new DateTime(2017, 01, 20);
			taxiApi.FinishRide(order);
			Assert.AreEqual(
				"OrderId: 0 Status: Finished Client: John Doe Driver: Drive Driverson From: Street1 building1 To: far far away 42 LastProgressTime: 2017-01-20 00:00:00",
				taxiApi.GetShortOrderInfo(order));
		}

		[Test]
		public void NotBeAnemicEntity()
		{
			taxiOrderType.AssertNotAnemic();
			Assert.AreEqual(
				typeof(Entity<int>), taxiOrderType.BaseType,
				taxiOrderType.Name + " should inherit Entity<int>");
		}

		[Test]
		public void StartRide()
		{
			var taxiApi = CreateApi();
			time = new DateTime(2017, 1, 1);
			var order = taxiApi.CreateOrderWithoutDestination("John", "Doe", "Street1", "building1");
			taxiApi.AssignDriver(order, 15);
			taxiApi.UpdateDestination(order, "far", "away");
			time = new DateTime(2017, 1, 2);
			taxiApi.StartRide(order);
			Assert.AreEqual(
				"OrderId: 0 Status: InProgress Client: John Doe Driver: Drive Driverson From: Street1 building1 To: far away LastProgressTime: 2017-01-02 00:00:00",
				taxiApi.GetShortOrderInfo(order));
		}

		[Test]
		public void TaxiOrderConsistsOfValueObjects()
		{
			taxiOrderType.AssertHasPropertyOrField("ClientName", nameof(PersonName));
			taxiOrderType.AssertHasPropertyOrField("Start", nameof(Address));
			taxiOrderType.AssertHasPropertyOrField("Destination", nameof(Address));
			taxiOrderType.AssertHasPropertyOrField("Driver", "Driver");
			Assert.AreEqual(
				typeof(Entity<int>), taxiOrderType.BaseType,
				taxiOrderType.Name + " should inherit Entity<int>");
		}

		[Test]
		public void UnassignDriver()
		{
			var taxiApi = CreateApi();
			time = new DateTime(2000, 01, 01);
			var order = taxiApi.CreateOrderWithoutDestination("John", "Doe", "Street1", "building1");
			time = new DateTime(2000, 01, 30);
			taxiApi.AssignDriver(order, 15);
			taxiApi.UnassignDriver(order);
			Assert.AreEqual(
				"OrderId: 0 Status: WaitingForDriver Client: John Doe Driver:  From: Street1 building1 To:  LastProgressTime: 2000-01-01 00:00:00",
				taxiApi.GetShortOrderInfo(order));
		}

		[Test]
		public void UpdateDestination()
		{
			var taxiApi = CreateApi();
			time = new DateTime(2017, 1, 1);
			var order = taxiApi.CreateOrderWithoutDestination("John", "Doe", "Street1", "building1");
			taxiApi.UpdateDestination(order, "far", "away");
			Assert.AreEqual(
				"OrderId: 0 Status: WaitingForDriver Client: John Doe Driver:  From: Street1 building1 To: far away LastProgressTime: 2017-01-01 00:00:00",
				taxiApi.GetShortOrderInfo(order));
		}

		[Test]
		public void ThrowInformativeExceptions_OnOnvalidActions()
		{
			var taxiApi = CreateApi();
			time = new DateTime(2017, 1, 1);
			var order = taxiApi.CreateOrderWithoutDestination("John", "Doe", "Street1", "building1");
			var ex = Assert.Throws<InvalidOperationException>(() => taxiApi.UnassignDriver(order));
			StringAssert.Contains("WaitingForDriver", ex.Message, "Message should contain some usefull information for debugging purpose (at least current order status)");
		}
	}
}