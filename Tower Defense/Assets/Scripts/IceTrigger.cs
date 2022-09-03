using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class IceTrigger : MonoBehaviour
{
    public event Action<TargetPoint> Entered;
    public event Action<TargetPoint> Exited;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out TargetPoint target))
        {            
            Entered?.Invoke(target);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out TargetPoint target))
        {
            Exited?.Invoke(target);
        }
    }
}
