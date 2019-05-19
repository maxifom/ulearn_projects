namespace Ddd.Taxi.Domain
{
	public interface ITaxiApi<TOrder>
	{
		TOrder CreateOrderWithoutDestination(string firstName, string lastName, string street, string building);
		void UpdateDestination(TOrder order, string street, string building);
		void AssignDriver(TOrder order, int driverId);
		void UnassignDriver(TOrder order);
		void Cancel(TOrder order);
		void StartRide(TOrder order);
		void FinishRide(TOrder order);
		string GetDriverFullInfo(TOrder order);
		string GetShortOrderInfo(TOrder order);
	}
}