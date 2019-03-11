using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Camera")]
    public Camera _camera;
    public float desiredHeightOverTarget; //How much higher (delta y) the camera should be over the target
    public float desiredDistanceFromTarget;
    private float cameraAngle;
    private Vector3 desiredCameraPosition;
    private Vector3 cameraTarget;

    [Header("Players")]
    public Transform leftPlayer;
    public Transform rightPlayer;

    private void Update()
    {
        //Get the vector from the left to the right player, define the camera target
        Vector3 vectorBetweenPlayers = rightPlayer.position - leftPlayer.position;
        Debug.DrawRay(leftPlayer.position, vectorBetweenPlayers, Color.white);
        cameraTarget = leftPlayer.position + (vectorBetweenPlayers / 2); //TODO: get rid of vectorbetweenplayers

        //Change the camera's distance from the players based on their distances from each other
        float distanceFromTarget = desiredDistanceFromTarget + (vectorBetweenPlayers.magnitude / 2);
        float heightOverTarget = desiredHeightOverTarget + (vectorBetweenPlayers.magnitude / 5);

        //Get a normalized vector perpendicular to the players, coming toward the screen
        Vector3 perpendicularVector = new Vector3(vectorBetweenPlayers.z, 0, -vectorBetweenPlayers.x);
        perpendicularVector.Normalize();

        //Calculate the magnitude of the perpendicular vector
        float cameraAngle = Mathf.Asin(heightOverTarget / distanceFromTarget);
        perpendicularVector *= (Mathf.Cos(cameraAngle) * distanceFromTarget);

        Debug.DrawRay(cameraTarget, perpendicularVector, Color.cyan);
        Vector3 lateralCameraPosition = cameraTarget + perpendicularVector;

        //Calculate the desiredCameraPosition
        desiredCameraPosition = new Vector3(lateralCameraPosition.x, lateralCameraPosition.y + heightOverTarget, lateralCameraPosition.z);

        Debug.DrawLine(cameraTarget + perpendicularVector, desiredCameraPosition, Color.green);

        //TODO: Ease into these instead of making it on them the whole time
        _camera.transform.SetPositionAndRotation(desiredCameraPosition, Quaternion.identity);
        _camera.transform.LookAt(cameraTarget);
    }
}