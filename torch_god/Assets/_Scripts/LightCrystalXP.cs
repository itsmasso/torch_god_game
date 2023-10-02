using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightCrystalXP : MonoBehaviour
{
    public delegate void OnReceiveXP(int amount);
    public static event OnReceiveXP onReceiveXP;

    public int xpAmount;

    [SerializeField] 
    private float maxSpeed;

    [SerializeField]
    private AnimationCurve accelerationCurve;

    private float time;
    private Vector2 randomDirection;
    private bool bounceFinished = false;

    private GameObject player;

    private void Awake()
    {
        randomDirection = Random.insideUnitCircle.normalized;
        player = LevelManager.Instance.player;
    }
    private void Start()
    {
        StartCoroutine(Scatter());
    }
    private IEnumerator Scatter()
    {
        //this coroutine handles how long this object should move for before stopping in place.
        bounceFinished = false;
        yield return new WaitForSeconds(0.4f);
        bounceFinished = true;
    }

    private void Update()
    {
        if (!bounceFinished)
            transform.position += (Vector3)randomDirection * 2f * Time.deltaTime;

        //if player touches this xp crystal, fire event and send over xp amount and destroy object
        if (Vector2.Distance(player.transform.position, transform.position) <= 1 && bounceFinished)
        {
            onReceiveXP?.Invoke(xpAmount);
            Destroy(gameObject);
        }

        //when near player, slowly gain speed towards the player
        if (Vector2.Distance(player.transform.position, transform.position) <= player.GetComponent<PlayerBaseScript>().pickUpRange)
        {
            time += Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, accelerationCurve.Evaluate(time) *maxSpeed * Time.deltaTime);

            
        }

    }
}
