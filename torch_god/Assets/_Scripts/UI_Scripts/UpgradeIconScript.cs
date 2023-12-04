using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeIconScript : MonoBehaviour
{
    [SerializeField] private TMP_Text levelText;
    private GameObject upgrade;
    private AbilityUpgradeBase upgradeScript;

    private void Start()
    {
       
    }

    public void SetUpgrade(GameObject _upgrade)
    {
        upgrade = _upgrade;
        upgradeScript = upgrade.GetComponent<AbilityUpgradeBase>();
    }

    private void Update()
    {
        if(upgradeScript != null)
        {
            levelText.text = string.Format("Lvl {0}", upgradeScript.currentLevel);
        }
    }



}
