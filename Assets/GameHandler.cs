using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance;

    private void Awake()
    {
        // singleton setup
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }

    public static int credits;
    public int startingCredits = 500;
    public TMP_Text creditsText;


    private static Coroutine timescaleCoroutine;
    
    
    private void Start()
    {
        credits = startingCredits;
    }

    private void Update()
    {
        creditsText.text = credits.ToString();
    }

    public static void SetTimeOverDuration(float timeScale, float duration)
    {
        if (timescaleCoroutine != null) Instance.StopCoroutine(timescaleCoroutine);
        timescaleCoroutine = Instance.StartCoroutine(Instance.I_SetTimeOverDuration(timeScale, duration));
    }

    public IEnumerator I_SetTimeOverDuration(float timeScale, float duration)
    {
        float timePassed = 0.0f;
        float cachedTimeScale = Time.timeScale;
        
        yield return null;

        while (timePassed < duration)
        {
            timePassed += Time.unscaledDeltaTime;
            yield return null;
            
            Time.timeScale = Mathf.Lerp(timeScale, cachedTimeScale, timePassed / duration);
        }

        yield return null;
    }
    
}
