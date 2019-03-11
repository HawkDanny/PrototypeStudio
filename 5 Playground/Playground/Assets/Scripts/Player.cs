using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Player : MonoBehaviour
{
    [Header("Rewired Info")]
    public int playerID;
    private Rewired.Player player;

    [Header("Sprite Info")]
    public Sprite leftSprite;
    public Sprite rightSprite;
    public Sprite forwardSprite;
    public Sprite backwardSprite;
    private SpriteRenderer spriteRenderer;

    [Header("Movement Info")]
    public float moveSpeed;
    private CharacterController cc;
    private Vector3 moveVector;
    private bool grounded;
    private Direction currentDirection;
    private Direction previousDirection;
    public float wiggleAngle;
    public float wiggleStepDistance;
    private bool wiggleRight;
    private Vector3 wiggleRotation;
    private Vector3 previousWigglePosition;

    [Header("Jump Info")]
    public float jumpForce;
    public float flyForce;
    private Vector3 flyVector;
    private Vector3 previousFlyVector;
    private Rigidbody rb;

    [Header("Crown Info")]
    public GameObject crownPrefab;
    private Transform crownSpawn;
    private List<GameObject> crowns;
    

    private CameraManager _cameraManager;

    private void Awake()
    {
        _cameraManager = GameObject.FindGameObjectWithTag("CameraManager").GetComponent<CameraManager>();
        crownSpawn = this.transform.GetChild(0).transform;
        crowns = new List<GameObject>();
        rb = this.GetComponent<Rigidbody>();

        player = ReInput.players.GetPlayer(playerID);

        cc = this.GetComponent<CharacterController>();

        spriteRenderer = this.GetComponent<SpriteRenderer>();

        moveVector = Vector3.zero;

        previousWigglePosition = this.transform.position;
        wiggleRight = true;
    }

    void Update()
    {
        GroundedCheck();
        BillboardSprite();
        GetInput();
        ProcessInput();
    }

    private void GetInput()
    {
        moveVector.x = player.GetAxis("MoveHorizontal");
        moveVector.z = player.GetAxis("MoveVertical");
        flyVector.x = player.GetAxis("Jump Horizontal");
        flyVector.y = player.GetAxis("Jump Vertical");
    }

    private void ProcessInput()
    {
        //Camera based movement
        Vector3 forward = _cameraManager._camera.transform.forward;
        Vector3 right = _cameraManager._camera.transform.right;
        forward.y = 0f; //Forward and right are now on the horizontal plane
        right.y = 0f;
        forward.Normalize();
        right.Normalize();
        Vector3 desiredMovementVector = (moveVector.x * right) + (moveVector.z * forward);

        //Move the character around
        if (moveVector.x != 0.0f || moveVector.z != 0.0f)
        {
            cc.Move(desiredMovementVector * moveSpeed * Time.deltaTime);

            //If the character is going right more than any other direction
            if (Vector3.Dot(moveVector, Vector3.right) > 0.5f)
                currentDirection = Direction.West;
            else if (Vector3.Dot(moveVector, Vector3.right) < -0.5f)
                currentDirection = Direction.East;
            else if (Vector3.Dot(moveVector, Vector3.forward) > 0.5f)
                currentDirection = Direction.North;
            else if (Vector3.Dot(moveVector, Vector3.forward) < -0.5f)
                currentDirection = Direction.South;
        }

        //Vector3 desiredFlyVector = (flyVector.x * right) + (this.transform.up *  flyVector.y);

        //    print(desiredFlyVector);
        //If the stick was moved quickly in one frame
        //if (Mathf.Abs(previousFlyVector.magnitude - flyVector.magnitude) > 0.04f)
        //{
        //        rb.AddForce(desiredFlyVector * jumpForce * Time.deltaTime, ForceMode.Impulse);
        //}
        //rb.AddForce(desiredFlyVector * flyForce * Time.deltaTime);

        //Handle a direction change
        if (previousDirection != currentDirection)
            HandleDirectionDelta();

        //Wiggle if moving
        if (moveVector.magnitude > 0.05)
            HandleWiggle();

        previousDirection = currentDirection;
        previousFlyVector = flyVector;
    }

    private void BillboardSprite()
    {
        //Doesn't look at that camera
        //this.transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.y, 0f);

        //Looks at camera
        this.transform.LookAt(Camera.main.transform);
        Vector3 currentRotation = this.transform.rotation.eulerAngles;
        this.transform.rotation = Quaternion.Euler(0f, currentRotation.y, 0f); //not the best code but I'm over it
    }

    private void HandleDirectionDelta()
    {
        switch(currentDirection)
        {
            case Direction.North:
                spriteRenderer.sprite = backwardSprite;
                break;
            case Direction.South:
                spriteRenderer.sprite = forwardSprite;
                break;
            case Direction.East:
                spriteRenderer.sprite = rightSprite;
                break;
            case Direction.West:
                spriteRenderer.sprite = leftSprite;
                break;
        }
    }

    private void HandleWiggle()
    {
        if (Vector3.Distance(previousWigglePosition, this.transform.position) > wiggleStepDistance)
        {
            if (wiggleRight)
                wiggleRotation = new Vector3(this.transform.rotation.eulerAngles.x, this.transform.rotation.eulerAngles.y, wiggleAngle);
            else
                wiggleRotation = new Vector3(this.transform.rotation.eulerAngles.x, this.transform.rotation.eulerAngles.y, -wiggleAngle);

            wiggleRight = !wiggleRight;

            previousWigglePosition = this.transform.position;
        }

        //TODO: Set the angle
        this.transform.rotation = Quaternion.Euler(wiggleRotation);
    }

    private void GroundedCheck()
    {
        //Ray ray = new Ray(this.transform.position, -Vector3.up);

        if (Physics.Raycast(this.transform.position, -Vector3.up, 0.1f))
            grounded = true;
        else
            grounded = false;

        Debug.DrawRay(this.transform.position, -Vector3.up, Color.blue, 0.1f);
    }

    private void AddCrowns(int numCrowns)
    {
        int crownCount = crowns.Count;
        for (int i = crowns.Count; i < crownCount + numCrowns; i++)
        {
            Vector3 localSpawn = new Vector3(-0.08f, 3.85f + (i * 0.69f), i * 0.1f);
            crowns.Add(GameObject.Instantiate(crownPrefab, this.transform.TransformPoint(localSpawn), Quaternion.identity, crownSpawn));
        }
    }

    private void SubtractCrowns(int numCrowns)
    {
        if (crowns.Count == 0)
            return;
        int crownCount = crowns.Count;
        for (int i = crowns.Count; i > crownCount - numCrowns; i--)
        {
            GameObject.Destroy(crowns[i - 1]);
            crowns.RemoveAt(i - 1);
        }
    }

    public enum Direction
    {
        North,
        South,
        East,
        West
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Trash Can"))
            AddCrowns(1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bird") && other.gameObject != this.gameObject)
            SubtractCrowns(1);
    }
}
