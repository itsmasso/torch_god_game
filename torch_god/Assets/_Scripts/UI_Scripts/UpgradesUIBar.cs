using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UpgradesUIBar : MonoBehaviour
{
    [SerializeField] private List<GameObject> upgradeIconPositions;
    [SerializeField] private GameObject canvas;
    private int currentIconPosition = 1;
    private Character currentCharacter;
    [Header("Character Weapon Ability Icons")]
    [SerializeField] private GameObject torchWeaponAbilityIcon;
   
    void Start()
    {
        UpgradeManager.onAttatchUpgrade += AddUpgradeIcon;
        currentCharacter = GameManager.Instance.currentCharacter;
        switch (currentCharacter)
        {
            case Character.TorchCharacter:
                upgradeIconPositions[0].SetActive(false);
                GameObject characterAbility = Instantiate(torchWeaponAbilityIcon, upgradeIconPositions[0].transform.position, Quaternion.identity);
                characterAbility.transform.parent = canvas.transform;
                break;
            case Character.LanternCharacter:
                break;
            default:
                break;
        }   
    }


    public void AddUpgradeIcon(ScriptableUpgrade upgrade, GameObject upgradeObj)
    {
        upgradeIconPositions[currentIconPosition].SetActive(false);
        GameObject abilityUpgrade = Instantiate(upgrade.upgradeIcon, upgradeIconPositions[currentIconPosition].transform.position, Quaternion.identity);
        abilityUpgrade.transform.parent = canvas.transform;
        abilityUpgrade.GetComponent<UpgradeIconScript>().SetUpgrade(upgradeObj);
        currentIconPosition++;
    }

    void Update()
    {
        
    }

    private void OnDestroy()
    {
        UpgradeManager.onAttatchUpgrade -= AddUpgradeIcon;
    }
}
