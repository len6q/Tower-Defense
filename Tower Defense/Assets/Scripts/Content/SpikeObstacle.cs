using System;
using UnityEngine;

public class SpikeObstacle : Trap
{
    [SerializeField, Range(25, 80f)] private float _damagePerSecond = 25f;

    private void Awake()
    {
        Stayed += OnTargetStayed;
    }

    private void OnDestroy()
    {
        Stayed -= OnTargetStayed;        
    }

    private void OnTargetStayed(TargetPoint target)
    {              
        target.Enemy.TakeDamage(_damagePerSecond * Time.deltaTime);
    }    
}
