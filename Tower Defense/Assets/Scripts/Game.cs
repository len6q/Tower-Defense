using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private GameScenario _scenario;
    [SerializeField] private Vector2Int _boardSize;
    [SerializeField] private GameBoard _board;
    [SerializeField] private Camera _main;
    [SerializeField] private GameTileContentFactory _contentFactory;    
    [SerializeField] private WarFactory _warFactory;

    [SerializeField, Range(5f, 30f)] private float _prepareTime = 5f;
    
    private bool _scenationInProcess;

    [SerializeField, Range(.1f, 10f)] private float _playSpeed = 1f; 

    [SerializeField, Range(0f, 100f)] private float _startingPlayerHealth = 50f;
    private float _playerHealth;

    private static Game _instance;

    private GameBehaviorCollection _enemies = new GameBehaviorCollection();
    private GameBehaviorCollection _nonEnemies = new GameBehaviorCollection();

    private Ray TouchRay => _main.ScreenPointToRay(Input.mousePosition);

    private GameTileContentType _currentTowerType = GameTileContentType.LaserTower;
    private GameTileContentType _currentTrapType = GameTileContentType.IceObstacle;

    private GameScenario.State _activeScenario;

    private const float PAUSE_TIME_SCALE = 0f;

    private void Start()
    {
        _board.Initialize(_boardSize, _contentFactory);
        _board.ShowPath = false;
        _board.ShowGrid = true;
        BeginNewGame();
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = Time.timeScale > PAUSE_TIME_SCALE ? PAUSE_TIME_SCALE : 1f;
        }
        else if(Time.timeScale > PAUSE_TIME_SCALE)
        {
            Time.timeScale = _playSpeed;
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            BeginNewGame();
        }

        if(Input.GetKeyDown(KeyCode.G))
        {
            _board.ShowGrid = !_board.ShowGrid;
        }

        if(Input.GetKeyDown(KeyCode.V))
        {
            _board.ShowPath = !_board.ShowPath;
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            _currentTowerType = GameTileContentType.LaserTower;
            _currentTrapType = GameTileContentType.IceObstacle;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _currentTowerType = GameTileContentType.MortarTower;
            _currentTrapType = GameTileContentType.SpikeObstacle;
        }

        if(Input.GetMouseButtonDown(0))
        {            
            HandleAnotherTouch();
        }
        else if(Input.GetMouseButtonDown(1))
        {
            HandleTouch();
        }

        if(_scenationInProcess)
        {
            if (_playerHealth <= 0f)
            {
                Debug.Log("defeat");
                BeginNewGame();
            }

            if (!_activeScenario.Progress() && _enemies.IsEmpty)
            {
                Debug.Log("victory!");
                BeginNewGame();
                _activeScenario.Progress();
            }

            _activeScenario.Progress();

            _enemies.GameUpdate();
            Physics.SyncTransforms();
            _board.GameUpdate();
            _nonEnemies.GameUpdate();
        }        
    }

    private void BeginNewGame()
    {
        _scenationInProcess = false;
        if(_prepareRoutine != null)
        {
            StopCoroutine(_prepareRoutine);
        }
        _playerHealth = _startingPlayerHealth;
        _enemies.Clear();
        _nonEnemies.Clear();
        _board.Clear();
        _activeScenario = _scenario.Begin();
        _prepareRoutine = StartCoroutine(PrepareRoutine());
    }

    private Coroutine _prepareRoutine;

    private IEnumerator PrepareRoutine()
    {
        yield return new WaitForSeconds(_prepareTime);
        _activeScenario = _scenario.Begin();
        _scenationInProcess = true;
    }

    public static void EnemyReachedDestination()
    {
        _instance._playerHealth -= 1;
    }

    public static void SpawnEnemy(EnemyFactory factory, EnemyType type)
    {
        GameTile spawnTile = _instance._board.GetSpawnPoint();
        Enemy enemy = factory.Get(type);
        enemy.SpawnOn(spawnTile);
        _instance._enemies.Add(enemy);
    }

    private void HandleTouch()
    {        
        GameTile tile = _board.GetTile(TouchRay);        
        if(tile != null)
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                _board.ToggleContent(tile, GameTileContentType.SpawnPoint);
            }
            else
            {
                _board.ToggleContent(tile, GameTileContentType.Destination);
            }            
        }
    }
    
    private void HandleAnotherTouch()
    {
        GameTile tile = _board.GetTile(TouchRay);
        if(tile != null)
        {
            if(Input.GetKey(KeyCode.I))
            {
                _board.ToggleContent(tile, _currentTrapType);
            }
            else if(Input.GetKey(KeyCode.LeftShift))
            {
                _board.ToggleContent(tile, _currentTowerType);
            }
            else
            {
                _board.ToggleContent(tile, GameTileContentType.Wall);
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
