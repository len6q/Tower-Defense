using UnityEngine;

[CreateAssetMenu]
public class WarFactory : GameObjectFactory
{
    [SerializeField] private Shell _shellPrefab;
    [SerializeField] private Explosion _explosionPrefab;

    public Shell Shell => Get(_shellPrefab);

    public Explosion Explosion => Get(_explosionPrefab);

    public void Reclaim(WarEntity entity)
    {
        Destroy(entity.gameObject);
    }

    private T Get<T>(T prefab) where T : WarEntity
    {
        T instance = CreateGameObjectInstance(prefab);
        instance.OriginFactory = this;
        return instance;
    }

}
