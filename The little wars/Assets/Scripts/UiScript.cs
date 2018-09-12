using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiScript : MonoBehaviour
{
    public GameObject WeaponsPanel;
    public GameObject ShowWeaponsPanelButton;

    public UnityEngine.UI.Text TimerTextScript;
    public GameModel gameModel;


    // Use this for initialization
    void Start()
    {
        gameModel.RoundChangedEvent += (sender, eventArgs) =>
        {
            TimerTextScript.color = gameModel.GetCurrentPlayer().Color;
        };
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        string time = gameModel.GetTime();
        if (time != TimerTextScript.text)
        {
            TimerTextScript.text = time;
        }
    }

    public void ShowWeaponsPanel()
    {
        WeaponsPanel.SetActive(true);
        ShowWeaponsPanelButton.SetActive(false);
    }

    public void HideWeaponsPanel()
    {
        WeaponsPanel.SetActive(false);
        ShowWeaponsPanelButton.SetActive(true);
    }
}
