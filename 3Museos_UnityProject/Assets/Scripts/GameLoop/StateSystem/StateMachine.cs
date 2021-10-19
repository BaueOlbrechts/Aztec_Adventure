using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Museos.StateSystem
{
    public class StateMachine<TState> where TState : IState<TState>
    {
        private Dictionary<string, TState> _states = new Dictionary<string, TState>();

        public TState CurrentState { get; internal set; }

        public void RegisterState(string name, TState state)
        {
            state.StateMachine = this;
            _states.Add(name, state);
        }

        internal void MoveTo(string name)
        {
            var state = _states[name];

            CurrentState?.OnExit();
            CurrentState = state;
            CurrentState?.OnEnter();
        }
    }

    public static class GameStates
    {
        public const string Play = "Play";
        public const string Inventory = "Inventory";
        public const string Settings = "Settings";
        public const string NoTracker = "No tracker";
    }
}
