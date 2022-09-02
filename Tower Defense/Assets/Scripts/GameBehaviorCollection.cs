using System.Collections.Generic;

public class GameBehaviorCollection 
{
    private List<GameBehavior> _behaviors = new List<GameBehavior>();

    public bool IsEmpty => _behaviors.Count == 0;

    public void Add(GameBehavior behavior)
    {
        _behaviors.Add(behavior);
    }

    public void GameUpdate()
    {
        for(int i = 0; i < _behaviors.Count; i++)
        {
            if(!_behaviors[i].GameUpdate())
            {
                int lastIndex = _behaviors.Count - 1;
                _behaviors[i] = _behaviors[lastIndex];
                _behaviors.RemoveAt(lastIndex);
                i -= 1;
            }
        }
    }

    public void Clear()
    {
        foreach(var behavior in _behaviors)
        {
            behavior.Recycle();
        }
        _behaviors.Clear();
    }

}
