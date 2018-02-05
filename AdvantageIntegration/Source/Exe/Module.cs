using System;
using Nancy;
using GSC.Advantage.Integration.Data;

namespace GSC.Advantage.Integration
{
	public class PlanMemberModule : NancyModule
	{
		public PlanMemberModule() : base("/planmember/")
		{
			Get["{subscriberId}/{dependentNo}"] = parameters =>
			{
				PlanMember pm;
				
				Facade.Instance.GetPlanMember(parameters.subscriberId, parameters.dependentNo, out pm);
				
				return Response.AsJson(pm);
			};
		}
	}
}