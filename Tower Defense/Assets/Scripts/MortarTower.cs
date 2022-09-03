using UnityEngine;

public class MortarTower : Tower
{
    [SerializeField, Range(.5f, 3f)] private float _shootsPerSeconds = 1f;
    [SerializeField] private Transform _mortar;

    [SerializeField, Range(.5f, 3f)] private float _shellBlastRadius = 1f;
    [SerializeField, Range(1f, 100f)] private float _shellDamage = 10f;

    public override TowerType Type => TowerType.Mortar;

    private readonly float _gravity = -Physics.gravity.y;
    private float _lauchSpeed;
    private float _lauchProgress;

    private void Awake()
    {
        OnValidate();
    }

    private void OnValidate()
    {
        float x = _targetingRange + .301f;
        float y = -_mortar.position.y;
        _lauchSpeed = Mathf.Sqrt(_gravity * (y + Mathf.Sqrt(x * x + y * y)));
    }

    public override void GameUpdate()
    {
        _lauchProgress += _shootsPerSeconds * Time.deltaTime;
        while(_lauchProgress >= 1f)
        {
            if(IsAcquireTarget(out TargetPoint target))
            {
                Launch(target);
                _lauchProgress -= 1f;
            }
            else
            {
                _lauchProgress = .9f;
            }
        }
    }

    private void Launch(TargetPoint target)
    {
        Vector3 launchPoint = _mortar.position;
        Vector3 targetPoint = target.Position;
        targetPoint.y = 0f;

        Vector2 direction;
        direction.x = targetPoint.x - launchPoint.x;
        direction.y = targetPoint.z - launchPoint.z;
        float x = direction.magnitude;
        float y = -launchPoint.y;
        direction /= x;
        
        float s = _lauchSpeed;
        float s2 = s * s;

        float r = s2 * s2 - _gravity * (_gravity * x * x + 2f * y * s2);        
        float tanTheta = (s2 + Mathf.Sqrt(r)) / (_gravity * x);
        float cosTheta = Mathf.Cos(Mathf.Atan(tanTheta));
        float sinTheta = cosTheta * tanTheta;

        _mortar.localRotation = Quaternion.LookRotation(new Vector3(direction.x, tanTheta, direction.y));

        Game.SpawnShell().Initialize(
            launchPoint, targetPoint,
           new Vector3(s * cosTheta * direction.x, s * sinTheta, s * cosTheta * direction.y),
           _shellBlastRadius, _shellDamage
           );        
    }
}
