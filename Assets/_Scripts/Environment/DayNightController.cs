using UnityEngine;
using System.Collections;
namespace UnityEngine.PostProcessing
{
    public class DayNightController : MonoBehaviour
    {
        public bool active = true;
        public Light sun;
        public Light moon;
        [HideInInspector]
        [Range(0, 5)]
        public float colorShiftAmplitude = 1.05f;
        [HideInInspector]
        [Range(0, 20)]
        public float colorShiftRange = 7.77f;
        public Color dayColor;
        public Color nightColor;
        public PostProcessingProfile m_profile;
        public float secondsInFullDay = 120f;
        [Range(0, 1)]
        public float currentTimeOfDay = 0;
        [HideInInspector]
        public float timeMultiplier = 1f;

        public bool moonRises = true;
        public float darkNightSpeed = 0.01f;
        public float moonLightSpeed = 0.001f;
        public float nightColorGradingChange = 0.01f;


        public bool doMorningRoutine = false;
        public bool doEveningRoutine = false;
        public bool doDayRoutine = false;

        private ColorGradingModel.Settings colorGradingSettings;

        float sunInitialIntensity;

        void Start()
        {
            sunInitialIntensity = sun.intensity;
            PostProcessingBehaviour behaviour = Camera.main.GetComponent<PostProcessingBehaviour>();
            m_profile = Instantiate(behaviour.profile);
            behaviour.profile = m_profile;
            colorGradingSettings = m_profile.colorGrading.settings;

        }

        void FixedUpdate()
        {
            UpdateColorGrading();
        }

        void Update()
        {
            if(active)
            {
                UpdateSun();
                UpdateMoon();
                currentTimeOfDay += (Time.deltaTime / secondsInFullDay) * timeMultiplier;
                if (currentTimeOfDay >= 1)
                {
                    currentTimeOfDay = 0;
                    doEveningRoutine = false;
                    doMorningRoutine = false;
                }
                // Evening
                else if (currentTimeOfDay > 0.6 && !doEveningRoutine)
                {
                    doEveningStuff();
                    doEveningRoutine = true;
                }
                else if (currentTimeOfDay > 0.27 && !doMorningRoutine)
                {
                    doMorningStuff();
                    doMorningRoutine = true;
                }

            }


        }

        void doEveningStuff()
        {
            moonRises = trueOrFalse();
        }

        void doMorningStuff()
        {
            colorGradingSettings.basic.postExposure = 0f;
            m_profile.colorGrading.settings = colorGradingSettings;
        }

        bool trueOrFalse()
        {
            float random = Random.Range(0f, 1f);
            return random > 0.5f ? true : false;
        }

        void UpdateSun()
        {
            sun.transform.localRotation = Quaternion.Euler((currentTimeOfDay * 360f) - 90, -130, 0);

            Color sunColor = Color.Lerp(nightColor, dayColor, colorShiftAmplitude * Mathf.Sin((currentTimeOfDay + 0.5f) * colorShiftRange));
            //  Debug.Log(sunColor);
            sun.color = sunColor;

            float intensityMultiplier = 1;
            if (currentTimeOfDay <= 0.23f || currentTimeOfDay >= 0.75f)
            {
                intensityMultiplier = 0;
            }
            else if (currentTimeOfDay <= 0.25f)
            {
                intensityMultiplier = Mathf.Clamp01((currentTimeOfDay - 0.23f) * (1 / 0.02f));

            }
            else if (currentTimeOfDay >= 0.73f)
            {
                intensityMultiplier = Mathf.Clamp01(1 - ((currentTimeOfDay - 0.73f) * (1 / 0.02f)));
            }
            sun.intensity = sunInitialIntensity * intensityMultiplier;

        }

        void UpdateColorGrading()
        {
            if (currentTimeOfDay <= 0.27f)
            {
                if (!moonRises)
                {
                    nightColorGradingChange = +Time.deltaTime * darkNightSpeed;
                    colorGradingSettings.basic.postExposure = colorGradingSettings.basic.postExposure + nightColorGradingChange;
                }
                else
                {
                    nightColorGradingChange = +Time.deltaTime * moonLightSpeed;
                    colorGradingSettings.basic.postExposure = colorGradingSettings.basic.postExposure + nightColorGradingChange;
                }
                colorGradingSettings.channelMixer.blue.Set(0, 0, 1.5f - currentTimeOfDay);
                m_profile.colorGrading.settings = colorGradingSettings;
            }

            else if (currentTimeOfDay >= 0.73f)
            {

                // TODO: make this behave along with the timer
                if (!moonRises)
                {
                    nightColorGradingChange = +Time.deltaTime * darkNightSpeed;
                    colorGradingSettings.basic.postExposure = colorGradingSettings.basic.postExposure - nightColorGradingChange;
                }
                else
                {
                    nightColorGradingChange = +Time.deltaTime * moonLightSpeed;
                    colorGradingSettings.basic.postExposure = colorGradingSettings.basic.postExposure - nightColorGradingChange;
                }
                colorGradingSettings.channelMixer.blue.Set(0, 0, Mathf.Clamp(currentTimeOfDay + 0.4f, 1f, 1.5f));
                m_profile.colorGrading.settings = colorGradingSettings;
            }
        }

        void UpdateMoon()
        {
            //Moon 
            if (moonRises && currentTimeOfDay <= 0.23f)
            {
                moon.intensity = Mathf.Clamp01(0.2f - currentTimeOfDay);
            }
            else if (moonRises && currentTimeOfDay >= 0.73f)
            {
                moon.intensity = Mathf.Clamp01(-0.8f + currentTimeOfDay);
            }
            moon.transform.localRotation = Quaternion.Euler( 30, (currentTimeOfDay * 360f) - 90, 0);

        }
    }

}