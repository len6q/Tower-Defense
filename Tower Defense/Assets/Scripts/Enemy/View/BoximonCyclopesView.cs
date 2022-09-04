
public class BoximonCyclopesView : EnemyView
{
    public void OnDieAnimationFinished()
    {
        _enemy.Recycle();
    }
}

