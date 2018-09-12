using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.ScriptableObjects;
using UnityEngine;

public class WeaponButtonScript : MonoBehaviour
{
    public WeaponDefinition WeaponDefinition;

    private GameModel _gameModel;
    private GameModel GameModel
    {
        get
        {
            if (_gameModel == null)
            {
                _gameModel = GameObject.Find("GameModel").GetComponent<GameModel>();
            }
            return _gameModel;
        }
    }

    public void SetWeaponAsCurrent()
    {
        GameModel.SetCurrentWeapon(WeaponDefinition);
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
