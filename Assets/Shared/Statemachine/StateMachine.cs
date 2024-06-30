using System;
namespace Ozee.StateMachine
{
    public class StateMachine
    {
        private IState _currentState;
        private StateTransition[] _normalTransitions;
        private StateTransition[] _anyTransitions;
        public StateMachine(IState defaultState, StateTransition[] normalTransitions, StateTransition[] anyTransitions)
        {
            _normalTransitions = normalTransitions;
            _anyTransitions = anyTransitions;
            //SetState(defaultState); //SetState methodu yerine ilk giri�i burada yap�yoruz yoksa s�n�f� �a��raca��m�z yerde sorun olabilir(Durumlar� ekleme problemi?)..
            _currentState = defaultState;
            _currentState.OnEnter(); 
        }
        public void Tick()
        {
            StateTransition transition = GetTransition(); 
            if (transition != null)
                SetState(transition.To);  

            //_currentState?.Tick(); //currentStatein null olma durumu yok sanki??
            _currentState.Tick();
        }
        public void FixedTick()
        {
            if (_currentState != null)
                _currentState.FixedTick();
        }


        public void SetState(IState state)
        {
            
            if (state == _currentState) return;

            _currentState?.OnExit(); 

            _currentState = state; 
            _currentState.OnEnter(); 

        }
        private StateTransition GetTransition()
        {
            for (int i = 0, length = _anyTransitions.Length; i < length; i++)
            {
                if (_anyTransitions[i].Condition()) return _anyTransitions[i];
            }

            for (int i = 0, length = _normalTransitions.Length; i < length; i++)
            {
                if (_normalTransitions[i].Condition() && // ge�i� ko�ulu sa�lanm��.
                    _normalTransitions[i].From != default &&  // ge�i� yapmak i�in �nceki ko�ul tan�mlanm�� m� ona bak�yoruz -- buna bakmasakta olur sanki?
                    _currentState == _normalTransitions[i].From) // ge�erli state den bu ko�ula ge�ilebilir mi?
                {
                    return _normalTransitions[i]; // her�ey yolundaysa ge�i� i�lemini geri g�nderiyoruz.
                }
            }
            return null;
        }
    }

    public class StateTransition
    {
        public Func<bool> Condition { get; }
        public IState From { get; }
        public IState To { get; }

        public StateTransition(IState _from, IState _to, Func<bool> condition)
        {
            To = _to;
            From = _from;
            Condition = condition;
        }
    }

    
    public interface IState
    {
        void OnEnter();
        void OnExit();
        void Tick();
        void FixedTick();
    }
}