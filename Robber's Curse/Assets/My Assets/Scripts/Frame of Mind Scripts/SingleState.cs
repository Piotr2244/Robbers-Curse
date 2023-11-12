using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleState
{
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

    public delegate void changeHeroAtributes(SingleState instance);
    public delegate void UndoHeroAtributes();
    public static event changeHeroAtributes ChangeState;
    public static event UndoHeroAtributes UndoState;

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
    public SingleState() { }
    public void UseActiveState()
    {
        if (isActiveState && !atributeDelegateSend)
        {
            atributeDelegateSend = true;
            ChangeState.Invoke(this);
        }
    }
    public void UndoActiveState()
    {
        isActiveState = false;
        atributeDelegateSend = false;
        UndoState.Invoke();
    }


}
