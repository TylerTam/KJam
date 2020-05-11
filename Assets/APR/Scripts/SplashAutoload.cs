using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashAutoload : MonoBehaviour
{
    public float CountDown;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Countdown());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator Countdown()
    {
        float duration = CountDown;

        float normalizedTime = 0;
        while (normalizedTime <= duration)
        {
            normalizedTime += Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
