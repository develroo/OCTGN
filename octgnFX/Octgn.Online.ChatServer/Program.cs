using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Topshelf;
using Topshelf.HostConfigurators;
using Topshelf.Runtime;

namespace Octgn.Online.ChatService
{
    static class Program
    {
        static int Main(string[] args)
        {
            return (int)HostFactory.Run(config=> {
                config.Service(x => new MultiServiceControl(GetServices()));
            });
        }

        static IEnumerable<ServiceControl> GetServices()
        {
            yield return new ChatService("chatservice1", "chatservice1");
            yield return new ChatService("chatservice2", "chatservice2");
            yield return new TestClient("usera", "usera");
            yield return new TestClient("userb", "userb");
        }
    }

    public class MultiServiceControl : ServiceControl
    {
        private readonly ServiceControl[] _services;

        public MultiServiceControl(IEnumerable<ServiceControl> services)
        {
            _services = services.ToArray();
        }

        public bool Start(HostControl hostControl)
        {
            foreach(var service in _services)
            {
                service.Start(hostControl);
            }
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            foreach(var service in _services)
            {
                service.Stop(hostControl);
            }
            return true;
        }
    }
}
