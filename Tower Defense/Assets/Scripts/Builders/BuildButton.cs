using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class BuildButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameTileContentType _type;

    private event Action<GameTileContentType> _listenerAction;

    public void AddListener(Action<GameTileContentType> listenerAction)
    {
        _listenerAction = listenerAction;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _listenerAction?.Invoke(_type);
    }
}
