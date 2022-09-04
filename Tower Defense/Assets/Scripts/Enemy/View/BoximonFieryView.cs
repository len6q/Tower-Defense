
public class BoximonFieryView : EnemyView
{
    public void OnDieAnimationFinished()
    {
        _enemy.Recycle();
    }
}
