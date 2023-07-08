using System;
using System.Collections.Generic;
using Bhaptics.SDK2;
using UnityEngine;

public class Vibration : MonoBehaviour
{
    private float distance;
    private int distanceID;

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");

        // set the intensity of all motors to 0
        foreach (var motor in Motors.Instance.motors)
        {
            Motors.Instance.motors[motor] = 0;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Obstacle")) return;
        
        // identify the distance pattern(1, 3, 5 rows)
        distanceID = GetDistancePatternIdentifier(other.gameObject);
        // set the intensity based on the current distance
        Motors.Instance.SetIntensity(distance);

        // identify where the obstacle is and set the respective motors
        switch (this.name)
        {
            case "left":
                Motors.Instance.SetMotors(Motors.Instance.DistanceIDPatternDictLeft[distanceID]);
                BhapticsLibrary.PlayMotors((int) Bhaptics.SDK2.PositionType.Vest, Motors.Instance.motors, 1000);
                break;
            case "middle_left":
                Motors.Instance.SetMotors(Motors.Instance.DistanceIDPatternDictMiddleLeft[distanceID]);
                BhapticsLibrary.PlayMotors((int) Bhaptics.SDK2.PositionType.Vest, Motors.Instance.motors, 1000);
                break;
            case "middle":
                Motors.Instance.SetMotors(Motors.Instance.DistanceIDPatternDictMiddle[distanceID]);
                BhapticsLibrary.PlayMotors((int) Bhaptics.SDK2.PositionType.Vest, Motors.Instance.motors, 1000);
                break;
            case "middle_right":
                Motors.Instance.SetMotors(Motors.Instance.DistanceIDPatternDictMiddleRight[distanceID]);
                BhapticsLibrary.PlayMotors((int) Bhaptics.SDK2.PositionType.Vest, Motors.Instance.motors, 1000);
                break;
            case "right":
                Motors.Instance.SetMotors(Motors.Instance.DistanceIDPatternDictRight[distanceID]);
                BhapticsLibrary.PlayMotors((int) Bhaptics.SDK2.PositionType.Vest, Motors.Instance.motors, 1000);
                break;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Obstacle")) return;

        BhapticsLibrary.StopAll();
        
        Motors.Instance.ClearMotors();
    }

    private int GetDistancePatternIdentifier(GameObject obstacle)
    {
        distance = Vector3.Distance(new Vector3(player.transform.position.x, 0f, player.transform.position.z), 
            new Vector3(obstacle.transform.position.x, 0f, obstacle.transform.position.z));

        switch (distance)
        {
            case > 3.25f:
                return 1;
            case > 2.5f:
                return 3;
            case <= 2.5f:
                return 5;
            default:
                return 1;
        }
    }

}
