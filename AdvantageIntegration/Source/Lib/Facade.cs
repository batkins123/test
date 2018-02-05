using System;
using GSC.Advantage.Integration.Data;

namespace GSC.Advantage.Integration
{
	public sealed class Facade
	{
		#region Singleton pattern

		private Facade()
		{ }
		
		private static readonly Lazy<Facade> instance = new Lazy<Facade>(() => new Facade());
		
		public static Facade Instance
		{
			get
			{
				return instance.Value;
			}
		}
		
		public void GetPlanMember(string planMemberId, string dependentNo, out PlanMember pm)
		{
			pm = new PlanMember(planMemberId, dependentNo);
		}
		
		#endregion
	}
}