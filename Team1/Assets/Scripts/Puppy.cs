using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Puppy : MonoBehaviour
{
    // public bool isStray = true;
    public bool isSitting = true;

    public Transform player; // Reference to the player's Transform
    public float moveAwaySpeed = 1.5f; // Adjust this to control the puppy's movement speed
    public float moveToPlayerSpeed = 3.0f;
    public float acceleration = 1f;
    private Vector3 currentVelocity = Vector3.zero;
    public float closeToPlayerDistance = 0.1f; // distance to player that is good enough

    public bool isFree = true;
    public float timeToSit = 2f;

    public float maxDistance;

    public int puppyIndex = -1;

    public Rope rope;
    private float lerp_t = 0.3f;

    public Animator animator;
    public bool isMoving = false;

    void Update() {
        if (player != null) {
            Movement();
        }
    }

    /*
    // DEPRICATED
    void Movement() {
        if(!isStray) {
            if (isFree) {
                // Calculate the direction from the puppy to the player
                Vector3 awayFromPlayer = transform.position - player.position;

                if (awayFromPlayer.magnitude <= maxDistance) {
                    // other rules
                    // Vector3 awayFromPuppies = transform.position - AverageOtherPuppies();
                    Vector3 circleDir = CircleDirection();
                    // Vector3 randomness = Random.insideUnitSphere;
                    // randomness.y = 0;

                    // join and normalize
                    // Vector3 moveDirection = awayFromPlayer + awayFromPuppies + randomness;
                    Vector3 moveDirection = awayFromPlayer.normalized + circleDir.normalized; // + randomness;
                    moveDirection.Normalize();

                    // move
                    MoveInDirection(moveDirection, moveAwaySpeed);
                } else {
                    // stop
                }
            } else {
                Vector3 toPlayer = player.position - transform.position;
                MoveInDirection(toPlayer.normalized, moveToPlayerSpeed);

                if (toPlayer.magnitude <= closeToPlayerDistance) {
                    isFree = true;
                }
            }
        }        
    }
    */

    public void Movement()
    {
        if (!isSitting)
        {
            if(isFree)
            {
                // sum of all other dog positions
                Vector3 resultant = Vector3.zero;
                Puppy[] puppies = player.GetComponent<PlayerCharacter>().puppies;
                foreach (Puppy p in puppies)
                {
                    if(p != null)
                    {
                        resultant += (this.transform.position - p.transform.position).normalized;
                    }
                }

                // multiply by constant acceleration
                resultant = resultant.normalized * acceleration;

                // apply force to velocity and velocity to position
                
                currentVelocity = Vector3.ClampMagnitude(currentVelocity + resultant, moveAwaySpeed);
                Vector3 targetPosition = transform.position + currentVelocity * Time.deltaTime;

                // limit movement to rope length
                if (Vector3.Distance(player.transform.position, targetPosition) > maxDistance)
                {
                    // STOP movement
                    isMoving = false;
                    currentVelocity = Vector3.zero;
                }
                else
                {
                    // apply velocity to position
                    // position = position + v_current * time_diff
                    MoveToLocation(targetPosition);
                }
            }
            else
            {
                Vector3 toPlayer = (player.position - transform.position).normalized * acceleration;
                currentVelocity = Vector3.ClampMagnitude(currentVelocity + toPlayer, moveToPlayerSpeed);

                MoveToLocation(transform.position + currentVelocity * Time.deltaTime);

                if ((player.position - transform.position).magnitude <= closeToPlayerDistance)
                {
                    Debug.Log("close to player");
                    isFree = true;
                    isMoving = false;
                    currentVelocity = Vector3.zero;

                    // Sit
                    StartCoroutine(Sit());
                }
            }
        }
    }

    /*
     * 
     * DEPRICATED
    void MoveInDirection(Vector3 direction, float speed) {

        // find location where it wants to move
        var whereToGo = transform.position + direction * speed * Time.deltaTime;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(whereToGo, out hit, 100f, NavMesh.AllAreas)) {
            if(Vector3.Distance(hit.position, player.position) > maxDistance)
            {
                // rotate
                var save = transform.eulerAngles;
                transform.LookAt(hit.position);
                float newRotation = Mathf.LerpAngle(save.y, transform.eulerAngles.y, lerp_t);

                // move
                // transform.LookAt(hit.position);
                transform.position = hit.position;

                isMoving = true;
            } else
            {
                // Dont move
                isMoving = false;
            }

        }
        else {
            // stop
            Debug.Log("Puppy edge reached");
            isMoving = false;
        }
    }
    */

    void MoveToLocation(Vector3 whereToGo)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(whereToGo, out hit, 100f, NavMesh.AllAreas))
        {
            // rotate
            var save = transform.eulerAngles;
            transform.LookAt(hit.position);
            float newRotation = Mathf.LerpAngle(save.y, transform.eulerAngles.y, lerp_t);

            // move
            // transform.LookAt(hit.position);
            transform.position = hit.position;

            isMoving = true;
        }
        else
        {
            // stop
            Debug.Log("Puppy edge reached");
            isMoving = false;
            currentVelocity = Vector3.zero;
        }
    }


    private void OnCollisionEnter(Collision collision) {
        // Debug.Log(collision.gameObject.name);
        if(collision.gameObject.CompareTag("Player")) {
            // isFree = true;

            if(IsStray()) {
                collision.gameObject.GetComponent<PlayerCharacter>().AddPuppy(this);
            }
        }
    }

    public Vector3 AverageOtherPuppies() {
        Vector3 avg = Vector3.zero;
        int count = 0;
        foreach(var puppy in player.GetComponent<PlayerCharacter>().puppies) {
            if(puppy && puppy != this.gameObject) {
                avg += puppy.transform.position;
                count += 1;
            }
        }

        return avg / count;
    }

    public Vector3 CircleDirection() {
        float angle = 360.0f * puppyIndex / player.GetComponent<PlayerCharacter>().numPuppiesObtained;
        return new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
    }

    public void StrayNoMore(int index, Transform player, Transform arm) {
        // Debug.Log("Stray no more");
        puppyIndex = index;
        isSitting = false;
        this.player = player;
        isFree = true;

        // activate rope
        rope.arm = arm;
        rope.UpdateRope();
        rope.gameObject.SetActive(true);
    }

    public bool IsStray()
    {
        return player == null;
    }

    public IEnumerator Sit()
    {
        isSitting = true;

        yield return new WaitForSeconds(timeToSit);

        isSitting = false;
    }
}
