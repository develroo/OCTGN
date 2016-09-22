/* This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using Octgn.Library.Localization;
using Octgn.Site.Api;
using Octgn.Site.Api.Models;
using System.Runtime.CompilerServices;

namespace Octgn.Server
{
    public sealed class RequestHandler : IRemoteCalls
    {
        internal static ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [ThreadStatic]
        private static RequestContext Context;

        private static readonly bool IsDebug;
        private const string ServerName = "OCTGN.NET";
        private static readonly Version ServerVersion = GetServerVersion(); //unused
        private static Version GetServerVersion()
        {
            var asm = typeof(Game).Assembly;
            return asm.GetName().Version;
        }

        static RequestHandler() {
#if (DEBUG)
            IsDebug = true;
#endif
        }

        // C'tor
        public RequestHandler()
        {
        }

        public bool InitializeRequest(RequestContext context, [CallerMemberName] string request = null) {
            Context = context;
            // Check if this is the first message received
            if (!Context.Sender.SaidHello)
            {
                var acceptableMessages = new string[]
                    {
                        nameof(IRemoteCalls.Hello),
                        nameof(IRemoteCalls.HelloAgain),
                    };
                //TODO Maybe we shouldn't kill the connection here
                //     Basically, if someone dc's it's possible that
                //     a network call gets sent up on accident before HelloAgain,
                //     which effectivly kills the game.
                //     Maybe need a flag on the player saying they at least said
                //     hello once.

                // A new connection must always start with a hello message, send error message back
                if (!acceptableMessages.Contains(request))
                {
                    Context.Sender.Rpc.Error(L.D.ServerMessage__FailedToSendHelloMessage);
                    return false;
                }
            }
            return true;
        }

        #region IRemoteCalls interface

        public void Binary()
        {
            /* This never gets called. This message gets special treatment in the server. */
        }

        public void Error(string msg)
        {
            Debug.WriteLine(msg);
        }

        public void Start()
        {
            Context.Game.Status = Online.Library.Enums.EnumHostedGameStatus.GameStarted;
            Context.Game.AcceptingPlayers = false;
            Context.Broadcaster.Start();
            // Just a precaution, shouldn't happen though.
            if (!Context.Game.Spectators)
            {
                foreach (var p in Context.Game.Players.Cast<Player>())
                {
                    if (p.State == Online.Library.Enums.EnumPlayerState.Spectating)
                        p.Kick(false, L.D.ServerMessage__SpectatorsNotAllowed);
                }
            }
            if (!Context.Settings.IsLocalGame)
            {
                try
                {
                    var c = new ApiClient();
                    var req = new PutGameHistoryReq(Context.Settings.ApiKey, Context.Game.Id.ToString());
                    foreach (var p in Context.Game.Players.ToArray())
                    {
                        req.Usernames.Add(p.Name);
                    }
                    c.CreateGameHistory(req);
                }
                catch (Exception e)
                {
                    Log.Error("Start Error creating Game History", e);
                }
            }
        }

        public void Settings(bool twoSidedTable, bool allowSpectators, bool muteSpectators)
        {
            if (Context.Sender.Id != 1) return;

            if (Context.Game.Status == Online.Library.Enums.EnumHostedGameStatus.GameStarted)
                twoSidedTable = Context.Game.TwoSidedTable; // Can't change this after the game started.
            else Context.Game.TwoSidedTable = twoSidedTable;

            Context.Game.Spectators = allowSpectators;
            Context.Game.MuteSpectators = muteSpectators;
            Context.Game.Spectators = allowSpectators;
            Context.Broadcaster.Settings(twoSidedTable, allowSpectators, muteSpectators);
        }

        public void PlayerSettings(ulong player, bool invertedTable, bool spectator)
        {
            if (Context.Game.Status == Online.Library.Enums.EnumHostedGameStatus.GameStarted) return;

            // The player may have left the game concurrently
            Player p = (Player)Context.Game.Players.FirstOrDefault(x => x.Id == player);
            if (p == null) return;
            if (p.InvertedTable != invertedTable || p.State != Online.Library.Enums.EnumPlayerState.Spectating)
            {
                p.InvertedTable = invertedTable;
                p.State = spectator ? Online.Library.Enums.EnumPlayerState.Spectating
                    : Online.Library.Enums.EnumPlayerState.Playing;
                Context.Broadcaster.PlayerSettings(player, invertedTable, spectator);
            }
        }

        public void ResetReq()
        {
            Context.Game.CurrentTurnNumber = 0;
            Context.Game.TurnStopPlayers.Clear();
            Context.Game.PhaseStopPlayers.Clear();
            Context.Broadcaster.Reset(Context.Sender.Id);
        }

        public void ChatReq(string text)
        {
            if (Context.Sender.State == Online.Library.Enums.EnumPlayerState.Spectating && Context.Game.MuteSpectators)
            {
                Context.Sender.Rpc.Error(L.D.ServerMessage__SpectatorsMuted);
                return;
            }
            Context.Broadcaster.Chat(Context.Sender.Id, text);
            if (Context.Settings.IsLocalGame) return;
            var mess = new GameMessage();
            // don't send if we join our own room...that'd be annoying
            mess.Message = string.Format("{0} has left your game", Context.Sender.Name);
            mess.Sent = DateTime.Now;
            mess.SessionId = Context.Game.Id;
            mess.Type = GameMessageType.Chat;
        }

        public void PrintReq(string text)
        {
            if (Context.Sender.State == Online.Library.Enums.EnumPlayerState.Spectating && Context.Game.MuteSpectators)
            {
                Context.Sender.Rpc.Error(L.D.ServerMessage__SpectatorsMuted);
                return;
            }
            Context.Broadcaster.Print(Context.Sender.Id, text);
        }

		private Random rnd = new Random();
        public void RandomReq(int min, int max)
        {
            var result = rnd.Next(min, max + 1);
            Context.Sender.Rpc.Random(result);
        }


        public void CounterReq(int counter, int value, bool isScriptChange)
        {
            Context.Broadcaster.Counter(Context.Sender.Id, counter, value, isScriptChange);
        }

        private bool ValidateHello(string nick, ulong pkey, string client, Version clientVer, Version octgnVer, Guid lGameId,
                          Version gameVer, string password, bool spectator)
        {
            if (Context.Game.KickedPlayers.Any(x=>x.PublicKey == pkey))
            {
                ErrorAndCloseConnection(L.D.ServerMessage__SpectatorsMuted);
                return false;
            }
            // One should say Hello only once
            if (Context.Sender.SaidHello)
            {
                ErrorAndCloseConnection(L.D.ServerMessage__SayHelloOnlyOnce);
                return false;
            }

            // Verify password
            if (!string.IsNullOrWhiteSpace(Context.Game.Password))
            {
                if (!password.Equals(Context.Game.Password))
                {
                    ErrorAndCloseConnection(L.D.ServerMessage__IncorrectPassword);
                    return false;
                }
            }
            // Check if the versions are compatible
            if (!IsDebug) {
                if (clientVer.CompareTo(ServerVersion) < 0)
                //if ((clientVer.Major != ServerVersion.Major || clientVer.Minor != ServerVersion.Minor))
                {
                    ErrorAndCloseConnection(string.Format(L.D.ServerMessage__IncompatibleOctgnClient_Format,
                                            ServerVersion, clientVer));
                    return false;
                }
            }
            // Check if the client wants to play the correct game
            if (lGameId != Context.Game.GameId)
            {
                ErrorAndCloseConnection(L.D.ServerMessage__InvalidGame_Format, Context.Game.GameName);
                return false;
            }
            // Check if the client's major game version matches ours
            if (gameVer.Major != Context.Game.GameVersion.Major)
            {
                ErrorAndCloseConnection(
                    L.D.ServerMessage__IncompatibleGameVersion_Format,
                    Context.Game.GameVersion);
                return false;
            }
            return true;
        }

        private void ErrorAndCloseConnection(string message, params object[] args)
        {
            Context.Sender.Kick(false, message, args);
        }

        public void Hello(string nick, ulong pkey, string client, Version clientVer, Version octgnVer, Guid lGameId,
                          Version gameVer, string password, bool spectator)
        {
            if (!ValidateHello(nick, pkey, client, clientVer, octgnVer, lGameId, gameVer, password, spectator)) return;
            if (spectator && !Context.Game.Spectators)
            {
                ErrorAndCloseConnection(L.D.ServerMessage__SpectatorsNotAllowed);
                return;
            }
            // Check if we accept new players
            if (!Context.Game.AcceptingPlayers && !spectator)
            {
                ErrorAndCloseConnection(L.D.ServerMessage__GameStartedNotAcceptingNewPlayers);
                return;
            }
            // Create the new endpoint
            string software = client + " (" + clientVer + ')';
            Context.Sender.Setup(nick, pkey, Context.Sender.Rpc, spectator);

            // decide players side of table; before saying hello so new player not included
            short aPlayers = (short)Context.Game.Players.Count(x => !x.InvertedTable);
            short bPlayers = (short)Context.Game.Players.Count(x => x.InvertedTable);
            if (aPlayers > bPlayers) Context.Sender.InvertedTable = true;
            if (spectator)
                Context.Sender.InvertedTable = false;

            Context.Sender.SaidHello = true;
            // Welcome newcomer and asign them their side
            Context.Sender.Rpc.Welcome(Context.Sender.Id, Context.Game.Id, Context.Game.Status == Online.Library.Enums.EnumHostedGameStatus.GameStarted);
            Context.Sender.Rpc.PlayerSettings(Context.Sender.Id, Context.Sender.InvertedTable, Context.Sender.State == Online.Library.Enums.EnumPlayerState.Spectating);
            // Notify everybody of the newcomer
            Context.Broadcaster.NewPlayer(Context.Sender.Id, nick, pkey, Context.Sender.InvertedTable, spectator);
            // Add everybody to the newcomer
            foreach (Player player in Context.Game.Players.Where(x => x.Id != Context.Sender.Id))
                Context.Sender.Rpc.NewPlayer(player.Id, player.Name, player.PublicKey, player.InvertedTable, player.State == Online.Library.Enums.EnumPlayerState.Spectating);
            // Notify the newcomer of table sides
            Context.Sender.Rpc.Settings(Context.Game.TwoSidedTable, Context.Game.Spectators, Context.Game.MuteSpectators);
            if (Context.Game.Status == Online.Library.Enums.EnumHostedGameStatus.GameStarted)
            {
                Context.Sender.Rpc.Start();
            }
            else
            {
                if (Context.Settings.IsLocalGame) return;
                var mess = new GameMessage();
                // don't send if we join our own room...that'd be annoying
                if (nick.Equals(Context.Game.HostUserName, StringComparison.InvariantCultureIgnoreCase)) return;
                mess.Message = string.Format("{0} has joined your game", nick);
                mess.Sent = DateTime.Now;
                mess.SessionId = Context.Game.Id;
                mess.Type = GameMessageType.Event;
                new Octgn.Site.Api.ApiClient().GameMessage(Context.Settings.ApiKey, mess);
            }
        }

        public void HelloAgain(ulong pid, string nick, ulong pkey, string client, Version clientVer, Version octgnVer, Guid lGameId, Version gameVer, string password)
        {
            if (!ValidateHello(nick, pkey, client, clientVer, octgnVer, lGameId, gameVer, password, false)) return;

            // Make sure the pkey matches the pkey for the pid
            if (Context.Sender.PublicKey != pkey)
            {
                ErrorAndCloseConnection(L.D.ServerMessage__PublicKeyDoesNotMatch);
                return;
            }

            string software = client + " (" + clientVer + ')';

            Context.Sender.SaidHello = true;
            // welcome the player and assign them their side
            Context.Sender.Rpc.Welcome(Context.Sender.Id, Context.Game.Id, true);
            Context.Sender.Rpc.PlayerSettings(Context.Sender.Id, Context.Sender.InvertedTable, Context.Sender.State == Online.Library.Enums.EnumPlayerState.Spectating);
            // Notify everybody of the newcomer
            Context.Broadcaster.NewPlayer(Context.Sender.Id, nick, pkey, Context.Sender.InvertedTable, Context.Sender.State == Online.Library.Enums.EnumPlayerState.Spectating);
            // Add everybody to the newcomer
            foreach (Player player in Context.Game.Players.Where(x => x.Id != Context.Sender.Id))
                Context.Sender.Rpc.NewPlayer(player.Id, player.Name, player.PublicKey, player.InvertedTable, player.State == Online.Library.Enums.EnumPlayerState.Spectating);
            // Notify the newcomer of some shared settings
            Context.Sender.Rpc.Settings(Context.Game.TwoSidedTable, Context.Game.Spectators, Context.Game.MuteSpectators);
            foreach (Player player in Context.Game.Players)
                Context.Sender.Rpc.PlayerSettings(player.Id, player.InvertedTable, player.State == Online.Library.Enums.EnumPlayerState.Spectating);
            // Add it to our lists
            Context.Sender.Connected = true;
            Context.Game.PlayerReconnected(Context.Sender.Id);
        }

        public void LoadDeck(int[] id, Guid[] type, int[] @group, string[] size, string sleeveString, bool limited)        {
            short s = Context.Sender.Id;
            for (int i = 0; i < id.Length; i++)
                id[i] = s << 16 | (id[i] & 0xffff);

            var sstring = "";
            try
            {
                var split = sleeveString.Split(new char[1] { '\t' }, 2);
                if (split.Length == 2)
                {
                    var sid = 0;
                    if (int.TryParse(split[0], out sid))
                    {
                        Uri url = null;
                        if (Uri.TryCreate(split[1], UriKind.Absolute, out url))
                        {
                            if (!Context.Settings.IsLocalGame)
                            {
                                // Check if the user can even do this
                                var c = new ApiClient();
                                var resp = c.CanUseSleeve(Context.Sender.Name, sid);
                                if (resp.Authorized)
                                {
                                    sstring = split[1];
                                }
                            }
                            else
                            {
                                sstring = split[1];
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                if (Context.Settings.IsLocalGame)
                    Log.Warn(nameof(LoadDeck), e);
                else
                    Log.Error(nameof(LoadDeck), e);
            }
            Context.Broadcaster.LoadDeck(id, type, group, size, sstring, limited);
        }

        public void CreateCard(int[] id, Guid[] type, string[] size, int @group)        {
            short s = Context.Sender.Id;
            for (int i = 0; i < id.Length; i++)
                id[i] = s << 16 | (id[i] & 0xffff);
            Context.Broadcaster.CreateCard(id, type, size, group);
        }

        public void CreateCardAt(int[] id, Guid[] modelId, int[] x, int[] y, bool faceUp, bool persist)
        {
            short s = Context.Sender.Id;
            for (int i = 0; i < id.Length; i++)
                id[i] = s << 16 | (id[i] & 0xffff);
            Context.Broadcaster.CreateCardAt(id, modelId, x, y, faceUp, persist);
        }

        public void NextTurn(ulong nextPlayer)
        {
            if (Context.Game.TurnStopPlayers.Count > 0)
            {
                var stopPlayerId = Context.Game.TurnStopPlayers.First();
                Context.Game.TurnStopPlayers.Remove(stopPlayerId);
                Context.Broadcaster.StopTurn(stopPlayerId);
                return;
            }
            Context.Game.CurrentTurnNumber++;
            Context.Game.PhaseStopPlayers.Clear();
            Context.Broadcaster.NextTurn(nextPlayer);
        }

        public void PlayerSetGlobalVariable(ulong p, string name, string oldvalue, string value)
        {
            Context.Broadcaster.PlayerSetGlobalVariable(p, name, oldvalue, value);
        }

        public void SetGlobalVariable(string name, string oldvalue, string value)
        {
            Context.Broadcaster.SetGlobalVariable(name, oldvalue, value);
        }

        public void StopTurnReq(int lTurnNumber, bool stop)
        {
            if (lTurnNumber != Context.Game.CurrentTurnNumber) return; // Message StopTurn crossed a NextTurn message
            var id = Context.Sender.Id;
            if (stop)
                Context.Game.TurnStopPlayers.Add(id);
            else
                Context.Game.TurnStopPlayers.Remove(id);
        }

        public void CardSwitchTo(ulong uid, int c, string alternate)
        {
            Context.Broadcaster.CardSwitchTo(uid, c, alternate);
        }

        public void MoveCardReq(int[] id, int to, int[] idx, bool[] faceUp, bool isScriptMove)
        {
            Context.Broadcaster.MoveCard(Context.Sender.Id, id, to, idx, faceUp, isScriptMove);
        }

        public void MoveCardAtReq(int[] card, int[] x, int[] y, int[] idx, bool isScriptMove, bool[] faceUp)
        {
            Context.Broadcaster.MoveCardAt(Context.Sender.Id, card, x, y, idx, faceUp, isScriptMove);
        }

        public void AddMarkerReq(int card, Guid id, string name, ushort count, ushort oldCount, bool isScriptChange)
        {
            Context.Broadcaster.AddMarker(Context.Sender.Id, card, id, name, count, oldCount, isScriptChange);
        }

        public void RemoveMarkerReq(int card, Guid id, string name, ushort count, ushort oldCount, bool isScriptChange)
        {
            Context.Broadcaster.RemoveMarker(Context.Sender.Id, card, id, name, count, oldCount, isScriptChange);
        }

        public void TransferMarkerReq(int from, int to, Guid id, string name, ushort count, ushort oldCount, bool isScriptChange)
        {
            Context.Broadcaster.TransferMarker(Context.Sender.Id, from, to, id, name, count, oldCount, isScriptChange);
        }

        public void NickReq(string nick)
        {
            Context.Sender.Name= nick;
            Context.Broadcaster.Nick(Context.Sender.Id, nick);
        }

        public void PeekReq(int card)
        {
            Context.Broadcaster.Peek(Context.Sender.Id, card);
        }

        public void UntargetReq(int card, bool isScriptChange)
        {
            Context.Broadcaster.Untarget(Context.Sender.Id, card, isScriptChange);
        }

        public void TargetReq(int card, bool isScriptChange)
        {
            Context.Broadcaster.Target(Context.Sender.Id, card, isScriptChange);
        }

        public void TargetArrowReq(int card, int otherCard, bool isScriptChange)
        {
            Context.Broadcaster.TargetArrow(Context.Sender.Id, card, otherCard, isScriptChange);
        }

        public void Highlight(int card, string color)
        {
            Context.Broadcaster.Highlight(card, color);
        }

        public void Filter(int card, string color)
        {
            Context.Broadcaster.Filter(card, color);
        }

        public void TurnReq(int card, bool up)
        {
            Context.Broadcaster.Turn(Context.Sender.Id, card, up);
        }

        public void RotateReq(int card, CardOrientation rot)
        {
            Context.Broadcaster.Rotate(Context.Sender.Id, card, rot);
        }

        public void Shuffled(ulong player, int group, int[] card, short[] pos)
        {
            Context.Broadcaster.Shuffled(player, group, card, pos);
        }

        public void PassToReq(int id, ulong player, bool requested)
        {
            Context.Broadcaster.PassTo(Context.Sender.Id, id, player, requested);
        }

        public void TakeFromReq(int id, ulong fromPlayer)
        {
            Context.Game.GetPlayer(fromPlayer).Rpc.TakeFrom(id, Context.Sender.Id);
        }

        public void DontTakeReq(int id, ulong toPlayer)
        {
            Context.Game.GetPlayer(toPlayer).Rpc.DontTake(id);
        }

        public void FreezeCardsVisibility(int group)
        {
            Context.Broadcaster.FreezeCardsVisibility(group);
        }

        public void GroupVisReq(int id, bool defined, bool visible)
        {
            Context.Broadcaster.GroupVis(Context.Sender.Id, id, defined, visible);
        }

        public void GroupVisAddReq(int gId, ulong pId)
        {
            Context.Broadcaster.GroupVisAdd(Context.Sender.Id, gId, pId);
        }

        public void GroupVisRemoveReq(int gId, ulong pId)
        {
            Context.Broadcaster.GroupVisRemove(Context.Sender.Id, gId, pId);
        }

        public void LookAtReq(int uid, int gId, bool look)
        {
            Context.Broadcaster.LookAt(Context.Sender.Id, uid, gId, look);
        }

        public void LookAtTopReq(int uid, int gId, int count, bool look)
        {
            Context.Broadcaster.LookAtTop(Context.Sender.Id, uid, gId, count, look);
        }

        public void LookAtBottomReq(int uid, int gId, int count, bool look)
        {
            Context.Broadcaster.LookAtBottom(Context.Sender.Id, uid, gId, count, look);
        }

        public void StartLimitedReq(Guid[] packs)
        {
            Context.Broadcaster.StartLimited(Context.Sender.Id, packs);
        }

        public void CancelLimitedReq()
        {
            Context.Broadcaster.CancelLimited(Context.Sender.Id);
        }

        public void AddPacksReq(Guid[] packs, bool selfOnly)
        {
            Context.Broadcaster.AddPacks(Context.Sender.Id, packs, selfOnly);
        }
        public void IsTableBackgroundFlipped(bool isFlipped)
        {
            Context.Broadcaster.IsTableBackgroundFlipped(isFlipped);
        }

        #endregion IRemoteCalls interface

        public void PlaySound(ulong player, string soundName)
        {
            Context.Broadcaster.PlaySound(player, soundName);
        }

        public void Ready(ulong player)
        {
            Context.Broadcaster.Ready(player);
        }

        public void RemoteCall(ulong player, string func, string args)
        {
            Context.Game.GetPlayer(player).Rpc.RemoteCall(Context.Sender.Id, func, args);
        }

        public void GameState(ulong player, string state)
        {
            Context.Game.GetPlayer(player).Rpc.GameState(Context.Sender.Id, state);
        }

        public void GameStateReq(ulong toPlayer)
        {
            Context.Game.GetPlayer(toPlayer).Rpc.GameStateReq(Context.Sender.Id);
        }

        public void DeleteCard(int cardId, ulong playerId)
        {
            Context.Broadcaster.DeleteCard(cardId, playerId);
        }

        public void Leave(ulong player)
        {
            Context.Sender.Connected = false;
            // Notify everybody that the player has left the game
            Context.Broadcaster.Leave(Context.Sender.Id);
            if (Context.Settings.IsLocalGame) return;
            var mess = new GameMessage();
            // don't send if we join our own room...that'd be annoying
            if (Context.Sender.Name.Equals(Context.Game.HostUserName, StringComparison.InvariantCultureIgnoreCase)) return;
            mess.Message = string.Format("{0} has left your game", Context.Sender.Name);
            mess.Sent = DateTime.Now;
            mess.SessionId = Context.Game.Id;
            mess.Type = GameMessageType.Event;
            new Octgn.Site.Api.ApiClient().GameMessage(Context.Settings.ApiKey, mess);
        }

        public void Boot(ulong player, string reason)
        {
            var p = Context.Sender;
            var bplayer = Context.Game.GetPlayer(player);
            if (bplayer == null)
            {
                Context.Broadcaster.Error(string.Format(L.D.ServerMessage__CanNotBootPlayerDoesNotExist, p.Name));
                return;
            }
            if (p.Id != 1)
            {
                Context.Broadcaster.Error(string.Format(L.D.ServerMessage__CanNotBootNotHost, p.Name, bplayer.Name));
                return;
            }
            Context.Game.KickPlayer(bplayer.Id, reason);
            Context.Broadcaster.Leave(bplayer.Id);
        }

        internal void AnchorCard(int card, ulong player, bool anchor)
        {
            Context.Broadcaster.AnchorCard(card, player, anchor);
        }

        internal void SetCardProperty(int card, ulong player, string name, string val, string valtype)
        {
            Context.Broadcaster.SetCardProperty(card, player, name, val, valtype);
        }

        public void ResetCardProperties(int card, ulong player)
        {
            Context.Broadcaster.ResetCardProperties(card, player);
        }

        public void SetBoard(string name)
        {
            Context.Broadcaster.SetBoard(name);
        }

	    public void SetPlayerColor(ulong player, string colorHex)
	    {
            Context.Broadcaster.SetPlayerColor(player, colorHex);
	    }

        public void SetPhase(byte phase, byte nextPhase)
        {
            var stopPlayers = Context.Game.PhaseStopPlayers.Where(x => x.Item2 == phase).ToList();
            if (stopPlayers.Count > 0)
            {
                var stopPlayer = stopPlayers.First();
                Context.Game.PhaseStopPlayers.Remove(stopPlayer);
                Context.Broadcaster.StopPhase(stopPlayer.Item1, stopPlayer.Item2);
                return;
            }
            Context.Broadcaster.SetPhase(phase, nextPhase);
        }

        public void StopPhaseReq(int lTurnNumber, byte phase, bool stop)
        {
            if (lTurnNumber != Context.Game.CurrentTurnNumber) return; // Message StopTurn crossed a NextTurn message
            var tuple = new Tuple<ulong, byte>(Context.Sender.Id, phase);
            if (stop)
                if (!Context.Game.PhaseStopPlayers.Contains(tuple)) Context.Game.PhaseStopPlayers.Add(tuple);
            else
                if (Context.Game.PhaseStopPlayers.Contains(tuple)) Context.Game.PhaseStopPlayers.Remove(tuple);
        }

        public void SwitchWithAlternate() {
            throw new NotImplementedException();
        }
    }
}