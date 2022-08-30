using UnityEngine;

[CreateAssetMenu]
public class EnemyFactory : GameObjectFactory
{
    [SerializeField] private Enemy _enemyPrefab;

    public void Reclaim(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }

    public Enemy Get()
    {
        Enemy instance = CreateGameObjectInstance(_enemyPrefab);
        instance.OriginFactory = this;
        return instance;
    }
}
