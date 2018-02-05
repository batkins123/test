using System;
using Topshelf;

namespace GSC.Advantage.Integration
{
	public class Program
	{
		public static void Main()
		{
			HostFactory.Run(x => 
			{
				x.UseLinuxIfAvailable();
				
				x.Service<Host>(s => 
				{
					s.ConstructUsing(name => new Host()); 
					s.WhenStarted(tc => tc.Start()); 
					s.WhenStopped(tc => tc.Stop()); 
				});

				x.RunAsLocalSystem(); 
				
				x.SetDescription("AdvantageIntegrationService"); 
				x.SetDisplayName("Advantage Integration Service"); 
				x.SetServiceName("AdvantageIntegrationService"); 
			}); 
		}
	}
}