using UnityEngine;

public class LaserTower : Tower
{
    [SerializeField] private Transform _turret;
    [SerializeField] private Transform _laserBeam;

    [SerializeField, Range(1f, 100f)] private float _damagePerSecond = 10f;

    private Vector3 _laserBeamScale;
    private Vector3 _laserBeamStartPosition;

    private TargetPoint _target;    

    private void Awake()
    {
        _laserBeamScale = _laserBeam.localScale;
        _laserBeamStartPosition = _laserBeam.localPosition;
    }

    public override void GameUpdate()
    {
        if (IsTargetTracked(ref _target) || IsAcquireTarget(out _target))
        {
            Shoot();
        }
        else
        {
            _laserBeam.localScale = Vector3.zero;
        }
    }

    private void Shoot()
    {
        var point = _target.Position;
        _turret.LookAt(point);
        _laserBeam.localRotation = _turret.localRotation;

        var distance = Vector3.Distance(_turret.position, point);
        _laserBeamScale.z = distance;
        _laserBeam.localScale = _laserBeamScale;
        _laserBeam.localPosition = _laserBeamStartPosition + 
            .5f * distance * _laserBeam.forward;

        _target.Enemy.TakeDamage(_damagePerSecond * Time.deltaTime);
    }
}
