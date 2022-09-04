using UnityEngine;

[CreateAssetMenu]
public class GameTileContentFactory : GameObjectFactory
{
    [SerializeField] private GameTileContent _emptyPrefab;
    [SerializeField] private GameTileContent _destinationPrefab;
    [SerializeField] private GameTileContent _wallPrefab;
    [SerializeField] private GameTileContent _spawnPointPrefab;    
    [SerializeField] private GameTileContent _laserTowerPrefab;  
    [SerializeField] private GameTileContent _mortarTowerPrefab;  
    [SerializeField] private GameTileContent _iceObstaclePrefab;  
    [SerializeField] private GameTileContent _spikeObstaclePrefab;      

    public void Reclaim(GameTileContent content)
    {        
        Destroy(content.gameObject);
    }

    public GameTileContent Get(GameTileContentType type)
    {
        switch (type) 
        {
            case GameTileContentType.Empty:
                return Get(_emptyPrefab);
            case GameTileContentType.Destination:
                return Get(_destinationPrefab);
            case GameTileContentType.Wall:
                return Get(_wallPrefab);
            case GameTileContentType.SpawnPoint:
                return Get(_spawnPointPrefab); 
            case GameTileContentType.LaserTower:
                return Get(_laserTowerPrefab);
            case GameTileContentType.MortarTower:
                return Get(_mortarTowerPrefab);
            case GameTileContentType.IceObstacle:
                return Get(_iceObstaclePrefab);
            case GameTileContentType.SpikeObstacle:
                return Get(_spikeObstaclePrefab);
        }
        return null;
    }

    private T Get<T>(T prefab) where T : GameTileContent
    {
        T instance = CreateGameObjectInstance(prefab);
        instance.OriginFactory = this;        
        return instance;
    } 
}
