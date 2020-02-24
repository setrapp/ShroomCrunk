using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro.Examples;

public class TitleEffects : MonoBehaviour
{
    private TMPExampleHackNum2 effects;

    private void Awake()
    {
        effects = GetComponent<TMPExampleHackNum2>();
    }

    public void startStartEffect()
    {
        effects.startEffectOn = true;
    }

    public void stopStartEffect()
    {
        effects.startEffectOn = false;
    }

    public void startQuitEffect()
    {
        effects.quitEffectOn = true;
    }

    public void stopQuitEffect()
    {
        effects.quitEffectOn = false;
    }
}
