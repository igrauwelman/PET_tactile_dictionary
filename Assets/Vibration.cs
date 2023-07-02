using System;
using System.Collections.Generic;
using Bhaptics.SDK2;
using UnityEngine;

public class Vibration : MonoBehaviour
{
    private float distance;
    private float intensity = 1f;
    private int distanceID;
    
    // index 0-19: front motors, index 20-39: back motors; value indicates intensity from 0-100
    private int[] motors = new int[40] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    
    private GameObject player;
    private Dictionary<int, String> distanceIDPatternDictLeft = new Dictionary<int, string>();
    private Dictionary<int, String> distanceIDPatternDictMiddleLeft = new Dictionary<int, string>();
    private Dictionary<int, String> distanceIDPatternDictMiddle = new Dictionary<int, string>();
    private Dictionary<int, String> distanceIDPatternDictMiddleRight = new Dictionary<int, string>();
    private Dictionary<int, String> distanceIDPatternDictRight = new Dictionary<int, string>();

    private Dictionary<String, int[]> patternMotorsIdentifier = new Dictionary<string, int[]>();

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        
        distanceIDPatternDictLeft[1] = BhapticsEvent.LEFT_1;
        distanceIDPatternDictLeft[3] = BhapticsEvent.LEFT_3;
        distanceIDPatternDictLeft[5] = BhapticsEvent.LEFT_5;

        distanceIDPatternDictMiddleLeft[1] = BhapticsEvent.MIDDLE_LEFT_1;
        distanceIDPatternDictMiddleLeft[3] = BhapticsEvent.MIDDLE_LEFT_3;
        distanceIDPatternDictMiddleLeft[5] = BhapticsEvent.MIDDLE_LEFT_5;
        
        distanceIDPatternDictMiddle[1] = BhapticsEvent.MIDDLE_1;
        distanceIDPatternDictMiddle[3] = BhapticsEvent.MIDDLE_3;
        distanceIDPatternDictMiddle[5] = BhapticsEvent.MIDDLE_5;
        
        distanceIDPatternDictMiddleRight[1] = BhapticsEvent.MIDDLE_RIGHT_1;
        distanceIDPatternDictMiddleRight[3] = BhapticsEvent.MIDDLE_RIGHT_3;
        distanceIDPatternDictMiddleRight[5] = BhapticsEvent.MIDDLE_RIGHT_5;
        
        distanceIDPatternDictRight[1] = BhapticsEvent.RIGHT_1;
        distanceIDPatternDictRight[3] = BhapticsEvent.RIGHT_3;
        distanceIDPatternDictRight[5] = BhapticsEvent.RIGHT_5;

        // set the motor indices for each pattern
        patternMotorsIdentifier[BhapticsEvent.LEFT_1] = new[] {28};
        patternMotorsIdentifier[BhapticsEvent.LEFT_3] = new[] {24, 28, 32};
        patternMotorsIdentifier[BhapticsEvent.LEFT_5] = new[] {20, 24, 28, 32, 36};
        patternMotorsIdentifier[BhapticsEvent.MIDDLE_LEFT_1] = new[] {28, 29};
        patternMotorsIdentifier[BhapticsEvent.MIDDLE_LEFT_3] = new[] {24, 25, 28, 29, 32, 33};
        patternMotorsIdentifier[BhapticsEvent.MIDDLE_LEFT_5] = new[] {20, 21, 24, 25, 28, 29, 32, 33, 36, 37};
        patternMotorsIdentifier[BhapticsEvent.MIDDLE_1] = new[] {29, 30};
        patternMotorsIdentifier[BhapticsEvent.MIDDLE_3] = new[] {25, 26, 29, 30, 33, 34};
        patternMotorsIdentifier[BhapticsEvent.MIDDLE_5] = new[] {21, 22, 25, 26, 29, 30, 33, 34, 37, 38};
        patternMotorsIdentifier[BhapticsEvent.MIDDLE_RIGHT_1] = new[] {30, 31};
        patternMotorsIdentifier[BhapticsEvent.MIDDLE_RIGHT_3] = new[] {26, 27, 30, 31, 34, 35};
        patternMotorsIdentifier[BhapticsEvent.MIDDLE_RIGHT_5] = new[] {22, 23, 26, 27, 30, 31, 34, 35, 38, 39};
        patternMotorsIdentifier[BhapticsEvent.RIGHT_1] = new[] {31};
        patternMotorsIdentifier[BhapticsEvent.RIGHT_3] = new[] {27, 31, 35};
        patternMotorsIdentifier[BhapticsEvent.RIGHT_5] = new[] {23, 27, 31, 35, 39};

        // set the intensity of all motors to 0
        foreach (var motor in motors)
        {
            motors[motor] = (int) 0;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Obstacle")) return;
        
        // identify the distance pattern(1, 3, 5 rows)
        distanceID = GetDistancePatternIdentifier(other.gameObject);
        // set the intensity based on the current distance
        SetIntensity(distance);

        // identify where the obstacle is and set the respective motors
        switch (this.name)
        {
            case "left":
                SetMotors(distanceIDPatternDictLeft[distanceID]);
                BhapticsLibrary.PlayMotors((int) Bhaptics.SDK2.PositionType.Vest, motors, 1000);
                break;
            case "middle_left":
                SetMotors(distanceIDPatternDictMiddleLeft[distanceID]);
                BhapticsLibrary.PlayMotors((int) Bhaptics.SDK2.PositionType.Vest, motors, 1000);
                break;
            case "middle":
                SetMotors(distanceIDPatternDictMiddle[distanceID]);
                BhapticsLibrary.PlayMotors((int) Bhaptics.SDK2.PositionType.Vest, motors, 1000);
                break;
            case "middle_right":
                SetMotors(distanceIDPatternDictMiddleRight[distanceID]);
                BhapticsLibrary.PlayMotors((int) Bhaptics.SDK2.PositionType.Vest, motors, 1000);
                break;
            case "right":
                SetMotors(distanceIDPatternDictRight[distanceID]);
                BhapticsLibrary.PlayMotors((int) Bhaptics.SDK2.PositionType.Vest, motors, 1000);
                break;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Obstacle")) return;

        BhapticsLibrary.StopAll();
        
        ClearMotors();
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

    // map distance (0-4) to intensity (50-100)
    private void SetIntensity(float dist)
    {
        float x = Mathf.Clamp(dist, 0.5f, 4f);
        intensity = ((50f - 100f) * (x - 0.0f) / (4.0f - 0.0f)) + 100f;
    }
    
    private void SetMotors(String pattern)
    {
        foreach (var motor in patternMotorsIdentifier[pattern])
        {
            motors[motor] = (int) intensity;
        }
    }

    private void ClearMotors()
    {
        Array.Clear(motors, 0, motors.Length);
    }
    
}
