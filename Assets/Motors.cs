using System;
using System.Collections;
using System.Collections.Generic;
using Bhaptics.SDK2;
using UnityEngine;

public class Motors : MonoBehaviour
{
    // index 0-19: front motors, index 20-39: back motors; value indicates intensity from 0-100
    public readonly int[] motors = new int[40] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    
    public readonly Dictionary<int, string> DistanceIDPatternDictLeft = new ();
    public readonly Dictionary<int, string> DistanceIDPatternDictMiddleLeft = new ();
    public readonly Dictionary<int, string> DistanceIDPatternDictMiddle = new ();
    public readonly Dictionary<int, string> DistanceIDPatternDictMiddleRight = new ();
    public readonly Dictionary<int, string> DistanceIDPatternDictRight = new ();

    public readonly Dictionary<string, int[]> PatternMotorsIdentifier = new ();

    public float Intensity { get; set; } = 1f;
    public int Rows { get; private set; } = 1;

    public static Motors Instance { get; private set; }

    private void Awake() 
    { 
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        }  
    }

    private void Start()
    {
        DistanceIDPatternDictLeft[1] = BhapticsEvent.LEFT_1;
        DistanceIDPatternDictLeft[3] = BhapticsEvent.LEFT_3;
        DistanceIDPatternDictLeft[5] = BhapticsEvent.LEFT_5;

        DistanceIDPatternDictMiddleLeft[1] = BhapticsEvent.MIDDLE_LEFT_1;
        DistanceIDPatternDictMiddleLeft[3] = BhapticsEvent.MIDDLE_LEFT_3;
        DistanceIDPatternDictMiddleLeft[5] = BhapticsEvent.MIDDLE_LEFT_5;
        
        DistanceIDPatternDictMiddle[1] = BhapticsEvent.MIDDLE_1;
        DistanceIDPatternDictMiddle[3] = BhapticsEvent.MIDDLE_3;
        DistanceIDPatternDictMiddle[5] = BhapticsEvent.MIDDLE_5;
        
        DistanceIDPatternDictMiddleRight[1] = BhapticsEvent.MIDDLE_RIGHT_1;
        DistanceIDPatternDictMiddleRight[3] = BhapticsEvent.MIDDLE_RIGHT_3;
        DistanceIDPatternDictMiddleRight[5] = BhapticsEvent.MIDDLE_RIGHT_5;
        
        DistanceIDPatternDictRight[1] = BhapticsEvent.RIGHT_1;
        DistanceIDPatternDictRight[3] = BhapticsEvent.RIGHT_3;
        DistanceIDPatternDictRight[5] = BhapticsEvent.RIGHT_5;

        // set the motor indices for each pattern
        PatternMotorsIdentifier[BhapticsEvent.LEFT_1] = new[] {28};
        PatternMotorsIdentifier[BhapticsEvent.LEFT_3] = new[] {24, 28, 32};
        PatternMotorsIdentifier[BhapticsEvent.LEFT_5] = new[] {20, 24, 28, 32, 36};
        PatternMotorsIdentifier[BhapticsEvent.MIDDLE_LEFT_1] = new[] {28, 29};
        PatternMotorsIdentifier[BhapticsEvent.MIDDLE_LEFT_3] = new[] {24, 25, 28, 29, 32, 33};
        PatternMotorsIdentifier[BhapticsEvent.MIDDLE_LEFT_5] = new[] {20, 21, 24, 25, 28, 29, 32, 33, 36, 37};
        PatternMotorsIdentifier[BhapticsEvent.MIDDLE_1] = new[] {29, 30};
        PatternMotorsIdentifier[BhapticsEvent.MIDDLE_3] = new[] {25, 26, 29, 30, 33, 34};
        PatternMotorsIdentifier[BhapticsEvent.MIDDLE_5] = new[] {21, 22, 25, 26, 29, 30, 33, 34, 37, 38};
        PatternMotorsIdentifier[BhapticsEvent.MIDDLE_RIGHT_1] = new[] {30, 31};
        PatternMotorsIdentifier[BhapticsEvent.MIDDLE_RIGHT_3] = new[] {26, 27, 30, 31, 34, 35};
        PatternMotorsIdentifier[BhapticsEvent.MIDDLE_RIGHT_5] = new[] {22, 23, 26, 27, 30, 31, 34, 35, 38, 39};
        PatternMotorsIdentifier[BhapticsEvent.RIGHT_1] = new[] {31};
        PatternMotorsIdentifier[BhapticsEvent.RIGHT_3] = new[] {27, 31, 35};
        PatternMotorsIdentifier[BhapticsEvent.RIGHT_5] = new[] {23, 27, 31, 35, 39};
    }
    
    // map distance (0-4) to intensity (50-100)
    public void SetIntensity(float dist)
    {
        float x = Mathf.Clamp(dist, 0.5f, 4f);
        Intensity = ((50f - 100f) * (x - 0.5f) / (4.0f - 0.5f)) + 100f;
    }
    
    // map distance (0-4) to number of rows (1,3,5)
    public void SetRows(float dist)
    {
        float x = Mathf.Clamp(dist, 0.5f, 4f);
        float rows = ((0f - 5f) * (x - 0.5f) / (4.0f - 0.5f)) + 5f;
        
        // clamp value to 1, 3 or 5
        switch (rows)
        {
            case >= 3f:
                Rows = 5;
                break;
            case >=2f:
                Rows = 3;
                break;
            case < 2f:
                Rows = 1;
                break;
        }
    }
    
    public void SetMotors(string pattern)
    {
        foreach (var motor in Motors.Instance.PatternMotorsIdentifier[pattern])
        {
            motors[motor] = (int) Intensity;
        }
    }

    public void ClearMotors()
    {
        Array.Clear(motors, 0, motors.Length);
    }
}
