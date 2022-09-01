using UnityEngine;

public class Shell : WarEntity
{
    private readonly float _gravity = -Physics.gravity.y;

    private Vector3 _launchPoint, _targetPoint, _launchVelocity;

    private float _age;
    private float _blastRadius, _damage;

    public void Initialize(Vector3 launchPoint, Vector3 targetPoint, Vector3 launchVelocity,
        float blastRadius, float damage)
    {
        _launchPoint = launchPoint;
        _targetPoint = targetPoint;
        _launchVelocity = launchVelocity;
        _blastRadius = blastRadius;
        _damage = damage;
    }

    public override bool GameUpdate()
    {
        _age += Time.deltaTime;

        Vector3 position = _launchPoint + _launchVelocity * _age;
        position.y -= .5f * _gravity * _age * _age;

        if(position.y <= 0f)
        {
            Game.SpawnExplosion().Initialize(_targetPoint, _blastRadius, _damage);
            OriginFactory.Reclaim(this);
            return false;
        }

        transform.localPosition = position;

        Vector3 direction = _launchVelocity;
        direction.y -= _gravity * _age;
        transform.localRotation = Quaternion.LookRotation(direction);

        Game.SpawnExplosion().Initialize(position, .1f);
        return true;
    }
}
