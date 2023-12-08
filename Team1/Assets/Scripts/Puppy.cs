using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Puppy : MonoBehaviour
{
    [Header("Necessary references")]
    public bool isSitting = true;
    public bool isFree = true;
    public Transform player; // Reference to the player's Transform
    private Vector3 currentVelocity = Vector3.zero;
    public Rope rope;
    public Animator animator;

    private int puppyIndex = -1;


    [Header("Tweakable")]
    public float moveAwaySpeed;     // 1.5f      // puppys movement speed away from the player
    public float moveToPlayerSpeed; // 3.0f      // puppys movement speed towards the player
    public float acceleration;      // 1f;       // acceleration at which the puppy gains speed
    private float lerp_t = 0.3f;    // 0.3f

    public float closeToPlayerDistance; // 1f   // distance to player that is good enough to consider close
    public float timeToFollow;  // 4f           // time for which the puppy keeps following the player
    public float maxDistance;   // 10f          // maximum distance the puppy can move away from the player (also, rope length)


    void Update() {
        if (player != null) {
            Movement();
        }
    }

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


                if ((player.position - transform.position).magnitude <= closeToPlayerDistance)
                {
                    currentVelocity = Vector3.zero;
                } else
                {
                    MoveToLocation(transform.position + currentVelocity * Time.deltaTime);
                }
            }
        }
    }


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
        }
        else
        {
            // stop
            Debug.Log("Puppy edge reached");
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
        float angle = 360.0f * puppyIndex / player.GetComponent<PlayerCharacter>().GetNumPuppiesObtained();
        return new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
    }

    public void StrayNoMore(int index, Transform player, Transform arm) {
        // Debug.Log("Stray no more");
        puppyIndex = index;
        isSitting = false;
        this.player = player;
        isFree = true;

        // activate rope
        rope.ConnectRope(arm);
        rope.gameObject.SetActive(true);
    }

    public bool IsStray()
    {
        return player == null;
    }


    public IEnumerator Follow()
    {
        isFree = false;
        yield return new WaitForSeconds(timeToFollow);
        isFree = true;
    }
}
