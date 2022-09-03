using System;
using System.Collections.Generic;
using UnityEngine;

public class IceObstacle : GameTileContent
{
    [SerializeField] private IceTrigger _iceTrigger;

    private static readonly Dictionary<TargetPoint, Guid> _globalTargetStorage =
        new Dictionary<TargetPoint, Guid>();
    private readonly Dictionary<TargetPoint, Guid> _internalTargetStorage =
        new Dictionary<TargetPoint, Guid>();

    private void Awake()
    {
        _iceTrigger.Entered += OnTargetEntered;
        _iceTrigger.Exited += OnTargetExited;
    }

    private void OnDestroy()
    {
        _iceTrigger.Entered -= OnTargetEntered;
        _iceTrigger.Exited -= OnTargetExited;
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
