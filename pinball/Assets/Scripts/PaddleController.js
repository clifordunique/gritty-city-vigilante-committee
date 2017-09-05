#pragma strict

//Parameters
var restPosition : float = 0F;
var pressedPosition : float = 45F;
var flipperStrength : float = 10F;
var flipperDamper : float = 1F;
var inputButtonName : String = "LeftPaddle";

function Awake ()
{
	GetComponent.<HingeJoint>().useSpring = true;
}

// Update is called once per frame
function Update ()
{
	var spring : JointSpring = new JointSpring();
	
	spring.spring = flipperStrength;
	spring.damper = flipperDamper;
	
	if (Input.GetButton(inputButtonName))
		spring.targetPosition = pressedPosition;
	else
		spring.targetPosition = restPosition;
	
	GetComponent.<HingeJoint>().spring = spring;
	GetComponent.<HingeJoint>().useLimits = true;
	GetComponent.<HingeJoint>().limits.min = restPosition;
	GetComponent.<HingeJoint>().limits.max = pressedPosition;
}