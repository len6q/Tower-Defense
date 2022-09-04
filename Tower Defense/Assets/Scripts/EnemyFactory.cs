
public abstract class EnemyFactory : GameObjectFactory
{      
    public Enemy Get(EnemyType type)
    {
        EnemyConfig config = GetConfig(type);
        Enemy instance = CreateGameObjectInstance(config.Prefab);
        instance.OriginFactory = this;
        instance.Initialize(
            config.Scale.RandomValueInRange, 
            config.PathOffset.RandomValueInRange, 
            config.Speed.RandomValueInRange,
            config.Health.RandomValueInRange
            );

        return instance;
    }

    public void Reclaim(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }

    protected abstract EnemyConfig GetConfig(EnemyType type);
}
