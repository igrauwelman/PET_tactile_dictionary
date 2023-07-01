using System;
using System.Collections;
using System.Collections.Generic;
using Bhaptics.SDK2;
using Unity.VisualScripting;
using UnityEngine;

public class Vibration : MonoBehaviour
{
    private float distance;
    
    private GameObject player;
    private Dictionary<int, String> distanceIDPatternDictLeft = new Dictionary<int, string>();
    private Dictionary<int, String> distanceIDPatternDictMiddleLeft = new Dictionary<int, string>();
    private Dictionary<int, String> distanceIDPatternDictMiddle = new Dictionary<int, string>();
    private Dictionary<int, String> distanceIDPatternDictMiddleRight = new Dictionary<int, string>();
    private Dictionary<int, String> distanceIDPatternDictRight = new Dictionary<int, string>();

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
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Obstacle")) return;
        
        int distanceID = GetDistancePatternIdentifier(other.gameObject);

        switch (this.name)
        {
            case "left":
                BhapticsLibrary.Play(distanceIDPatternDictLeft[distanceID]);
                break;
            case "middle_left":
                BhapticsLibrary.Play(distanceIDPatternDictMiddleLeft[distanceID]);
                break;
            case "middle":
                BhapticsLibrary.Play(distanceIDPatternDictMiddle[distanceID]);
                break;
            case "middle_right":
                BhapticsLibrary.Play(distanceIDPatternDictMiddleRight[distanceID]);
                break;
            case "right":
                BhapticsLibrary.Play(distanceIDPatternDictRight[distanceID]);
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Obstacle")) return;
        
        switch (this.name)
        {
            case "left":
                BhapticsLibrary.StopByEventId(BhapticsEvent.LEFT_1);
                BhapticsLibrary.StopByEventId(BhapticsEvent.LEFT_3);
                BhapticsLibrary.StopByEventId(BhapticsEvent.LEFT_5);
                break;
            case "middle_left":
                BhapticsLibrary.StopByEventId(BhapticsEvent.MIDDLE_LEFT_1);
                BhapticsLibrary.StopByEventId(BhapticsEvent.MIDDLE_LEFT_3);
                BhapticsLibrary.StopByEventId(BhapticsEvent.MIDDLE_LEFT_5);
                break;
            case "middle":
                BhapticsLibrary.StopByEventId(BhapticsEvent.MIDDLE_1);
                BhapticsLibrary.StopByEventId(BhapticsEvent.MIDDLE_3);
                BhapticsLibrary.StopByEventId(BhapticsEvent.MIDDLE_5);
                break;
            case "middle_right":
                BhapticsLibrary.StopByEventId(BhapticsEvent.MIDDLE_RIGHT_1);
                BhapticsLibrary.StopByEventId(BhapticsEvent.MIDDLE_RIGHT_3);
                BhapticsLibrary.StopByEventId(BhapticsEvent.MIDDLE_RIGHT_5);
                break;
            case "right":
                BhapticsLibrary.StopByEventId(BhapticsEvent.RIGHT_1);
                BhapticsLibrary.StopByEventId(BhapticsEvent.RIGHT_3);
                BhapticsLibrary.StopByEventId(BhapticsEvent.RIGHT_5);
                break;
        }
    }

    private int GetDistancePatternIdentifier(GameObject obstacle)
    {
        distance = Vector3.Distance(new Vector3(player.transform.position.x, 0f, player.transform.position.z), 
            new Vector3(obstacle.transform.position.x, 0f, obstacle.transform.position.z));

        switch (distance)
        {
            case > 3f:
                return 1;
            case > 2.25f:
                return 3;
            case <= 2.25f:
                return 5;
            default:
                return 1;
        }
    }
    
}
