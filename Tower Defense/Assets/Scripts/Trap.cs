using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Trap : GameTileContent
{    
    protected event Action<TargetPoint> Entered;
    protected event Action<TargetPoint> Exited;

    private void OnTriggerEnter(Collider other)
    {        
        if (other.TryGetComponent(out TargetPoint target))
        {            
            Entered?.Invoke(target);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out TargetPoint target))
        {
            Exited?.Invoke(target);
        }
    }
}
