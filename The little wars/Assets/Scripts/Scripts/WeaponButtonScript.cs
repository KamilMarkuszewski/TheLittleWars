using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using Assets.Scripts.ScriptableObjects;
using UnityEngine;

public class WeaponButtonScript : MonoBehaviour
{
    public WeaponDefinition WeaponDefinition;

    public void SetWeaponAsCurrent()
    {
        GameObjectsProviderHelper.GameModel.SetCurrentWeapon(WeaponDefinition);
    }
}
