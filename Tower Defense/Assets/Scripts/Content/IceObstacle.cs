using System;
using System.Collections.Generic;

public class IceObstacle : Trap
{
    private static readonly Dictionary<TargetPoint, Guid> _globalTargetStorage =
        new Dictionary<TargetPoint, Guid>();
    private readonly Dictionary<TargetPoint, Guid> _internalTargetStorage =
        new Dictionary<TargetPoint, Guid>();    

    private void Awake()
    {
        Entered += OnTargetEntered;
        Exited += OnTargetExited;
    }

    private void OnDestroy()
    {
        Entered -= OnTargetEntered;
        Exited -= OnTargetExited;
    }

    private void OnTargetEntered(TargetPoint target)
    {
        var guid = Guid.NewGuid();
        _globalTargetStorage[target] = guid;
        _internalTargetStorage[target] = guid;
        target.Enemy.SetSpeed(.5f);
    }

    private void OnTargetExited(TargetPoint target)
    {
        var guidGlobal = _globalTargetStorage[target];
        var guidinternal = _internalTargetStorage[target];
        _internalTargetStorage.Remove(target);
        if (guidGlobal != guidinternal)
            return;
        _globalTargetStorage.Remove(target);
        target.Enemy.SetSpeed(1f);
    }
}
