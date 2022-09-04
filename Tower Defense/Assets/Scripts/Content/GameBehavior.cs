using UnityEngine;

public abstract class GameBehavior : MonoBehaviour
{
    public abstract void Recycle();

    public virtual bool GameUpdate() => true;
}
