using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cone Upgrade/Cones Per Click", fileName = "Cones Per Click")]
public class ConeUpgradePerClick : ConeUpgrade
{
    public override void ApplyUpgrade()
    {
        ConeManager.instance.ConesPerClickUpgrade += UpgradeAmount;
    }
}

