using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/* Hero state controller, listens to game objects and
 * recieves info about status changing. Controlls hero
 * statistics depending on current state */
public class HeroStateControl : MonoBehaviour
{
    // Variables
    public int currentFatigue = 1;
    public int currentInjuries = 1;
    public int currentSickness = 1;
    public float FatigueOverload = 0;
    public float InjuriesOverload = 0;
    public float SicknessOverload = 0;
    private List<SingleState> States = new List<SingleState>();
    // References
    private SingleState CurrentState;
    private Image DisplayImage;
    private Text DisplayText;

    //LOADING STATS FROM ANOTHER SCENE
    public bool LoadFromPrev = false;
    public HeroStatus heroStatus;
    //Load current state info from previous scene
    private void LoadStats()
    {
        if (LoadFromPrev)
        {
            GameObject statusHolder = GameObject.FindGameObjectWithTag("StatusHolder");
            if (statusHolder != null)
            {
                heroStatus = statusHolder.GetComponent<HeroStatus>();
                currentFatigue = heroStatus.currentFatigue;
                currentInjuries = heroStatus.currentInjuries;
                currentSickness = heroStatus.currentSickness;
            }
        }
    }
    // Start is called on scene load
    private void Start()
    {
        LoadStats();
        createStates();
        DisplayImage = GetComponent<Image>();
        DisplayText = transform.Find("text").GetComponent<Text>();
        UpdateCurrentState();
        UpdateDisplay();
        StartCoroutine(OverloadController());

    }
    // Force request to set state variables
    private void ForceSetAtributes(int newFatigue, int newInjuries, int newSickness, int mode)// mode1 = set, mode2 = ++, mode3 = --;
    {
        if (mode == 1)
        {
            currentFatigue = newFatigue;
            currentInjuries = newInjuries;
            currentSickness = newSickness;
        }
        if (mode == 2)
        {
            currentFatigue += newFatigue;
            if (currentFatigue > 100)
                currentFatigue = 100;
            currentInjuries += newInjuries;
            if (currentInjuries > 100)
                currentInjuries = 100;
            currentSickness += newSickness;
            if (currentSickness > 100)
                currentSickness = 100;
        }
        if (mode == 3)
        {
            currentFatigue -= newFatigue;
            if (currentFatigue < 0)
                currentFatigue = 0;
            currentInjuries -= newInjuries;
            if (currentInjuries < 0)
                currentInjuries = 0;
            currentSickness -= newSickness;
            if (currentSickness < 0)
                currentSickness = 0;
        }
        UpdateCurrentState();
    }
    // After awake actions 
    private void Awake()
    {
        Hero.UpdateState += RequestAtributesUpdate;
        TavernController.UpdateState += ForceSetAtributes;
        Enemy.UpdateState += RequestAtributesUpdate;
    }
    // Check if current state overloading is done, if so, change state variable
    private IEnumerator OverloadController()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (FatigueOverload >= 1.0f)
            {
                FatigueOverload = 0.0f;
                currentFatigue += 1;
                UpdateCurrentState();
                StartCoroutine(ColorBlink(2));
            }
            else if (FatigueOverload <= -1.0f)
            {
                FatigueOverload = 0.0f;
                currentFatigue -= 1;
                UpdateCurrentState();
            }
            if (InjuriesOverload >= 1.0f)
            {
                InjuriesOverload = 0.0f;
                currentInjuries += 1;
                UpdateCurrentState();
                StartCoroutine(ColorBlink(1));
            }
            else if (InjuriesOverload <= -1.0f)
            {
                InjuriesOverload = 0.0f;
                currentInjuries -= 1;
                UpdateCurrentState();
            }
            if (SicknessOverload >= 1.0f)
            {
                SicknessOverload = 0.0f;
                currentSickness += 1;
                UpdateCurrentState();
                StartCoroutine(ColorBlink(3));
            }
            else if (SicknessOverload <= -1.0f)
            {
                SicknessOverload = 0.0f;
                currentSickness -= 1;
                UpdateCurrentState();
            }
        }
    }
    // Request atribute update with a proper parameters
    private void RequestAtributesUpdate(int AtributeIndex, float ChangeValue, float DelayValue = 0) //Fatigue = 1, Injuries = 2, Sickness = 3
    {
        StartCoroutine(StartUpdating(AtributeIndex, ChangeValue, DelayValue));
    }
    // Update state variable
    private IEnumerator StartUpdating(int AtributeIndex, float ChangeValue, float DelayValue)
    {
        yield return new WaitForSeconds(DelayValue);
        if (AtributeIndex == 1)
            FatigueOverload += ChangeValue;
        else if (AtributeIndex == 2)
            InjuriesOverload += ChangeValue;
        else if (AtributeIndex == 3)
            SicknessOverload += ChangeValue;
    }
    // Calculate the nearest state and change the current state
    private void UpdateCurrentState()
    {
        SingleState TempSingleState = new SingleState();
        float TempDistance = 174, CalculatedDistance;
        foreach (SingleState state in States)
        {
            CalculatedDistance = Mathf.Sqrt(((currentFatigue - state.fatigue) * (currentFatigue - state.fatigue)) +
                                            ((currentInjuries - state.injuries) * (currentInjuries - state.injuries)) +
                                             ((currentSickness - state.sickness) * (currentSickness - state.sickness)));

            if (CalculatedDistance < TempDistance)
            {
                TempDistance = CalculatedDistance;
                TempSingleState = state;
            }
        }
        if (CurrentState != null)
            CurrentState.UndoActiveState();
        CurrentState = TempSingleState;
        CurrentState.isActiveState = true;
        CurrentState.UseActiveState();
        UpdateDisplay();
    }
    // Update state UI element
    private void UpdateDisplay()
    {
        DisplayImage.color = new Color(currentFatigue * 0.01f, (100 - currentInjuries) * 0.01f, currentSickness * 0.01f);
        if (DisplayImage.color.r <= 0.5f)
            DisplayImage.color = new Color(0.5f, (100 - currentInjuries) * 0.01f, currentSickness * 0.01f);
        DisplayText.text = CurrentState.StateName;
    }
    // Bling on state UI element
    private IEnumerator ColorBlink(int index) // 1- red, 2 - yellow, 3 - blue
    {
        Color SavedColor = DisplayImage.color;
        if (index == 1)
            DisplayImage.color = new Color(1, 0, 0);
        if (index == 2)
            DisplayImage.color = new Color(0.5f, 0.5f, 0);
        if (index == 3)
            DisplayImage.color = new Color(0, 0, 1);
        yield return new WaitForSeconds(0.3f);
        DisplayImage.color = SavedColor;
    }
    // Creates all available states
    private void createStates()
    {
        SingleState tempState;
        tempState = new SingleState("Healthy", 0, 0, 0, 1, 1, 1, 2, 0);
        States.Add(tempState);
        tempState = new SingleState("Breathless", 20, 0, 0, 0, 0, 0, 0, 0);
        States.Add(tempState);
        tempState = new SingleState("tired", 40, 0, 0, 0, -1, -1, 0, 0);
        States.Add(tempState);
        tempState = new SingleState("Very tired", 60, 0, 0, -2, -3, -2, 0, 0);
        States.Add(tempState);
        tempState = new SingleState("Exhausted", 100, 0, 0, -2, -5, -3, -1, 0);
        States.Add(tempState);
        tempState = new SingleState("Slightly injured", 0, 25, 0, 0, -1, 0, 0, 0);
        States.Add(tempState);
        tempState = new SingleState("Wounded", 0, 50, 0, 0, -2, -1, 0, 0);
        States.Add(tempState);
        tempState = new SingleState("Damaged", 0, 75, 0, -1, -3, -2, 0, -1);
        States.Add(tempState);
        tempState = new SingleState("Mutilated", 0, 100, 0, -3, -3, -4, 0, -2);
        States.Add(tempState);
        tempState = new SingleState("Infected", 0, 0, 20, 1, 0, 0, 0, 0);
        States.Add(tempState);
        tempState = new SingleState("Sick", 0, 0, 40, 1, 1, 1, 1, 0);
        States.Add(tempState);
        tempState = new SingleState("Blighted", 0, 0, 60, 1, 2, 3, 2, -1);
        States.Add(tempState);
        tempState = new SingleState("Contaminated", 0, 0, 80, 3, 4, 3, 3, -1);
        States.Add(tempState);
        tempState = new SingleState("Demented", 0, 0, 100, 5, 6, 4, 5, -5);
        States.Add(tempState);
        tempState = new SingleState("Iritated", 25, 25, 25, 0.5f, -0.5f, 0.5f, 0, 0);
        States.Add(tempState);
        tempState = new SingleState("Motivated", 50, 25, 0, -1, 1, 1, 0, 0);
        States.Add(tempState);
        tempState = new SingleState("Staggered", 25, 75, 0, 1, -2, -1, 0, 0.5f);
        States.Add(tempState);
        tempState = new SingleState("Determined", 75, 75, 0, 3, -3, -1, 0, 1);
        States.Add(tempState);
        tempState = new SingleState("Death door", 100, 100, 0, 2, 2, 2, 0, 1);
        States.Add(tempState);
        tempState = new SingleState("Enraged", 50, 75, 0, 2, 2, -1, 0, -1);
        States.Add(tempState);
        tempState = new SingleState("Excited", 25, 25, 25, 1, 1, 1, 0.5f, 0);
        States.Add(tempState);
        tempState = new SingleState("Durable", 0, 75, 50, -1, -1, -1, 0, 2);
        States.Add(tempState);
        tempState = new SingleState("Scatterbrained", 50, 50, 50, -1, 4, 1, 0, 0);
        States.Add(tempState);
        tempState = new SingleState("Strengthened", 75, 0, 50, 1, 0, 4, 0, 0);
        States.Add(tempState);
        tempState = new SingleState("Frenzied", 75, 75, 75, 3, 3, 3, -1, -1);
        States.Add(tempState);
        tempState = new SingleState("Doomed", 100, 100, 100, 5, 5, 5, 2, -3);
        States.Add(tempState);
    }
    // On disable actions
    private void OnDisable()
    {
        Hero.UpdateState -= RequestAtributesUpdate;
        TavernController.UpdateState -= ForceSetAtributes;
        Enemy.UpdateState -= RequestAtributesUpdate;
    }

}
