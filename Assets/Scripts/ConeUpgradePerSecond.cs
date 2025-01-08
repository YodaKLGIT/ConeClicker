using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Cone Upgrade/Cones Per Second", fileName = "Cones Per Second")]

public class ConeUpgradePerSecond : ConeUpgrade
{
    public override void ApplyUpgrade()
    {
        GameObject go = Instantiate(ConeManager.instance.ConesPerSecondObjToSpawn, Vector3.zero, Quaternion.identity);
        go.GetComponent<ConePerSecondTimer>().ConePerSecond = UpgradeAmount;

        ConeManager.instance.SimpleConePerSecondIncrease(UpgradeAmount);
    }
}
