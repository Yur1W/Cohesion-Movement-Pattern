using UnityEngine.UI;
using UnityEngine;



[RequireComponent(typeof(Rigidbody2D))]
public class SteeringActorCohesiion : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Behavior behavior = Behavior.Seek;
    [SerializeField] Transform target = null;
    [SerializeField] float maxSpeed = 4f;
    [SerializeField, Range(0.1f, 0.99f)] float decelerationFactor = 0.75f;
    [SerializeField] float arriveRadius = 1.2f;
    [SerializeField] float stopRadius = 0.5f;
    [SerializeField] float evadeRadius = 5f;

    [SerializeField] float cohesionImpact = 1.0f;
    [SerializeField] float cohesionSize = 3.0f;
    [SerializeField] LayerMask cohesionLayer;

    Text behaviorDisplay = null;
    Rigidbody2D physics;
    State state = State.Idle;

    enum Behavior { Idle, Seek, Evade }
    enum State { Idle, Arrive, Seek, Evade }
    void FixedUpdate()
    {
        if (target != null)
        {
            switch (behavior)
            {
                case Behavior.Idle: IdleBehavior(); break;
                case Behavior.Seek: SeekBehavior(); break;
                case Behavior.Evade: EvadeBehavior(); break;
            }
        }

        physics.velocity = Vector2.ClampMagnitude(physics.velocity, maxSpeed);

        behaviorDisplay.text = state.ToString().ToUpper();
    }
    Vector2 CohesionDirection()
    { 
        Collider2D[] area = Physics2D.OverlapCircleAll(transform.position, cohesionSize, cohesionLayer);
        Vector2 avaragePosition = Vector2.zero;
        int count = 0;
        foreach (Collider2D col in area)
        {
            if (col.gameObject != this.gameObject)
            {
                avaragePosition += (Vector2)col.transform.position;
                count++;
            }
        }
        if (count == 0)
        {
            return Vector2.zero;
        }
        avaragePosition /= count;
        
        // Calculate steering force: desired velocity - current velocity
        avaragePosition = avaragePosition - (Vector2)transform.position;
        avaragePosition = avaragePosition.normalized;
        return avaragePosition;
    }

    void IdleBehavior()
    {
        physics.velocity = physics.velocity * decelerationFactor;
    }

    void SeekBehavior()
    {
        Vector2 delta = target.position - transform.position;
        Vector2 steering = delta.normalized * maxSpeed - physics.velocity;
        float distance = delta.magnitude;
        Vector2 cohesionDir = CohesionDirection();
        steering += cohesionDir * cohesionImpact;

        if (distance < stopRadius)
        {
            state = State.Idle;
        }
        else if (distance < arriveRadius)
        {
            state = State.Arrive;
        }
        else
        {
            state = State.Seek;
        }

        switch (state)
        {
            case State.Idle:
                IdleBehavior();
                break;
            case State.Arrive:
                var arriveFactor = 0.01f + (distance - stopRadius) / (arriveRadius - stopRadius);
                physics.velocity += arriveFactor * steering * Time.fixedDeltaTime;
                break;
            case State.Seek:
                physics.velocity += steering * Time.fixedDeltaTime;
                break;
        }
    }

    void EvadeBehavior()
    {
        Vector2 delta = target.position - transform.position;
        Vector2 steering = delta.normalized * maxSpeed - physics.velocity;
        float distance = delta.magnitude;
        Vector2 cohesionDir = CohesionDirection();
        steering += cohesionDir * cohesionImpact;

        if (distance > evadeRadius)
        {
            state = State.Idle;
        }
        else
        {
            state = State.Evade;
        }

        switch (state)
        {
            case State.Idle:
                IdleBehavior();
                break;
            case State.Evade:
                physics.velocity -= steering * Time.fixedDeltaTime;
                physics.velocity += cohesionDir * cohesionImpact * Time.fixedDeltaTime;
                break;
        }
    }

    void Awake()
    {
        physics = GetComponent<Rigidbody2D>();
        physics.isKinematic = true;
        behaviorDisplay = GetComponentInChildren<Text>();
    }

    void OnDrawGizmos()
    {
        if (target == null)
        {
            return;
        }

        switch (behavior)
        {
            case Behavior.Idle:
                break;
            case Behavior.Seek:
                Gizmos.color = Color.white;
                Gizmos.DrawWireSphere(transform.position, arriveRadius);
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position, stopRadius);
                break;
            case Behavior.Evade:
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, evadeRadius);
                break;
        }

        Gizmos.color = Color.gray;
        Gizmos.DrawLine(transform.position, target.position);
    }
}
