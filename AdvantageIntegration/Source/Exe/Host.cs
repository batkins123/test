using System;
using Nancy.Hosting.Self;

namespace GSC.Advantage.Integration
{
	public class Host
	{
		private NancyHost host;

		public void Start()
		{
			host = new NancyHost(new Uri("http://localhost:5000"));
			
			host.Start();
		}

		public void Stop()
		{
			host.Stop();
		}
	}
}