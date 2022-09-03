using UnityEngine;
using System;

[CreateAssetMenu]
public class EnemyFactory : GameObjectFactory
{
    [Serializable]
    private class EnemyConfig
    {
        public Enemy Prefab;

        [FloatRangeSlider(.5f, 2f)] public FloatRange Scale = new FloatRange(1f);
        [FloatRangeSlider(-.4f, .4f)] public FloatRange PathOffset = new FloatRange(0f);
        [FloatRangeSlider(.2f, 5f)] public FloatRange Speed = new FloatRange(1f);
        [FloatRangeSlider(10f, 1000f)] public FloatRange Health = new FloatRange(100f);
    }

    [SerializeField] private EnemyConfig _small;
    [SerializeField] private EnemyConfig _medium;
    [SerializeField] private EnemyConfig _large;

    public void Reclaim(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }

    private EnemyConfig GetConfig(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Small: return _small;
            case EnemyType.Medium: return _medium;
            case EnemyType.Large: return _large;
        }
        return null;
    }

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
}
