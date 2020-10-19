using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private CharacterController controller;
    private Vector3 dir;
    public float forwardSpeed;
    public float maxSpeed;

    public int desiredLane = 1;
    public float laneDistance = 4;

    public bool isRewinding = false;
    public List<Vector3> positions;
    public List<int> rewindLane;

    public new Collider collider;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
        positions = new List<Vector3>();
        rewindLane = new List<int>();
        controller = GetComponent<CharacterController>();
        controller.enabled = true;
    }

    private void Update()
    {
        dir.z = forwardSpeed;

        if (forwardSpeed < maxSpeed)
        {
            forwardSpeed += 5f * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            desiredLane++;
            if (desiredLane == 3)
            {
                desiredLane = 2;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            desiredLane--;
            if (desiredLane == -1)
            {
                desiredLane = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartRewind();
            collider.enabled = false;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            StopRewind();
            collider.enabled = true;
        }

        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        if (desiredLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        }
        else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }

        //transform.position = Vector3.Lerp(transform.position,targetPosition,10*Time.deltaTime);
        //controller.center = controller.center;
        if (transform.position == targetPosition)
            return;

        if (isRewinding)
            return;
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
        if (moveDir.sqrMagnitude < diff.sqrMagnitude)
            controller.Move(moveDir);
        else
            controller.Move(diff);
    }
    private void FixedUpdate()
    {
        if (!isRewinding)
        {
            controller.Move(dir * Time.deltaTime);
        }

        if (isRewinding)
            Rewind();
        else
            Record();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.transform.tag == "Obstacle")
        {
            PlayerManager.gameOver = true;
        }
    }

    void Rewind()
    {
        desiredLane = rewindLane[0];
        rewindLane.RemoveAt(0);
        transform.position = positions[0];
        positions.RemoveAt(0);
    }
    void Record()
    {
        positions.Insert(0, transform.position);
        rewindLane.Insert(0, desiredLane);
    }

    public void StartRewind()
    {
        isRewinding = true;
    }

    public void StopRewind()
    {
        isRewinding = false;
    }
}
