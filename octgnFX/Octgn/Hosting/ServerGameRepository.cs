using Octgn.Server.Data;
using Octgn.Online.Library.Models;
using Octgn.Library.Utils;

namespace Octgn.Hosting
{
    internal class ServerGameRepository : LibraryRepositoryBase<uint, IHostedGameState>, IGameRepository
    {
        public IPlayerRepository Players { get; }

        public ServerGameRepository() {
            Players = new ServerPlayerRepository();
        }
    }
}