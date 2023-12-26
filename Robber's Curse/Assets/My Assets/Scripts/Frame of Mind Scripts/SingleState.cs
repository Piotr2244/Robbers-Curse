using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* Single state, determines how to change hero atributes
 * depending on current state */
public class SingleState
{
    // Variables
    public string StateName;
    //atributes
    public float fatigue;
    public float injuries;
    public float sickness;

    //effects
    public float strength;
    public float speed;
    public float jump;
    public float ManaRegen;
    public float hpRegen;
    public bool isActiveState = false, atributeDelegateSend = false;
    // Events and delegates
    public delegate void changeHeroAtributes(SingleState instance);
    public delegate void UndoHeroAtributes();
    public static event changeHeroAtributes ChangeState;
    public static event UndoHeroAtributes UndoState;

    // Constructor
    public SingleState(string name, float setFatigue, float setInjuries, float setSickness, float setStrength, float setSpeed, float setJump, float setManaRegen, float setHpRegen)
    {
        this.StateName = name;
        this.fatigue = setFatigue;
        this.injuries = setInjuries;
        this.sickness = setSickness;
        this.strength = setStrength;
        this.speed = setSpeed;
        this.jump = setJump;
        this.ManaRegen = setManaRegen;
        this.hpRegen = setHpRegen;
    }
    // Default constructor
    public SingleState() { }
    // Request changes if state is active
    public void UseActiveState()
    {
        if (isActiveState && !atributeDelegateSend)
        {
            atributeDelegateSend = true;
            ChangeState.Invoke(this);
        }
    }
    // Undo status after being replaced by a new state
    public void UndoActiveState()
    {
        isActiveState = false;
        atributeDelegateSend = false;
        UndoState.Invoke();
    }
}
