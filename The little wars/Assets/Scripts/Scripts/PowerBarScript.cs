using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBarScript : MonoBehaviour
{
    public Transform[] Bars;
    public int CurrentPower { private set; get; }

    public void Reset()
    {
        foreach (var b in Bars)
        {
            b.gameObject.SetActive(false);
        }
        CurrentPower = 0;
    }

    public void IncrementPower()
    {
        if (CurrentPower < Bars.Length)
        {
            Bars[CurrentPower].gameObject.SetActive(true);
            CurrentPower++;
        }
    }

    // Use this for initialization
    void Start()
    {
        Reset();
    }
}
