using UnityEngine;

public class Enemy : GameBehavior
{
    [SerializeField] private Transform _model;
    [SerializeField] private EnemyView _view;

    public EnemyFactory OriginFactory { get; set; }

    private GameTile _tileFrom, _tileTo;    
    private Vector3 _positionFrom, _positionTo;
    private float _progress, _progressFactor;

    private Direction _direction;
    private DirectionChange _directionChange;
    private float _directionAngleFrom, _directionAngleTo;
    
    private float _pathOffset;
    private float _speed;

    public float Scale { get; private set; }
    public float Health { get; private set; }

    public void Initialize(float scale, float pathOffset, float speed, float health)
    {
        _model.localScale = new Vector3(scale, scale, scale);
        _pathOffset = pathOffset;
        _speed = speed;
        Scale = scale;
        Health = health;
        _view.Init(this);
    }

    public void SpawnOn(GameTile tile)
    {
        transform.localPosition = tile.transform.localPosition;
        
        _tileFrom = tile;
        _tileTo = tile.NextOnPath;        
        _progress = 0f;
        PrepareIntro();
    }

    private void PrepareIntro()
    {
        _positionFrom = _tileFrom.transform.localPosition;
        _positionTo = _tileTo.ExitPoint;
        _direction = _tileFrom.PathDirection;
        _directionChange = DirectionChange.None;
        _directionAngleFrom = _directionAngleTo = _direction.GetAngle();
        _model.localPosition = new Vector3(_pathOffset, 0f);

        transform.localRotation = _direction.GetRotation();
        _progressFactor = _speed * 2f;
    }

    private void PrepareOutro()
    {
        _positionTo = _tileFrom.transform.localPosition;
        _directionChange = DirectionChange.None;
        _directionAngleTo = _direction.GetAngle();
        _model.localPosition = new Vector3(_pathOffset, 0f);
        transform.localRotation = _direction.GetRotation();
        _progressFactor = _speed * 2f;
    }

    public override bool GameUpdate()
    {
        if (!_view.IsInited)
        {            
            return true;
        }

        if(Health <= 0)
        {
            DisableView();
            _view.Die();
            return false;
        }

        _progress += Time.deltaTime * _progressFactor;
        while (_progress >= 1f)
        {
            if(_tileTo == null)
            {
                Game.EnemyReachedDestination();
                Recycle();
                return false;
            }

            _progress = (_progress - 1f) / _progressFactor;
            PrepareNextState();
            _progress *= _progressFactor;
        }
        
        if(_directionChange == DirectionChange.None)
        {
            transform.localPosition = Vector3.LerpUnclamped(_positionFrom, _positionTo, _progress);            
        }
        else
        {
            float angle = Mathf.LerpUnclamped(_directionAngleFrom, _directionAngleTo, _progress);
            transform.localRotation = Quaternion.Euler(0f, angle, 0f);
        }

        return true;
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
    }

    private void PrepareNextState()
    {
        _tileFrom = _tileTo;
        _tileTo = _tileTo.NextOnPath;
        _positionFrom = _positionTo;
        if(_tileTo == null)
        {
            PrepareOutro();
        }
        _positionTo = _tileFrom.ExitPoint;
        _directionChange = _direction.GetDirectionChangeTo(_tileFrom.PathDirection);
        _direction = _tileFrom.PathDirection;
        _directionAngleFrom = _directionAngleTo;

        switch (_directionChange)
        {
            case DirectionChange.None: PrepareForward(); break;
            case DirectionChange.TurnRight: PrepareTurnRight(); break;
            case DirectionChange.TurnLeft: PrepareTurnLeft(); break;
            default: PrepareTurnAround(); break;
        }
    }

    private void PrepareForward()
    {
        transform.localRotation = _direction.GetRotation();
        _directionAngleTo = _direction.GetAngle();
        _model.localPosition = new Vector3(_pathOffset, 0f);
        _progressFactor = _speed;
    }

    private void PrepareTurnRight()
    {
        _directionAngleTo = _directionAngleFrom + 90f;
        _model.localPosition = new Vector3(_pathOffset - .5f, 0f);
        transform.localPosition = _positionFrom + _direction.GetHalfVector();
        _progressFactor = _speed / (Mathf.PI * .5f * (.5f - _pathOffset));
    }

    private void PrepareTurnLeft()
    {
        _directionAngleTo = _directionAngleFrom - 90f;
        _model.localPosition = new Vector3(_pathOffset + .5f, 0f);
        transform.localPosition = _positionFrom + _direction.GetHalfVector();
        _progressFactor = _speed / (Mathf.PI * .5f * (.5f - _pathOffset));
    }

    private void PrepareTurnAround()
    {
        _directionAngleTo = _directionAngleFrom + (_pathOffset < 0f ? 180f : -180f);
        _model.localPosition = new Vector3(_pathOffset, 0f);
        transform.localPosition = _positionFrom;
        _progressFactor = _speed / (Mathf.PI * Mathf.Max(Mathf.Abs(_pathOffset), .2f));
    }

    public override void Recycle()
    {
        OriginFactory.Reclaim(this);
    }

    private void DisableView()
    {        
        _view.GetComponent<Collider>().enabled = false;
        _view.GetComponent<TargetPoint>().IsEnabled = false;
    }
}
