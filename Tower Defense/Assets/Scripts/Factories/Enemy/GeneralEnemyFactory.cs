using UnityEngine;

[CreateAssetMenu]
public class GeneralEnemyFactory : EnemyFactory
{
    [SerializeField] private EnemyConfig _boximonCyclopes;
    [SerializeField] private EnemyConfig _boximonFiery;
    [SerializeField] private EnemyConfig _spider;

    protected override EnemyConfig GetConfig(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.BoximonCyclopes: return _boximonCyclopes;
            case EnemyType.BoximonFiery: return _boximonFiery;
            case EnemyType.Spider: return _spider;
        }
        return null;
    }
}
