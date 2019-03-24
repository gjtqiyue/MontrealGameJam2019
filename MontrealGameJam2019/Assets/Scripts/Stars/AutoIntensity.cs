
using UnityEngine;
using System.Collections;

public class AutoIntensity : MonoBehaviour {

    public Gradient nightDayColor;

    public float maxIntensity = 3f;
    public float minIntensity = 0f;
    public float minPoint = -0.2f;

    public float maxAmbient = 1f;
    public float minAmbient = 0f;
    public float minAmbientPoint = -0.2f;


    public Gradient nightDayFogColor;
    public AnimationCurve fogDensityCurve;
    public float fogScale = 1f;

    public float dayAtmosphereThickness = 0.4f;
    public float nightAtmosphereThickness = 0.33f;

    public Vector3 dayRotateSpeed;
    public Vector3 nightRotateSpeed;

    public bool nightDaySwitch = false;

    float skySpeed = 1f;


    Light mainLight;
    Skybox sky;
    Material skyMat;

    void Start ()
    {

        mainLight = GetComponent<Light>();
        skyMat = RenderSettings.skybox;

    }

    void Update ()
    {
        if (nightDaySwitch)
        {

            float tRange = 1 - minPoint;
            float dot = Mathf.Clamp01((Vector3.Dot(mainLight.transform.forward, Vector3.down) - minPoint) / tRange);
            float i = ((maxIntensity - minIntensity) * dot) + minIntensity;

            mainLight.intensity = i;

            tRange = 1 - minAmbientPoint;
            dot = Mathf.Clamp01((Vector3.Dot(mainLight.transform.forward, Vector3.down) - minAmbientPoint) / tRange);
            i = ((maxAmbient - minAmbient) * dot) + minAmbient;
            RenderSettings.ambientIntensity = i;

            mainLight.color = nightDayColor.Evaluate(dot);
            RenderSettings.ambientLight = mainLight.color;

            RenderSettings.fogColor = nightDayFogColor.Evaluate(dot);
            RenderSettings.fogDensity = fogDensityCurve.Evaluate(dot) * fogScale;

            i = ((dayAtmosphereThickness - nightAtmosphereThickness) * dot) + nightAtmosphereThickness;
            skyMat.SetFloat("_AtmosphereThickness", i);

            if (dot > 0 && mainLight.intensity < 1.5 && nightDaySwitch)
                transform.Rotate(dayRotateSpeed * Time.deltaTime * skySpeed);
            else if (mainLight.intensity < 1.5 && nightDaySwitch)
                transform.Rotate(nightRotateSpeed * Time.deltaTime * skySpeed);

            DayNightSwitch();
        }
      }

      void DayNightSwitch ()
      {

        if (Input.GetKeyDown("space"))
        {
          nightDaySwitch = true;
        }

      }

}
