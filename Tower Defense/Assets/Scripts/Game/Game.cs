using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private GameScenario _scenario;
    [SerializeField] private Vector2Int _boardSize;
    [SerializeField] private GameBoard _board;
    [SerializeField] private Camera _camera;
    [SerializeField] private GameTileContentFactory _contentFactory;    
    [SerializeField] private WarFactory _warFactory;
    [SerializeField] private TilesBuilder _tilesBuilder;
    [SerializeField] private DefenderHud _defenderHud;

    [SerializeField, Range(5f, 30f)] private float _prepareTime = 5f;
    
    private bool _scenationInProcess;

    [SerializeField, Range(.1f, 10f)] private float _playSpeed = 1f; 

    [SerializeField, Range(0f, 100)] private int _startingPlayerHealth = 50;
    private int _playerHealth;
    private int PlayerHeath
    {
        get => _playerHealth;
        set
        {
            _playerHealth = value;
            _defenderHud.UpdatePlayerHeath(_playerHealth, _startingPlayerHealth);
        }
    }

    private static Game _instance;

    private GameBehaviorCollection _enemies = new GameBehaviorCollection();
    private GameBehaviorCollection _nonEnemies = new GameBehaviorCollection();
    
    private GameScenario.State _activeScenario;

    private const float PAUSE_TIME_SCALE = 0f;

    private void Start()
    {
        _board.Initialize(_boardSize, _contentFactory);
        _tilesBuilder.Initialize(_contentFactory, _camera, _board);
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
       
        if(_scenationInProcess)
        {
            var waves = _activeScenario.GetWaves();
            _defenderHud.UpdateScenarioWaves(waves.currentWave, waves.wavesCount);
            if (PlayerHeath <= 0)
            {
                Debug.Log("Defeated!");
                BeginNewGame();
            }

            if (_activeScenario.Progress() == false && _enemies.IsEmpty)
            {
                Debug.Log("Victory!");
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
        Cleanup();
        _tilesBuilder.Enable();
        _activeScenario = _scenario.Begin();
        _prepareRoutine = StartCoroutine(PrepareRoutine());
    }

    private void Cleanup()
    {
        _tilesBuilder.Disable();
        _scenationInProcess = false;
        if (_prepareRoutine != null)
        {
            StopCoroutine(_prepareRoutine);
        }
        PlayerHeath = _startingPlayerHealth;
        _enemies.Clear();
        _nonEnemies.Clear();
        _board.Clear();

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
        _instance.PlayerHeath -= 1;
    }

    public static void SpawnEnemy(EnemyFactory factory, EnemyType type)
    {
        GameTile spawnTile = _instance._board.GetSpawnPoint();
        Enemy enemy = factory.Get(type);
        enemy.SpawnOn(spawnTile);
        _instance._enemies.Add(enemy);
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
