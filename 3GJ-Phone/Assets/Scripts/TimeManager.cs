using UnityEngine;


public class TimeManager : MonoBehaviour
{
    public Light sun;
    public float dayDuration = 120f;

    private float time;


    private void Update()
    {
       time += Time.deltaTime;

        float angle = (time / dayDuration) * 360f;

        sun.transform.rotation = Quaternion.Euler(angle -90, 170, 0f);

        float t = Mathf.Sin(time / dayDuration * Mathf.PI * 2f) * 0.5f * 0.5f;

        sun.color = Color.Lerp(Color.black, Color.yellow, t);

        sun.intensity = Mathf.Lerp(0.1f, 1f, t);

    }

  

   
}
