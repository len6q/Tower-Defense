using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private Vector2Int _boardSize;
    [SerializeField] private GameBoard _board;
    [SerializeField] private Camera _main;
    [SerializeField] private GameTileContentFactory _contentFactory;
    [SerializeField] private EnemyFactory _enemyFacory;

    [SerializeField, Range(1f, 10f)] private float _spawnSpeedEnemy;
    
    private float _spawnProgress;

    private EnemyCollection _enemies = new EnemyCollection();

    private Ray TouchRay => _main.ScreenPointToRay(Input.mousePosition);    

    private void Start()
    {
        _board.Initialize(_boardSize, _contentFactory);
    }

    private void Update()
    {
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
            _board.ToggleWall(tile);
        }
    }
}
