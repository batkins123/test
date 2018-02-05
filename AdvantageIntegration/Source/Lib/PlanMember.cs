using System;

namespace GSC.Advantage.Integration.Data
{
	public class PlanMember
	{
		// Private members
		
		private string m_id;
		private string m_dependentNo;
		private string m_firstName;
		private string m_lastName;
		private string m_addressLine1;
		private string m_addressLine2;
		private string m_addressLine3;
		
		private PlanSponsor m_planSponsor;
		
		public PlanMember(string id, string dependentNo)
		{ 
			m_id = id;
			m_dependentNo = dependentNo;
			
			// Replace with query populating private members exposed through public properties
			
			m_firstName = "John";
			m_lastName = "Doe";
			m_addressLine1 = "123 Main St.";
			m_addressLine2 = "";
			m_addressLine3 = "";
			
			m_planSponsor = new PlanSponsor(id, dependentNo);
		}
		
		// Public properties
		
		public string Id
		{
			get
			{
				return m_id;
			}
		}
		
		public string dependentNo
		{
			get
			{
				return m_dependentNo;
			}
		}
		
		public string firstName
		{
			get
			{
				return m_firstName;
			}
		}
		
		public string lastName
		{
			get
			{
				return m_lastName;
			}
		}

		public string addressLine1
		{
			get
			{
				return m_addressLine1;
			}
		}

		public string addressLine2
		{
			get
			{
				return m_addressLine2;
			}
		}

		public string addressLine3
		{
			get
			{
				return m_addressLine3;
			}
		}
		
		public PlanSponsor planSponsor
		{
			get
			{
				return m_planSponsor;
			}
		}
	}
}
