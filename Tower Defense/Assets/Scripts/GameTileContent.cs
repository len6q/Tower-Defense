using UnityEngine;

public class GameTileContent : MonoBehaviour
{
    [SerializeField] private GameTileContentType _type;

    public GameTileContentType Type => _type;

    public GameTileContentFactory OriginFactory { get; set; }

    public bool IsBlockingPath => Type == GameTileContentType.Wall || Type == GameTileContentType.LaserTower || Type == GameTileContentType.MortarTower;

    public void Recycle()
    {        
        OriginFactory.Reclaim(this);
    }

    public virtual void GameUpdate()
    {

    }
}

public enum GameTileContentType
{
    Empty,
    Destination,
    Wall,
    SpawnPoint,    
    LaserTower,
    MortarTower,
    IceObstacle,
    SpikeObstacle
}
