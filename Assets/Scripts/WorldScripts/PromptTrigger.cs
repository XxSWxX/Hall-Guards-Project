using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PromptTrigger : MonoBehaviour
{

    private GameObject Player;

    public GameObject uiCanvas;
    public TextMeshProUGUI promptText;

    public Image progressMeter;

    //public GameObject promptUIPrefab;
    public string EpromptText;
    public string EpromptTextActive;
    public KeyCode activationButton;


    public float holdTime = 3;
    private float currentHoldTime = 0;

    public bool RememberProgress = false; //if the progress is to be retained upon exiting the prompt, good for really long trigger times

    public bool completed = false;

    private bool buttonHeld = false;

    [SerializeField]
    private GameObject prompt;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("FPS Player");
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(activationButton))
        {
            buttonHeld = true;
        }
        if (Input.GetKeyUp(activationButton))
        {

            if (prompt != null)
            {
                if (!RememberProgress) 
                { 
                    progressMeter.fillAmount = 0; 
                }
            }

            buttonHeld = false;
        }

        //bonus points if I can later assign a physical position for the prompt UI to hover over the object

    }

    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject == Player && buttonHeld)
        {

            if (prompt != null)
            {
                promptText.text = EpromptTextActive;
                //Debug.Log($"Current EpromptActive: {EpromptTextActive}");
            }

            currentHoldTime += Time.deltaTime;
            if (currentHoldTime > holdTime)
            {
                completed = true; // other script should listen for this value and do what's needed, script example comment set up at bottom for easy copy/paste 
                if (prompt != null)
                {
                    prompt.SetActive(false);
                }
            }
            else
            {

                if (prompt != null)
                {
                    progressMeter.fillAmount = currentHoldTime / holdTime;
                }

            }
            
        }
        else if (other.gameObject == Player && !buttonHeld)
        {
            if (!RememberProgress)
            {
                currentHoldTime = 0;

                if (prompt != null)
                {
                    promptText.text = EpromptText;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player && completed == false)
        {
            prompt.SetActive(true);
            promptText.text = EpromptText;
            Debug.Log($"Current Static Prompt: {EpromptText}, Current Active Prompt: {EpromptTextActive}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player)
        {
            if (prompt != null)
            {
                prompt.SetActive(false);
            }
            if (!RememberProgress) 
            { 
                currentHoldTime = 0; 
            }
        }
    }



}

//example of code in another script listening for completed value

//public PromptTrigger promptTrigger; //the trigger gameobject containing the PromptTrigger script

//private bool triggered = false;

//// Update is called once per frame
//void Update()
//{
//    if (promptTrigger.completed == true && triggered == false)
//    {
//        triggered = true;
//        Debug.Log("Trigger completed");
//    }
//}

