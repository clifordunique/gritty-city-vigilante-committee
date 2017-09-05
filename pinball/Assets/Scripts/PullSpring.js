#pragma strict

var inputButtonName : String = "Pull";
var distance : float = 50;
var speed : float = 1;
var ball : GameObject;
var power : float = 2000;

private var ready : boolean = false;
private var fire : boolean = false;
private var moveCount : float = 0;

function OnCollisionEnter(_other : Collision) {
	if(_other.gameObject.tag == "Ball"){
		ready = true;
	}
}

function Update () {

	if(Input.GetButton(inputButtonName)){
		//As the button is held down, slowly move the piece
		if(moveCount < distance){
			transform.Translate(0,0,-speed * Time.deltaTime);
			moveCount += speed * Time.deltaTime;
			fire = true;
		}
	}
	else if(moveCount > 0){
		//Shoot the ball
		if(fire && ready){
			ball.transform.TransformDirection(Vector3.forward * 10);
			ball.GetComponent.<Rigidbody>().AddForce(0, 0, moveCount * power);
			fire = false;
			ready = false;
		}
		//Once we have reached the starting position fire off!
		transform.Translate(0,0,20 * Time.deltaTime);
		moveCount -= 20 * Time.deltaTime;
	}
	
	//Just ensure we don't go past the end
	if(moveCount <= 0){
		fire = false;
		moveCount = 0;
	}

}