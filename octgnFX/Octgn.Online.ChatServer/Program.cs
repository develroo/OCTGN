using System;
using System.Collections;
using System.Collections.Generic;
using Topshelf;
using Topshelf.HostConfigurators;
using Topshelf.Runtime;

namespace Octgn.Online.ChatService
{
    static class Program
    {
        static int Main(string[] args)
        {
            return (int)HostFactory.Run(Configure);
        }

        static void Configure(HostConfigurator config)
        {
            foreach(var service in GetServices())
            {
                config.Service(x => service(x));
            }
        }

        static IEnumerable<Func<HostSettings, ServiceControl>> GetServices()
        {
            yield return cfg => new ChatService("chatservice1", "chatservice1");
            yield return cfg => new ChatService("chatservice2", "chatservice2");
            yield return cfg => new TestClient("usera", "usera");
            yield return cfg => new TestClient("userb", "userb");
        }
    }
}
