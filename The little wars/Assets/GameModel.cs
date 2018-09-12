using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Scripts.ExtensionMethods;
using Assets.Scripts.ObjectsScripts;
using Assets.Scripts;
using System;
using Assets.Scripts.Constants;
using Assets.Scripts.ScriptableObjects;

public class GameModel : MonoBehaviour
{
    public Dictionary<int, Player> Players = new Dictionary<int, Player>();
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

    public void ResetPower()
    {
        PowerBarScript.Reset();
    }

    public int GetPower()
    {
        return PowerBarScript.CurrentPower;
    }

    private CameraFollowScript _cameraFollowScript;
    private CameraFollowScript cameraFollowScript
    {
        get
        {
            if (_cameraFollowScript == null)
            {
                _cameraFollowScript = Camera.main.GetComponent<CameraFollowScript>();
            }
            return _cameraFollowScript;
        }
    }


    private int roundLength;
    private float roundStart;
    private int currentPlayer = -1;
    private int currentUnit = -1;

    public event EventHandler<EventArgs> RoundChangedEvent;




    // Use this for initialization
    void Start()
    {


        var spawns = SpawnsHelper.GetSpawns();

        Players.Add(0, new Player(Color.red, 3, spawns, CharacterPrefab, CharactersParentObject));
        Players.Add(1, new Player(Color.blue, 3, spawns, CharacterPrefab, CharactersParentObject));


        roundLength = 45;

        NewRound();
    }



    public string GetTime()
    {
        int seconds = (int)(roundLength - (Time.time - roundStart));
        var span = new TimeSpan(0, 0, seconds);
        if (seconds < 0)
        {
            NewRound();
        }
        return string.Format("{0}:{1:00}", span.Minutes, span.Seconds);
    }

    public Player GetCurrentPlayer()
    {
        Player player;
        if (Players.TryGetValue(currentPlayer, out player))
        {
            return player;
        }
        return null;
    }

    public Unit GetCurrenUnit()
    {
        Unit unit;
        if (GetCurrentPlayer().Units.TryGetValue(currentUnit, out unit))
        {
            return unit;
        }
        return null;
    }

    public Color GetCurrentPlayerColor()
    {
        return GetCurrentPlayer().Color;
    }

    public void NewRound()
    {
        if (currentUnit >= 0 && currentPlayer >= 0)
        {
            GetCurrenUnit().SetAllowControll(false);
            GetCurrenUnit().SetScopeVisibility(false);
        }
        roundStart = Time.time;
        currentPlayer = (currentPlayer + 1) % Players.Count;
        currentUnit = (currentUnit + 1) % GetCurrentPlayer().Units.Count;
        GetCurrenUnit().SetAllowControll(true);
        GetCurrenUnit().SetScopeVisibility(true);

        cameraFollowScript.target = GetCurrenUnit().UnitTransform;
        PowerBarScript.Reset();
        SetCurrentWeapon(ShootManager.GetNoneWeapon());
        CurrentWeapon = WeaponEnum.None;

        if (RoundChangedEvent != null)
        {
            RoundChangedEvent.Invoke(this, null);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

public class Player
{
    public Color Color;
    public Dictionary<int, Unit> Units = new Dictionary<int, Unit>();

    public Player(Color color, int unitsAmount, List<Vector3> spawns, Transform characterPrefab, Transform charactersParentObject)
    {
        Color = color;

        for (int i = 0; i < unitsAmount; i++)
        {
            var unit = new Unit();
            Units.Add(i, unit);

            unit.Instantiate(characterPrefab, charactersParentObject, SpawnsHelper.GetNextSpawn(spawns), Color);
        }
    }
}


public class Unit
{
    public string Name { private set; get; }
    public int Hp { private set; get; }
    public Transform UnitTransform;

    private CharacterMoveScript _characterMoveScript;
    private CharacterMoveScript characterMoveScript
    {
        get
        {
            if (_characterMoveScript == null)
            {
                _characterMoveScript = UnitTransform.GetComponent<CharacterMoveScript>();
            }
            return _characterMoveScript;
        }
    }

    private SpriteRenderer _spriteRenderer;
    private SpriteRenderer spriteRenderer
    {
        get
        {
            if (_spriteRenderer == null)
            {
                _spriteRenderer = UnitTransform.GetComponentInChildren<SpriteRenderer>();
            }
            return _spriteRenderer;
        }
    }

    public Unit()
    {
        Hp = 100;
        Name = "Some name from pool";
    }

    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }

    public void SetAllowControll(bool allowControll)
    {
        characterMoveScript.AllowControll = allowControll;
    }

    public void SetScopeVisibility(bool visibility)
    {
        characterMoveScript.SetScopeVisibility(visibility);
    }

    public void Instantiate(Transform characterPrefab, Transform charactersParentObject, Vector3 spawnPosition, Color color)
    {
        var createdCharacter = UnityEngine.Object.Instantiate(characterPrefab, charactersParentObject);
        createdCharacter.transform.position = spawnPosition;
        UnitTransform = createdCharacter;

        SetColor(color);
        SetAllowControll(false);
        SetScopeVisibility(false);
    }

}
