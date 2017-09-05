#pragma strict

var ball : Transform;

private var startingPos : Vector3;

function Start() {
	startingPos = ball.position;
}

function OnCollisionEnter(_other : Collision) {

	if(_other.gameObject.tag == "Ball") {
		Debug.Log("hello");
		_other.rigidbody.velocity = Vector3.zero;
		_other.transform.position = startingPos;
	}

}