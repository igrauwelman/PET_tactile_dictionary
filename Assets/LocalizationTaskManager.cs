using System;
using System.Collections;
using System.Collections.Generic;
using Bhaptics.SDK2;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LocalizationTaskManager : MonoBehaviour
{
    private Dictionary<String, int[]> patternMotorsDict = new();
    private List<string> vibrationLocations = new();
    private List<string> correctResponses = new();
    private List<string> participantResponses = new();
    [SerializeField] private int occurrencePerPattern = 1;

    [SerializeField] private List<GameObject> buttons;
    [SerializeField] private TMP_Text resultText;
    private double correctnessRate = 0;
    [SerializeField] private GameObject startButton;

    private void PrepareTask()
    {
        participantResponses.Clear();
        
        patternMotorsDict = Motors.Instance.PatternMotorsIdentifier;
        Motors.Instance.Intensity = 40f;

        for (int i = 0; i < occurrencePerPattern; i++)
        {
            vibrationLocations.Add(BhapticsEvent.LEFT_1);
            vibrationLocations.Add(BhapticsEvent.LEFT_3);
            vibrationLocations.Add(BhapticsEvent.LEFT_5);
            vibrationLocations.Add(BhapticsEvent.MIDDLE_1);
            vibrationLocations.Add(BhapticsEvent.MIDDLE_3);
            vibrationLocations.Add(BhapticsEvent.MIDDLE_5);
            vibrationLocations.Add(BhapticsEvent.RIGHT_1);
            vibrationLocations.Add(BhapticsEvent.RIGHT_3);
            vibrationLocations.Add(BhapticsEvent.RIGHT_5);
        }
        
        ShuffleList();
        Debug.Log("Randomized List (" + vibrationLocations.Count + " items): " + string.Join(", ", vibrationLocations));
        correctResponses = new List<string>(vibrationLocations);
        
        UpdateUI("start");
    }
    
    private void ShuffleList()
    {
        for (int i = 0; i < vibrationLocations.Count; i++)
        {
            var temp = vibrationLocations[i];
            int randIdx = Random.Range(i, vibrationLocations.Count);
            vibrationLocations[i] = vibrationLocations[randIdx];
            vibrationLocations[randIdx] = temp;
        }
    }

    // public function to use for the Start Button
    public void StartTask()
    {
        PrepareTask();
        NextPattern();
    }

    // called upon Button click
    public void AddParticipantResponse(Button button)
    {
        Motors.Instance.ClearMotors();
        BhapticsLibrary.StopAll();
        
        switch (button.name)
        {
            case "Left5":
                participantResponses.Add(BhapticsEvent.LEFT_5);
                break;
            case "Left3":
                participantResponses.Add(BhapticsEvent.LEFT_3);
                break;
            case "Left1":
                participantResponses.Add(BhapticsEvent.LEFT_1);
                break;
            case "Middle5":
                participantResponses.Add(BhapticsEvent.MIDDLE_5);
                break;
            case "Middle3":
                participantResponses.Add(BhapticsEvent.MIDDLE_3);
                break;
            case "Middle1":
                participantResponses.Add(BhapticsEvent.MIDDLE_1);
                break;
            case "Right5":
                participantResponses.Add(BhapticsEvent.RIGHT_5);
                break;
            case "Right3":
                participantResponses.Add(BhapticsEvent.RIGHT_3);
                break;
            case "Right1":
                participantResponses.Add(BhapticsEvent.RIGHT_1);
                break;
        }
        
        // remove the pattern that was just played
        vibrationLocations.RemoveAt(0);
        
        // if no pattern is left, end the task
        if (vibrationLocations.Count == 0)
        {
            
            correctnessRate = CheckCorrectnessRate();
            UpdateUI("finished");
        }
        else
        {
            NextPattern();
        }
    }
    

    private void NextPattern()
    {
        Motors.Instance.SetMotors(vibrationLocations[0]);
        BhapticsLibrary.PlayMotors((int) Bhaptics.SDK2.PositionType.Vest, Motors.Instance.motors, 1000);
    }

    private void UpdateUI(string status)
    {
        switch (status)
        {
            case "finished":
            {
                foreach (var button in buttons)
                {
                    button.SetActive(false);
                }
                resultText.gameObject.SetActive(true);
                resultText.text =
                    "Localization task finished. The correctness rate is " + correctnessRate + "%!";
                startButton.gameObject.SetActive(true);
                startButton.GetComponentInChildren<TMP_Text>().text = "Start again";
                break;
            }
            case "start":
            {
                foreach (var button in buttons)
                {
                    button.SetActive(true);
                }
                startButton.GetComponentInChildren<TMP_Text>().text = "Start";
                resultText.gameObject.SetActive(false);
                break;
            }
        }
    }

    private double CheckCorrectnessRate()
    {
        var correct = 0f;
        
        for (int i = 0; i < correctResponses.Count; i++)
        {
            if (participantResponses[i] == correctResponses[i])
            {
                correct += 1f;
            }
        }

        correctnessRate = Math.Round((double) (correct / correctResponses.Count), 4);

        return correctnessRate * 100f; 
    }
}
