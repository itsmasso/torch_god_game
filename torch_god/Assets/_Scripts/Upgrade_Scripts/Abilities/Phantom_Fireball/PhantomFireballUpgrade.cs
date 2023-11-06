using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomFireballUpgrade : AbilityUpgradeBase, IUpgradeable
{
    [SerializeField] private GameObject phantomFireball;
    [SerializeField] private float spawnInterval;
    [SerializeField] private float spawnPadding;
   
    protected override void Start()
    {
        base.Start();
        StartCoroutine(SpawnPhantomFireball());
    }

    public void LevelUpUpgrade()
    {      
        currentLevel++;
        upgradeScriptable.level = currentLevel;
        Debug.Log("Phantom Fireball Level: " + currentLevel);
        //maybe add cool upgrades besides damage later
        damageAmount = upgradeScriptable.baseDamage + (currentLevel * additionalDamageAmount);
    }

    public ScriptableUpgrade ReturnScriptableObject()
    {
        return upgradeScriptable;
    }

    public GameObject ReturnGameObject()
    {
        return gameObject;
    }

    private Vector2 GenerateRandomPosition()
    {
        float cameraHeight = 2f * Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        float randomX = Random.Range((Camera.main.transform.position.x - cameraWidth / 2) - spawnPadding, (Camera.main.transform.position.x + cameraWidth / 2) - spawnPadding);
        float randomY = Random.Range((Camera.main.transform.position.y - cameraHeight / 2) - spawnPadding, (Camera.main.transform.position.y + cameraHeight / 2) - spawnPadding);

        Vector2 randomPoint = new Vector3(randomX, randomY);
        return randomPoint;
    }

    private IEnumerator SpawnPhantomFireball()
    {
        while (canAttack)
        {
            GameObject newPhantomFireball = Instantiate(phantomFireball, GenerateRandomPosition(), Quaternion.identity);
            PhantomFireballBombs phantomFireballScript = newPhantomFireball.GetComponent<PhantomFireballBombs>();
            phantomFireballScript.damageAmount = GenerateDamageAmount(player.attack + damageAmount);

            yield return new WaitForSeconds(spawnInterval / player.attackSpeed);
        }
    }

}
