
using UnityEngine;

namespace Museos.StateSystem
{
    public abstract class BaseState : IState<BaseState>
    {
        public StateMachine<BaseState> StateMachine { protected get; set; }

        public void OnEnter()
        {
            SetupScene();
        }

        public void OnExit()
        {
            CleanUpState();
        }

        protected abstract void CleanUpState();

        protected abstract void SetupScene();

        public virtual void SelectItem(Item item) { }

        public virtual void Button1() { }

        public virtual void Button2() { }
    }

    public interface IState<TState> where TState : IState<TState>
    {
        void OnEnter();

        void OnExit();

        StateMachine<TState> StateMachine { set; }
    }
}
