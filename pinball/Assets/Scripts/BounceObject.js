#pragma strict

var explosionStrength : float = 100;

function OnCollisionEnter (_other : Collision)
{
    _other.rigidbody.AddExplosionForce(explosionStrength, this.transform.position, 5);
}