namespace Ddd.Taxi.Domain
{
	public enum TaxiOrderStatus
	{
		WaitingForDriver,
		WaitingCarArrival,
		InProgress,
		Finished,
		Canceled
	}
}