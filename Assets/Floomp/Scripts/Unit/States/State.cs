public abstract class State
{
    protected Unit unit;
    public State(Unit _unit) {
        unit = _unit;
    }

    public abstract void Enter();
    public abstract void Execute();
    public abstract void Exit();
}
