using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private Vector2Int _boardSize;
    [SerializeField] private GameBoard _board;
    [SerializeField] private Camera _main;
    [SerializeField] private GameTileContentFactory _contentFactory;

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
    }

    private void HandleTouch()
    {        
        GameTile tile = _board.GetTile(TouchRay);        
        if(tile != null)
        {
            _board.ToggleDestination(tile);            
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
