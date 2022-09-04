using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Trap : GameTileContent
{    
    protected event Action<TargetPoint> Entered;
    protected event Action<TargetPoint> Exited;
    protected event Action<TargetPoint> Stayed;

    private void OnTriggerEnter(Collider other)
    {        
        if (other.TryGetComponent(out TargetPoint target))
        {            
            Entered?.Invoke(target);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent(out TargetPoint target))
        {
            Stayed?.Invoke(target);
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
