using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Range(0.0f,1.0f)]
    public float time;
    public float fullDayLength;
    public float startTime = 0.4f;
    private float timeRate;
    public Vector3 noon;
    public float nightDuration;
    private float dayDuration;
    private float dayRate;
    private float nightRate;
    public float nightStart;
    public float nightEnd;

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("Other Lighting")]
    public AnimationCurve lightingIntensityMultiplier;
    public AnimationCurve reflectionsIntensityMultiplier;
    // Start is called before the first frame update
    void Start()
    {
        dayDuration = fullDayLength - nightDuration;
        nightRate = (1.0f - nightStart + nightEnd) / nightDuration;
        dayRate = (nightStart - nightEnd) / dayDuration;
        timeRate = 1.0f / fullDayLength;
        time = startTime;
    }

    // Update is called once per frame
    void Update()
    {
        //increment time
        if (time >= nightStart || time <= nightEnd)
            time += Time.deltaTime * nightRate;
        else
            time += Time.deltaTime * dayRate;

        //reset time
        if (time >= 1.0f)
            time = 0.0f;

        //light rotation
        sun.transform.eulerAngles = (time - 0.25f) * noon * 4.0f;
        moon.transform.eulerAngles = (time - 0.75f) * noon * 4.0f;

        //light intensity
        sun.intensity = sunIntensity.Evaluate(time);
        moon.intensity = moonIntensity.Evaluate(time);

        //change color
        sun.color = sunColor.Evaluate(time);
        moon.color = moonColor.Evaluate(time);

        //enable or disable sun
        if (sun.intensity <= 0 && sun.gameObject.activeInHierarchy)
            sun.gameObject.SetActive(false);
        else if (sun.intensity > 0 && !sun.gameObject.activeInHierarchy)
            sun.gameObject.SetActive(true);

        //enable or disable moon
        if (moon.intensity <= 0 && moon.gameObject.activeInHierarchy)
            moon.gameObject.SetActive(false);
        else if (moon.intensity > 0 && !moon.gameObject.activeInHierarchy)
            moon.gameObject.SetActive(true);

        //lighting and reflections intensity
        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionsIntensityMultiplier.Evaluate(time);
    }

    public bool isNight()
    {
        return (time >= nightStart || time <= nightEnd);
    }
}
