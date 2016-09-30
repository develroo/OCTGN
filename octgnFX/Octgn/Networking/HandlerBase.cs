﻿/* This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */
/*
 * This file was automatically generated.
 * Do not modify, changes will get lost when the file is regenerated!
 */
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Octgn.Play;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;

namespace Octgn.Networking
{
	internal abstract class HandlerBase
	{
        internal static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IHubProxy _hub;


        internal void InitializeHub(IHubProxy hub) {
            var oldHub = System.Threading.Interlocked.Exchange(ref _hub, hub);
            if (oldHub != null) {
                // TODO unregister all event handlers
            }

            Subscription sub = null;

            sub = _hub.Subscribe(nameof(Error));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>Error(args[0].ToObject<string>()));
                        } catch(Exception ex) {
                            Log.Error("Error call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("Error task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(Kick));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>Kick(args[0].ToObject<string>()));
                        } catch(Exception ex) {
                            Log.Error("Kick call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("Kick task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(Welcome));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>Welcome(args[0].ToObject<uint>(), args[1].ToObject<int>(), args[2].ToObject<bool>()));
                        } catch(Exception ex) {
                            Log.Error("Welcome call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("Welcome task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(Settings));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>Settings(args[0].ToObject<bool>(), args[1].ToObject<bool>(), args[2].ToObject<bool>()));
                        } catch(Exception ex) {
                            Log.Error("Settings call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("Settings task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(PlayerSettings));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>PlayerSettings(Player.Find(args[0].ToObject<uint>()), args[1].ToObject<bool>(), args[2].ToObject<bool>()));
                        } catch(Exception ex) {
                            Log.Error("PlayerSettings call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("PlayerSettings task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(NewPlayer));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>NewPlayer(args[0].ToObject<uint>(), args[1].ToObject<string>(), args[2].ToObject<long>(), args[3].ToObject<bool>(), args[4].ToObject<bool>()));
                        } catch(Exception ex) {
                            Log.Error("NewPlayer call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("NewPlayer task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(Leave));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>Leave(Player.Find(args[0].ToObject<uint>())));
                        } catch(Exception ex) {
                            Log.Error("Leave call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("Leave task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(Nick));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>Nick(Player.Find(args[0].ToObject<uint>()), args[1].ToObject<string>()));
                        } catch(Exception ex) {
                            Log.Error("Nick call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("Nick task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(Start));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>Start());
                        } catch(Exception ex) {
                            Log.Error("Start call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("Start task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(Reset));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>Reset(Player.Find(args[0].ToObject<uint>())));
                        } catch(Exception ex) {
                            Log.Error("Reset call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("Reset task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(NextTurn));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>NextTurn(Player.Find(args[0].ToObject<uint>())));
                        } catch(Exception ex) {
                            Log.Error("NextTurn call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("NextTurn task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(StopTurn));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>StopTurn(Player.Find(args[0].ToObject<uint>())));
                        } catch(Exception ex) {
                            Log.Error("StopTurn call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("StopTurn task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(Chat));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>Chat(Player.Find(args[0].ToObject<uint>()), args[1].ToObject<string>()));
                        } catch(Exception ex) {
                            Log.Error("Chat call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("Chat task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(Print));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>Print(Player.Find(args[0].ToObject<uint>()), args[1].ToObject<string>()));
                        } catch(Exception ex) {
                            Log.Error("Print call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("Print task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(Random));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>Random(args[0].ToObject<int>()));
                        } catch(Exception ex) {
                            Log.Error("Random call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("Random task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(Counter));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>Counter(Player.Find(args[0].ToObject<uint>()), Play.Counter.Find(args[1].ToObject<ulong>()), args[2].ToObject<int>(), args[3].ToObject<bool>()));
                        } catch(Exception ex) {
                            Log.Error("Counter call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("Counter task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(LoadDeck));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>LoadDeck(Player.Find(args[0].ToObject<uint>()), args[1].ToObject<ulong[]>(), args[2].ToObject<Guid[]>(), args[3].ToObject<ulong[]>().Select(grp=>Group.Find(grp)).ToArray(), args[4].ToObject<string[]>(), args[5].ToObject<string>(), args[6].ToObject<bool>()));
                        } catch(Exception ex) {
                            Log.Error("LoadDeck call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("LoadDeck task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(CreateCard));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>CreateCard(Player.Find(args[0].ToObject<uint>()), args[1].ToObject<ulong[]>(), args[2].ToObject<Guid[]>(), args[3].ToObject<string[]>(), Group.Find(args[4].ToObject<ulong>())));
                        } catch(Exception ex) {
                            Log.Error("CreateCard call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("CreateCard task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(CreateCardAt));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>CreateCardAt(Player.Find(args[0].ToObject<uint>()), args[1].ToObject<ulong[]>(), args[2].ToObject<Guid[]>(), args[3].ToObject<int[]>(), args[4].ToObject<int[]>(), args[5].ToObject<bool>(), args[6].ToObject<bool>()));
                        } catch(Exception ex) {
                            Log.Error("CreateCardAt call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("CreateCardAt task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(MoveCard));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>MoveCard(Player.Find(args[0].ToObject<uint>()), args[1].ToObject<ulong[]>(), Group.Find(args[2].ToObject<ulong>()), args[3].ToObject<int[]>(), args[4].ToObject<bool[]>(), args[5].ToObject<bool>()));
                        } catch(Exception ex) {
                            Log.Error("MoveCard call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("MoveCard task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(MoveCardAt));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>MoveCardAt(Player.Find(args[0].ToObject<uint>()), args[1].ToObject<ulong[]>(), args[2].ToObject<int[]>(), args[3].ToObject<int[]>(), args[4].ToObject<int[]>(), args[5].ToObject<bool[]>(), args[6].ToObject<bool>()));
                        } catch(Exception ex) {
                            Log.Error("MoveCardAt call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("MoveCardAt task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(Peek));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>Peek(Player.Find(args[0].ToObject<uint>()), Card.Find(args[1].ToObject<ulong>())));
                        } catch(Exception ex) {
                            Log.Error("Peek call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("Peek task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(Untarget));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>Untarget(Player.Find(args[0].ToObject<uint>()), Card.Find(args[1].ToObject<ulong>()), args[2].ToObject<bool>()));
                        } catch(Exception ex) {
                            Log.Error("Untarget call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("Untarget task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(Target));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>Target(Player.Find(args[0].ToObject<uint>()), Card.Find(args[1].ToObject<ulong>()), args[2].ToObject<bool>()));
                        } catch(Exception ex) {
                            Log.Error("Target call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("Target task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(TargetArrow));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>TargetArrow(Player.Find(args[0].ToObject<uint>()), Card.Find(args[1].ToObject<ulong>()), Card.Find(args[2].ToObject<ulong>()), args[3].ToObject<bool>()));
                        } catch(Exception ex) {
                            Log.Error("TargetArrow call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("TargetArrow task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(Highlight));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>Highlight(Card.Find(args[0].ToObject<ulong>()), args[1].ToObject<Color?>()));
                        } catch(Exception ex) {
                            Log.Error("Highlight call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("Highlight task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(Turn));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>Turn(Player.Find(args[0].ToObject<uint>()), Card.Find(args[1].ToObject<ulong>()), args[2].ToObject<bool>()));
                        } catch(Exception ex) {
                            Log.Error("Turn call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("Turn task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(Rotate));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>Rotate(Player.Find(args[0].ToObject<uint>()), Card.Find(args[1].ToObject<ulong>()), args[2].ToObject<CardOrientation>()));
                        } catch(Exception ex) {
                            Log.Error("Rotate call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("Rotate task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(Shuffled));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>Shuffled(Player.Find(args[0].ToObject<uint>()), Group.Find(args[1].ToObject<ulong>()), args[2].ToObject<ulong[]>(), args[3].ToObject<short[]>()));
                        } catch(Exception ex) {
                            Log.Error("Shuffled call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("Shuffled task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(AddMarker));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>AddMarker(Player.Find(args[0].ToObject<uint>()), Card.Find(args[1].ToObject<ulong>()), args[2].ToObject<Guid>(), args[3].ToObject<string>(), args[4].ToObject<ushort>(), args[5].ToObject<ushort>(), args[6].ToObject<bool>()));
                        } catch(Exception ex) {
                            Log.Error("AddMarker call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("AddMarker task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(RemoveMarker));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>RemoveMarker(Player.Find(args[0].ToObject<uint>()), Card.Find(args[1].ToObject<ulong>()), args[2].ToObject<Guid>(), args[3].ToObject<string>(), args[4].ToObject<ushort>(), args[5].ToObject<ushort>(), args[6].ToObject<bool>()));
                        } catch(Exception ex) {
                            Log.Error("RemoveMarker call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("RemoveMarker task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(TransferMarker));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>TransferMarker(Player.Find(args[0].ToObject<uint>()), Card.Find(args[1].ToObject<ulong>()), Card.Find(args[2].ToObject<ulong>()), args[3].ToObject<Guid>(), args[4].ToObject<string>(), args[5].ToObject<ushort>(), args[6].ToObject<ushort>(), args[7].ToObject<bool>()));
                        } catch(Exception ex) {
                            Log.Error("TransferMarker call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("TransferMarker task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(PassTo));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>PassTo(Player.Find(args[0].ToObject<uint>()), ControllableObject.Find(args[1].ToObject<ulong>()), Player.Find(args[2].ToObject<uint>()), args[3].ToObject<bool>()));
                        } catch(Exception ex) {
                            Log.Error("PassTo call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("PassTo task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(TakeFrom));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>TakeFrom(ControllableObject.Find(args[0].ToObject<ulong>()), Player.Find(args[1].ToObject<uint>())));
                        } catch(Exception ex) {
                            Log.Error("TakeFrom call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("TakeFrom task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(DontTake));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>DontTake(ControllableObject.Find(args[0].ToObject<ulong>())));
                        } catch(Exception ex) {
                            Log.Error("DontTake call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("DontTake task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(FreezeCardsVisibility));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>FreezeCardsVisibility(Group.Find(args[0].ToObject<ulong>())));
                        } catch(Exception ex) {
                            Log.Error("FreezeCardsVisibility call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("FreezeCardsVisibility task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(GroupVis));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>GroupVis(Player.Find(args[0].ToObject<uint>()), Group.Find(args[1].ToObject<ulong>()), args[2].ToObject<bool>(), args[3].ToObject<bool>()));
                        } catch(Exception ex) {
                            Log.Error("GroupVis call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("GroupVis task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(GroupVisAdd));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>GroupVisAdd(Player.Find(args[0].ToObject<uint>()), Group.Find(args[1].ToObject<ulong>()), Player.Find(args[2].ToObject<uint>())));
                        } catch(Exception ex) {
                            Log.Error("GroupVisAdd call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("GroupVisAdd task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(GroupVisRemove));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>GroupVisRemove(Player.Find(args[0].ToObject<uint>()), Group.Find(args[1].ToObject<ulong>()), Player.Find(args[2].ToObject<uint>())));
                        } catch(Exception ex) {
                            Log.Error("GroupVisRemove call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("GroupVisRemove task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(LookAt));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>LookAt(Player.Find(args[0].ToObject<uint>()), args[1].ToObject<uint>(), Group.Find(args[2].ToObject<ulong>()), args[3].ToObject<bool>()));
                        } catch(Exception ex) {
                            Log.Error("LookAt call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("LookAt task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(LookAtTop));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>LookAtTop(Player.Find(args[0].ToObject<uint>()), args[1].ToObject<uint>(), Group.Find(args[2].ToObject<ulong>()), args[3].ToObject<int>(), args[4].ToObject<bool>()));
                        } catch(Exception ex) {
                            Log.Error("LookAtTop call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("LookAtTop task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(LookAtBottom));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>LookAtBottom(Player.Find(args[0].ToObject<uint>()), args[1].ToObject<uint>(), Group.Find(args[2].ToObject<ulong>()), args[3].ToObject<int>(), args[4].ToObject<bool>()));
                        } catch(Exception ex) {
                            Log.Error("LookAtBottom call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("LookAtBottom task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(StartLimited));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>StartLimited(Player.Find(args[0].ToObject<uint>()), args[1].ToObject<Guid[]>()));
                        } catch(Exception ex) {
                            Log.Error("StartLimited call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("StartLimited task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(CancelLimited));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>CancelLimited(Player.Find(args[0].ToObject<uint>())));
                        } catch(Exception ex) {
                            Log.Error("CancelLimited call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("CancelLimited task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(CardSwitchTo));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>CardSwitchTo(Player.Find(args[0].ToObject<uint>()), Card.Find(args[1].ToObject<ulong>()), args[2].ToObject<string>()));
                        } catch(Exception ex) {
                            Log.Error("CardSwitchTo call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("CardSwitchTo task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(PlayerSetGlobalVariable));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>PlayerSetGlobalVariable(Player.Find(args[0].ToObject<uint>()), args[1].ToObject<string>(), args[2].ToObject<string>(), args[3].ToObject<string>()));
                        } catch(Exception ex) {
                            Log.Error("PlayerSetGlobalVariable call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("PlayerSetGlobalVariable task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(SetGlobalVariable));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>SetGlobalVariable(args[0].ToObject<string>(), args[1].ToObject<string>(), args[2].ToObject<string>()));
                        } catch(Exception ex) {
                            Log.Error("SetGlobalVariable call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("SetGlobalVariable task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(IsTableBackgroundFlipped));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>IsTableBackgroundFlipped(args[0].ToObject<bool>()));
                        } catch(Exception ex) {
                            Log.Error("IsTableBackgroundFlipped call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("IsTableBackgroundFlipped task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(PlaySound));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>PlaySound(Player.Find(args[0].ToObject<uint>()), args[1].ToObject<string>()));
                        } catch(Exception ex) {
                            Log.Error("PlaySound call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("PlaySound task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(Ready));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>Ready(Player.Find(args[0].ToObject<uint>())));
                        } catch(Exception ex) {
                            Log.Error("Ready call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("Ready task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(PlayerState));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>PlayerState(Player.Find(args[0].ToObject<uint>()), args[1].ToObject<byte>()));
                        } catch(Exception ex) {
                            Log.Error("PlayerState call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("PlayerState task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(RemoteCall));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>RemoteCall(Player.Find(args[0].ToObject<uint>()), args[1].ToObject<string>(), args[2].ToObject<string>()));
                        } catch(Exception ex) {
                            Log.Error("RemoteCall call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("RemoteCall task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(GameStateReq));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>GameStateReq(Player.Find(args[0].ToObject<uint>())));
                        } catch(Exception ex) {
                            Log.Error("GameStateReq call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("GameStateReq task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(GameState));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>GameState(Player.Find(args[0].ToObject<uint>()), args[1].ToObject<string>()));
                        } catch(Exception ex) {
                            Log.Error("GameState call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("GameState task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(DeleteCard));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>DeleteCard(Card.Find(args[0].ToObject<ulong>()), Player.Find(args[1].ToObject<uint>())));
                        } catch(Exception ex) {
                            Log.Error("DeleteCard call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("DeleteCard task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(PlayerDisconnect));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>PlayerDisconnect(Player.Find(args[0].ToObject<uint>())));
                        } catch(Exception ex) {
                            Log.Error("PlayerDisconnect call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("PlayerDisconnect task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(AddPacks));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>AddPacks(Player.Find(args[0].ToObject<uint>()), args[1].ToObject<Guid[]>(), args[2].ToObject<bool>()));
                        } catch(Exception ex) {
                            Log.Error("AddPacks call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("AddPacks task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(AnchorCard));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>AnchorCard(Card.Find(args[0].ToObject<ulong>()), Player.Find(args[1].ToObject<uint>()), args[2].ToObject<bool>()));
                        } catch(Exception ex) {
                            Log.Error("AnchorCard call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("AnchorCard task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(SetCardProperty));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>SetCardProperty(Card.Find(args[0].ToObject<ulong>()), Player.Find(args[1].ToObject<uint>()), args[2].ToObject<string>(), args[3].ToObject<string>(), args[4].ToObject<string>()));
                        } catch(Exception ex) {
                            Log.Error("SetCardProperty call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("SetCardProperty task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(ResetCardProperties));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>ResetCardProperties(Card.Find(args[0].ToObject<ulong>()), Player.Find(args[1].ToObject<uint>())));
                        } catch(Exception ex) {
                            Log.Error("ResetCardProperties call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("ResetCardProperties task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(Filter));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>Filter(Card.Find(args[0].ToObject<ulong>()), args[1].ToObject<Color?>()));
                        } catch(Exception ex) {
                            Log.Error("Filter call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("Filter task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(SetBoard));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>SetBoard(args[0].ToObject<string>()));
                        } catch(Exception ex) {
                            Log.Error("SetBoard call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("SetBoard task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(SetPlayerColor));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>SetPlayerColor(Player.Find(args[0].ToObject<uint>()), args[1].ToObject<string>()));
                        } catch(Exception ex) {
                            Log.Error("SetPlayerColor call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("SetPlayerColor task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(SetPhase));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>SetPhase(args[0].ToObject<byte>(), args[1].ToObject<byte>()));
                        } catch(Exception ex) {
                            Log.Error("SetPhase call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("SetPhase task call failed", ex);
                }
            };

            sub = _hub.Subscribe(nameof(StopPhase));
            sub.Received += (args) => {
                try {
                    if (Program.Client == null) return;
                    Task.Run(async ()=>{
                        try {
                            if (Program.Client == null) return;
                            await Application.Current.Dispatcher.InvokeAsync(()=>StopPhase(Player.Find(args[0].ToObject<uint>()), args[1].ToObject<byte>()));
                        } catch(Exception ex) {
                            Log.Error("StopPhase call failed", ex);
                        } finally {
                            if (Program.Client != null) Program.Client.Muted = 0;
                        }
                    });
                } catch(Exception ex) {
                    Log.Error("StopPhase task call failed", ex);
                }
            };


        }
		protected abstract void Error(string msg);
		protected abstract void Kick(string reason);
		protected abstract void Welcome(uint id, int gameId, bool waitForGameState);
		protected abstract void Settings(bool twoSidedTable, bool allowSpectators, bool muteSpectators);
		protected abstract void PlayerSettings(Player playerId, bool invertedTable, bool spectator);
		protected abstract void NewPlayer(uint id, string nick, long pkey, bool tableSide, bool spectator);
		protected abstract void Leave(Player player);
		protected abstract void Nick(Player player, string nick);
		protected abstract void Start();
		protected abstract void Reset(Player player);
		protected abstract void NextTurn(Player nextPlayer);
		protected abstract void StopTurn(Player player);
		protected abstract void Chat(Player player, string text);
		protected abstract void Print(Player player, string text);
		protected abstract void Random(int result);
		protected abstract void Counter(Player player, Counter counter, int value, bool isScriptChange);
		protected abstract void LoadDeck(Player player, ulong[] id, Guid[] type, Group[] group, string[] size, string sleeve, bool limited);
		protected abstract void CreateCard(Player player, ulong[] id, Guid[] type, string[] size, Group group);
		protected abstract void CreateCardAt(Player player, ulong[] id, Guid[] modelId, int[] x, int[] y, bool faceUp, bool persist);
		protected abstract void MoveCard(Player player, ulong[] id, Group group, int[] idx, bool[] faceUp, bool isScriptMove);
		protected abstract void MoveCardAt(Player player, ulong[] id, int[] x, int[] y, int[] idx, bool[] faceUp, bool isScriptMove);
		protected abstract void Peek(Player player, Card card);
		protected abstract void Untarget(Player player, Card card, bool isScriptChange);
		protected abstract void Target(Player player, Card card, bool isScriptChange);
		protected abstract void TargetArrow(Player player, Card card, Card otherCard, bool isScriptChange);
		protected abstract void Highlight(Card card, Color? color);
		protected abstract void Turn(Player player, Card card, bool up);
		protected abstract void Rotate(Player player, Card card, CardOrientation rot);
		protected abstract void Shuffled(Player player, Group group, ulong[] card, short[] pos);
		protected abstract void AddMarker(Player player, Card card, Guid id, string name, ushort count, ushort origCount, bool isScriptChange);
		protected abstract void RemoveMarker(Player player, Card card, Guid id, string name, ushort count, ushort origCount, bool isScriptChange);
		protected abstract void TransferMarker(Player player, Card from, Card to, Guid id, string name, ushort count, ushort origCount, bool isScriptChange);
		protected abstract void PassTo(Player player, ControllableObject id, Player to, bool requested);
		protected abstract void TakeFrom(ControllableObject id, Player to);
		protected abstract void DontTake(ControllableObject id);
		protected abstract void FreezeCardsVisibility(Group group);
		protected abstract void GroupVis(Player player, Group group, bool defined, bool visible);
		protected abstract void GroupVisAdd(Player player, Group group, Player who);
		protected abstract void GroupVisRemove(Player player, Group group, Player who);
		protected abstract void LookAt(Player player, uint uid, Group group, bool look);
		protected abstract void LookAtTop(Player player, uint uid, Group group, int count, bool look);
		protected abstract void LookAtBottom(Player player, uint uid, Group group, int count, bool look);
		protected abstract void StartLimited(Player player, Guid[] packs);
		protected abstract void CancelLimited(Player player);
		protected abstract void CardSwitchTo(Player player, Card card, string alternate);
		protected abstract void PlayerSetGlobalVariable(Player player, string name, string oldval, string val);
		protected abstract void SetGlobalVariable(string name, string oldval, string val);
		protected abstract void IsTableBackgroundFlipped(bool isFlipped);
		protected abstract void PlaySound(Player player, string name);
		protected abstract void Ready(Player player);
		protected abstract void PlayerState(Player player, byte state);
		protected abstract void RemoteCall(Player player, string function, string args);
		protected abstract void GameStateReq(Player player);
		protected abstract void GameState(Player toPlayer, string state);
		protected abstract void DeleteCard(Card card, Player player);
		protected abstract void PlayerDisconnect(Player player);
		protected abstract void AddPacks(Player player, Guid[] packs, bool selfOnly);
		protected abstract void AnchorCard(Card id, Player player, bool anchor);
		protected abstract void SetCardProperty(Card id, Player player, string name, string val, string valtype);
		protected abstract void ResetCardProperties(Card id, Player player);
		protected abstract void Filter(Card card, Color? color);
		protected abstract void SetBoard(string name);
		protected abstract void SetPlayerColor(Player player, string color);
		protected abstract void SetPhase(byte phase, byte nextPhase);
		protected abstract void StopPhase(Player player, byte phase);
	}
}
