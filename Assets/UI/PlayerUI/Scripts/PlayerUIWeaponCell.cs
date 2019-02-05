using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIWeaponCell : MonoBehaviour
{

    [SerializeField] Image cooldownFill;
    [SerializeField] Image weaponIcon;
   
    public void UpdateCooldownFill(object newAmount)
    {
        float percentage = (float)newAmount;
        float convertedPercentage = 1 - percentage;
        this.cooldownFill.fillAmount = convertedPercentage;
    }
}
