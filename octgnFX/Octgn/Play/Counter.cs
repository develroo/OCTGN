using System.ComponentModel;
using System.Globalization;
using Octgn.Utils;
using Octgn.Library.Utils;
using System;

namespace Octgn.Play
{
    public sealed class Counter : INotifyPropertyChanged
    {
        #region Private fields

        private readonly DataNew.Entities.Counter _defintion;
        private readonly byte _id;
        private readonly Player _player; // Player who owns this counter, if any
        private int _state; // Value of this counter

        #endregion

        #region Public interface

        // Find a counter given its Id

        // Name of this counter
        private readonly string _name;

        public Counter(Player player, DataNew.Entities.Counter def)
        {
            _player = player;
            _state = def.Start;
            _name = def.Name;
            _id = def.Id;
            _defintion = def;
            Id = Guid.NewGuid();
        }

        public Player Owner
        {
            get
            {
                return _player;
            }
        }

        public string Name
        {
            get { return _name; }
        }

        // Get or set the counter's value
        public int Value
        {
            get { return _state; }
            set { SetValue(value, Player.LocalPlayer, true, false); }
        }

        public DataNew.Entities.Counter Definition
        {
            get { return _defintion; }
        }

        public static Counter Find(Guid id)
        {
            Player p = Player.Find(id);
            Counter ret = null;
            p.Counters.TryGetValue(id, out ret);
            return ret;
        }

        // C'tor

        public override string ToString()
        {
            return (_player != null ? _player.Name + "'s " : "Global ") + Name;
        }

        #endregion

        #region Implementation



        // Get the id of this counter
        internal Guid Id { get; private set; }

        private readonly CompoundCall setCounterNetworkCompoundCall = new CompoundCall();

        // Set the counter's value
        internal void SetValue(int value, Player who, bool notifyServer, bool isScriptChange)
        {
            var oldValue = _state;
            // Check the difference with current value
            int delta = value - _state;
            if (delta == 0) return;
            // Notify the server if needed
            if (notifyServer)
            {
                setCounterNetworkCompoundCall.Call(() => Program.Client.Rpc.CounterReq(this, value, isScriptChange));
            }
            // Set the new value
            _state = value;
            OnPropertyChanged("Value");
            // Display a notification in the chat
            string deltaString = (delta > 0 ? "+" : "") + delta.ToString(CultureInfo.InvariantCulture);
            Program.GameMess.PlayerEvent(who, "sets {0} counter to {1} ({2})", this, value, deltaString);
            if (notifyServer || who != Player.LocalPlayer)
            {
                Program.GameEngine.EventProxy.OnChangeCounter_3_1_0_0(who, this, oldValue);
                Program.GameEngine.EventProxy.OnChangeCounter_3_1_0_1(who, this, oldValue);
            }
            if (notifyServer)
            {
                Program.GameEngine.EventProxy.OnCounterChanged_3_1_0_2(who, this, oldValue, isScriptChange);
            }
        }

        internal void Reset()
        {
            if (!Definition.Reset) return;
            _state = Definition.Start;
            OnPropertyChanged("Value");
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}