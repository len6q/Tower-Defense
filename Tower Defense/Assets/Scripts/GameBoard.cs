using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [SerializeField] private Transform _ground;
    [SerializeField] private GameTile _tilePrefab;

    private Vector2Int _size;

    private GameTile[] _tiles;

    private Queue<GameTile> _searchFrontier = new Queue<GameTile>();

    private GameTileContentFactory _contentFactory;

    public void Initialize(Vector2Int size, GameTileContentFactory contentFactory)
    {
        _size = size;
        _contentFactory = contentFactory;

        _ground.localScale = new Vector3(_size.x, _size.y, 1);

        Vector2 offset = new Vector2((_size.x - 1) * .5f, (_size.y - 1) * .5f);

        _tiles = new GameTile[_size.x * _size.y];
        for (int i = 0, y = 0; y < _size.y; y++)
        {
            for (int x = 0; x < _size.x; x++, i++)
            {
                GameTile tile = _tiles[i] = Instantiate(_tilePrefab, transform);
                tile.transform.localPosition = new Vector3(x - offset.x, 0f, y - offset.y);
            
                if(x > 0)
                {
                    GameTile.MakeEastWestNeighbors(tile, _tiles[i - 1]);
                }

                if(y > 0)
                {
                    GameTile.MakeNorthSouthNeighbors(tile, _tiles[i - _size.x]);
                }

                tile.IsAlternative = (x & 1) == 0;
                if((y & 1) == 0)
                {
                    tile.IsAlternative = !tile.IsAlternative;
                }

                tile.Content = _contentFactory.Get(GameTileContentType.Empty);
            }
        }

        ToggleDestination(_tiles[_tiles.Length / 2]);
    }

    private bool FindPaths()
    {
        foreach(var tile in _tiles)
        {
            if(tile.Content.Type == GameTileContentType.Destination)
            {
                tile.BecomeDestination();
                _searchFrontier.Enqueue(tile);
            }
            else
            {
                tile.ClearPath();
            }
        }

        if (_searchFrontier.Count == 0)
        {
            return false;
        }

        int destinationIndex = _tiles.Length / 2;
        _tiles[destinationIndex].BecomeDestination();
        _searchFrontier.Enqueue(_tiles[destinationIndex]);

        while(_searchFrontier.Count > 0)
        {
            GameTile tile = _searchFrontier.Dequeue();

            if(tile != null)
            {
                if (tile.IsAlternative)
                {
                    _searchFrontier.Enqueue(tile.GrowPathNorth());
                    _searchFrontier.Enqueue(tile.GrowPathEast());
                    _searchFrontier.Enqueue(tile.GrowPathSouth());
                    _searchFrontier.Enqueue(tile.GrowPathWest());
                }
                else
                {
                    _searchFrontier.Enqueue(tile.GrowPathWest());
                    _searchFrontier.Enqueue(tile.GrowPathSouth());
                    _searchFrontier.Enqueue(tile.GrowPathEast());
                    _searchFrontier.Enqueue(tile.GrowPathNorth());
                }

            }
        }

        foreach(var tile in _tiles)
        {
            if(!tile.HasPath)
            {
                return false;
            }
        }

        foreach(var tile in _tiles)
        {
            tile.ShowPath();
        }

        return true;
    }

    public void ToggleDestination(GameTile tile)
    {
        if(tile.Content.Type == GameTileContentType.Destination)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Empty);
            if(!FindPaths())
            {
                tile.Content = _contentFactory.Get(GameTileContentType.Destination);
                FindPaths();
            }            
        }
        else if(tile.Content.Type == GameTileContentType.Empty)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Destination);
            FindPaths();
        }
    }

    public void ToggleWall(GameTile tile)
    {
        if (tile.Content.Type == GameTileContentType.Wall)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Empty);
            FindPaths();
        }
        else if(tile.Content.Type == GameTileContentType.Empty)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Wall);
            if(!FindPaths())
            {
                tile.Content = _contentFactory.Get(GameTileContentType.Empty);
                FindPaths();
            }            
        }
    }

    public GameTile GetTile(Ray ray)
    {
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            int x = (int)(hit.point.x + _size.x * .5f);
            int y = (int)(hit.point.z + _size.y * .5f);
            
            if(x >= 0 && x < _size.x && y >= 0 && y < _size.y)
            {
                return _tiles[x + y * _size.x];
            }
        }
        return null;
    }
}
