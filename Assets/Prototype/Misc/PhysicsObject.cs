using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour {
    protected Vector2 velocity;
    protected Vector2 groundNormal;
    protected Vector2 targetVelocity; 
    protected const float minMoveDistance = 0.001f;
    protected Rigidbody2D rb2d;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitbuffer = new RaycastHit2D[16];
    protected const float shellRadius = 0.01f;
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);
    protected bool grounded; 
    public float minGroundNormaly = .65f;
    public float gravityModifier = 1f;

    void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
	// Use this for initialization
	void Start () {
        contactFilter.useTriggers = false;
        //gets info from physics 2d settings to determine collsion 
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity();	

		CheckAttack (); //This should be relocated somewhere else in a update
	}

	protected virtual void CheckAttack()
	{

	}

    protected virtual void ComputeVelocity()
    {

    }
    void FixedUpdate()
    {
        //adds gravity
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;

        //done this way to handle slopes
        velocity.x = targetVelocity.x;
        grounded = false;
        //finds the position
        Vector2 deltaPosition = velocity * Time.deltaTime;
        //handles the slopes instead of just going straight
        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
        Vector2 move = moveAlongGround * deltaPosition.x;
        Movement(move, false);
        
        

        //creates the movement
        move = Vector2.up * deltaPosition.y;

        Movement(move, true);
    }

    void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;
        //only checks collision if true
        if (distance > minMoveDistance)
        {
           int count = rb2d.Cast(move, contactFilter, hitbuffer, distance + shellRadius);
            hitBufferList.Clear();
            for(int i = 0; i < count; i++)
            {
                hitBufferList.Add(hitbuffer[i]);
            }

            //need to check the normal
            // make sure player is at a certain angle
            // so they dont stand on like the wall
            for (int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal;
                if (currentNormal.y > minGroundNormaly)
                {
                    grounded = true;
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }
                //add thing to make 0 movement in the y direction after hill
                // probably  like make gravity super heavy unless jump is hit

                //determines whether to subtract veloctiy from entering into
                //another collider 
                float projection = Vector2.Dot(velocity, currentNormal);
                if (projection < 0)
                {
                    //cancels velocity that would incure if hitting about head
                    velocity = velocity - projection * currentNormal;

                }

                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;

            }
        }

        rb2d.position = rb2d.position + move.normalized * distance; 
    }
}
