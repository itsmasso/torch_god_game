using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelBarScript : MonoBehaviour
{
    [SerializeField]
    private Slider levelBarSlider;

    [SerializeField]
    private ScriptablePlayerData playerData;

    [SerializeField]
    private RectTransform fill;
    void Start()
    {

    }

    private void Update()
    {
        //weird fix for NaN bug. Bug is a an unfixed unity bug it seems
        fill.anchoredPosition = Vector2.zero;

        //maybe change to event later
        levelBarSlider.value = (float)playerData.currentExperience / playerData.maxExperience;
    }
}
