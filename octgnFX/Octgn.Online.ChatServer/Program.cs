using Topshelf;

namespace Octgn.Online.ChatService
{
    static class Program
    {
        static int Main( string[] args ) {
            return (int)HostFactory.Run( cfg => cfg.Service( x => new ChatService() ).Service( x => new TestClient() ) );
        }
    }
}
