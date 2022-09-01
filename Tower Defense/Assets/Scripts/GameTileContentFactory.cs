using UnityEngine;

[CreateAssetMenu]
public class GameTileContentFactory : GameObjectFactory
{
    [SerializeField] private GameTileContent _emptyPrefab;
    [SerializeField] private GameTileContent _destinationPrefab;
    [SerializeField] private GameTileContent _wallPrefab;
    [SerializeField] private GameTileContent _spawnPointPrefab;    
    [SerializeField] private Tower[] _towersPrefab;    

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
        }
        return null;
    }

    public GameTileContent Get(TowerType type)
    {
        Tower prefab = _towersPrefab[(int)type];
        return Get(prefab);
    }

    private T Get<T>(T prefab) where T : GameTileContent
    {
        T instance = CreateGameObjectInstance(prefab);
        instance.OriginFactory = this;        
        return instance;
    } 
}
