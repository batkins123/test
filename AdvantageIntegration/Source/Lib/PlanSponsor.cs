using System;

namespace GSC.Advantage.Integration.Data
{
	public class PlanSponsor
	{
		// private members
		
		private string m_name;
		
		public PlanSponsor(string id, string dependentNo)
		{ 
			// Replace with query populating private members exposed through public properties

			m_name = "Plan sponsor for " + id + "-" + dependentNo;
		}
		
		// Public properties

		public string name
		{
			get
			{
				return m_name;
			}
		}
	}
}
