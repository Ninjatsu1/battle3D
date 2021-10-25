using System;
using System.Collections.Generic;

public class StateMachine
{

    private Dictionary<IState, List<StateTransition>> _stateTransitions
    = new Dictionary<IState, List<StateTransition>>();
    private List<IState> _states = new List<IState>();
    private IState _currentState;
    
    //Add state
    public void AddState(IState state)
    {
        _states.Add(state);
    }

    //Add state transitions
    public void AddTransition(IState from, IState to, Func<bool> condition)
    {
        if (_stateTransitions.ContainsKey(from) == false)
        {
            _stateTransitions[from] = new List<StateTransition>();
        }
        _stateTransitions[from].Add(new StateTransition(from, to, condition));
    }

    //Call in monobehavior Update()
    public void Tick()
    {
        var transition = CheckForTransitions();
        if (transition != null)
        {
            SetState(transition.To);
        }
        _currentState.Tick();
    }

    //Set state
    public void SetState(IState state)
    {
        _currentState?.OnExit();
        _currentState = state;
        _currentState.OnEnter();
    }

    //Check transition
    private StateTransition CheckForTransitions()
    {
        if (_stateTransitions.ContainsKey(_currentState))
        {
            foreach (var transition in _stateTransitions[_currentState])
            {
                if (transition.Condition())
                {
                    return transition;
                }
            }
        }
        return null;
    }
}

//Define IState
public interface IState
{
    void Tick();
    void OnEnter();
    void OnExit();
}

public class StateTransition
{
    public IState From;
    public IState To;
    public Func<bool> Condition;

    public StateTransition(IState from, IState to, Func<bool> condition)
    {
        From = from;
        To = to;
        Condition = condition;
    }
}