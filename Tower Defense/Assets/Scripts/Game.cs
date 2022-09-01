using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private Vector2Int _boardSize;
    [SerializeField] private GameBoard _board;
    [SerializeField] private Camera _main;
    [SerializeField] private GameTileContentFactory _contentFactory;
    [SerializeField] private EnemyFactory _enemyFacory;
    [SerializeField] private WarFactory _warFactory;

    private static Game _instance;

    [SerializeField, Range(1f, 10f)] private float _spawnSpeedEnemy;
    
    private float _spawnProgress;

    private GameBehaviorCollection _enemies = new GameBehaviorCollection();
    private GameBehaviorCollection _nonEnemies = new GameBehaviorCollection();

    private Ray TouchRay => _main.ScreenPointToRay(Input.mousePosition);

    private TowerType _currentTowerType;

    private void Start()
    {
        _board.Initialize(_boardSize, _contentFactory);
    }

    private void OnEnable()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            _currentTowerType = TowerType.Laser;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _currentTowerType = TowerType.Mortar;
        }

        if(Input.GetMouseButtonDown(0))
        {            
            HandleAnotherTouch();
        }
        else if(Input.GetMouseButtonDown(1))
        {
            HandleTouch();
        }

        _spawnProgress += _spawnSpeedEnemy * Time.deltaTime;
        while(_spawnProgress >= 1f)
        {
            _spawnProgress -= 1f;
            SpawnEnemy();
        }

        _enemies.GameUpdate();
        Physics.SyncTransforms();
        _board.GameUpdate();
        _nonEnemies.GameUpdate();
    }

    private void SpawnEnemy()
    {
        GameTile spawnTile = _board.GetSpawnPoint(Random.Range(0, _board.SpawnPointCount));
        Enemy enemy = _enemyFacory.Get();
        enemy.SpawnOn(spawnTile);
        _enemies.Add(enemy);
    }

    private void HandleTouch()
    {        
        GameTile tile = _board.GetTile(TouchRay);        
        if(tile != null)
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                _board.ToggleSpawnPoint(tile);
            }
            else
            {
                _board.ToggleDestination(tile);
            }            
        }
    }
    
    private void HandleAnotherTouch()
    {
        GameTile tile = _board.GetTile(TouchRay);
        if(tile != null)
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                _board.ToggleTower(tile, _currentTowerType);
            }
            else
            {
                _board.ToggleWall(tile);
            }            
        }
    }

    public static Shell SpawnShell()
    {
        Shell shell = _instance._warFactory.Shell;
        _instance._nonEnemies.Add(shell);
        return shell;
    }

    public static Explosion SpawnExplosion()
    {
        Explosion explosion = _instance._warFactory.Explosion;
        _instance._nonEnemies.Add(explosion);
        return explosion;
    }
}
