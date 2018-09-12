using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Scripts.ExtensionMethods;
using Assets.Scripts.Scripts;
using Assets.Scripts;
using System;
using Assets.Scripts.Constants;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Entities;
using Assets.Scripts.Helpers;

public class GameModel : MonoBehaviour
{
    public List<Player> Players = new List<Player>();
    public Transform CharacterPrefab;
    public Transform CharactersParentObject;
    public UnityEngine.UI.Image CurrentWeaponUiImage;
    public PowerBarScript PowerBarScript;

    public WeaponEnum CurrentWeapon;

    public void SetCurrentWeapon(WeaponDefinition definition)
    {
        CurrentWeapon = definition.weaponEnum;
        CurrentWeaponUiImage.sprite = definition.sprite;
    }

    public void IncrementPower()
    {
        if (CurrentWeapon != WeaponEnum.None)
        {
            PowerBarScript.IncrementPower();
        }
    }

    public void ChangeHp(Unit unit, int amount)
    {
        unit.ChangeHp(amount);
        if (unit.Hp < 1)
        {
            Kill(unit);
        }
    }

    private void Kill(Unit unit)
    {
        Debug.Log("Kill");
        Players.First(p => p.Color == unit.Color).Units.Remove(unit);
        unit.UnitTransform.gameObject.SetActive(false);
        if (unit.AllowControll)
        {
            NewRound();
        }
    }

    public void ResetPower()
    {
        PowerBarScript.Reset();
    }

    public int GetPower()
    {
        return PowerBarScript.CurrentPower;
    }

    private int roundLength;
    private float roundStart;
    private bool timeFrozen = true;
    private int currentPlayer = -1;
    private int currentUnit = -1;

    public event EventHandler<EventArgs> RoundChangedEvent;




    // Use this for initialization
    void Start()
    {
        var spawns = SpawnsHelper.GetSpawns();

        Players.Add(new Player(Color.red, 3, spawns, CharacterPrefab, CharactersParentObject));
        Players.Add(new Player(Color.blue, 3, spawns, CharacterPrefab, CharactersParentObject));


        roundLength = 434435;

        NewRound();
    }



    public string GetTime()
    {
        int seconds = (int)(roundLength - (Time.time - roundStart));
        if (timeFrozen)
        {
            seconds = 0;
        }

        var span = new TimeSpan(0, 0, seconds);
        if (seconds < 0)
        {
            NewRound();
        }
        return string.Format("{0}:{1:00}", span.Minutes, span.Seconds);
    }

    public Player GetCurrentPlayer()
    {
        return Players[currentPlayer];
    }

    public Unit GetCurrenUnit()
    {
        return GetCurrentPlayer().Units[currentUnit];
    }

    public Color GetCurrentPlayerColor()
    {
        return GetCurrentPlayer().Color;
    }

    public void NewRound()
    {
        Players.Remove(Players.FirstOrDefault(p => p.Units.Count == 0));
        if (Players.Count == 1)
        {
            Debug.Log("Player " + Players[0].Color + " won! ");
        }
        else if (Players.Count == 0)
        {
            Debug.Log("Cthulhu ftaghn! ");
        }
        else
        {
            StartCoroutine(RoundCoroutine());
        }
    }

    private IEnumerator RoundCoroutine()
    {
        RoundEnd();
        yield return new WaitForSeconds(2);
        RoundStart();
    }

    private void RoundEnd()
    {
        if (currentUnit >= 0 && currentPlayer >= 0 && GetCurrenUnit() != null)
        {
            GetCurrenUnit().SetAllowControll(false);
            GetCurrenUnit().SetScopeVisibility(false);
        }
        timeFrozen = true;
    }

    private void RoundStart()
    {
        timeFrozen = false;
        roundStart = Time.time;
        currentPlayer = (currentPlayer + 1) % Players.Count;
        currentUnit = (currentUnit + 1) % GetCurrentPlayer().Units.Count;
        GetCurrenUnit().SetAllowControll(true);
        GetCurrenUnit().SetScopeVisibility(true);

        CameraHelper.SetCameraFollowAndRotationTarget(GetCurrenUnit().UnitTransform);

        PowerBarScript.Reset();
        SetCurrentWeapon(ShootHelper.GetNoneWeapon());
        CurrentWeapon = WeaponEnum.None;

        if (RoundChangedEvent != null)
        {
            RoundChangedEvent.Invoke(this, null);
        }
    }

}



