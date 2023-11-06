using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMesh;
    [SerializeField] private float moveYSpeed = 20f;
    [SerializeField] private float fadeSpeed = 3f;

    [SerializeField] private float normalFontSize;
    [SerializeField] private Color defaultTextColor = Color.white;

    [SerializeField] private float criticalHitFontSize;
    [SerializeField] private Color criticalHitTextColor;
    [SerializeField] private float timeBeforeFade;

    private Color textcolor;
    public static void Create(Vector2 position, int damageAmount, bool isCriticalStrike)
    {
        GameObject damagePopupObject = Instantiate(ResourceSystem.Instance.damagePopup, position, Quaternion.identity);
        DamagePopup damagePopup = damagePopupObject.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount, isCriticalStrike);

    }

    public void Setup(int damageAmount, bool isCriticalStrike)
    {
        textMesh.SetText(damageAmount.ToString());
        if (!isCriticalStrike)
        {
            textMesh.fontSize = normalFontSize;
            textcolor = defaultTextColor; 
        }
        else
        {
            textMesh.fontSize = criticalHitFontSize;
            textcolor = criticalHitTextColor; //UtilsClass.GetColorFromString("String")  input string color code if wanting specific color
        }
        textcolor = textMesh.color;
    }

    private void Update()
    {     
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;
        timeBeforeFade -= Time.deltaTime;

        if(timeBeforeFade <= 0)
        {
            textcolor.a -= fadeSpeed * Time.deltaTime;
            textMesh.color = textcolor;
            if(textcolor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
