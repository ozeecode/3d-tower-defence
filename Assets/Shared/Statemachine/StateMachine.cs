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
            //SetState(defaultState); //SetState methodu yerine ilk giriþi burada yapýyoruz yoksa sýnýfý çaðýracaðýmýz yerde sorun olabilir(Durumlarý ekleme problemi?)..
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
                if (_normalTransitions[i].Condition() && // geçiþ koþulu saðlanmýþ.
                    _normalTransitions[i].From != default &&  // geçiþ yapmak için önceki koþul tanýmlanmýþ mý ona bakýyoruz -- buna bakmasakta olur sanki?
                    _currentState == _normalTransitions[i].From) // geçerli state den bu koþula geçilebilir mi?
                {
                    return _normalTransitions[i]; // herþey yolundaysa geçiþ iþlemini geri gönderiyoruz.
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