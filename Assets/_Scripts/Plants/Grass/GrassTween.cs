using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTween : MonoBehaviour {

    [Range(0f, 0.5f)]
    public float scaleChange = 0.05f;
    public float scaleChangeTime = 5f;
    [Range(0f, 5f)]
    public float rotateZto = 3f;
    public float rotationTime = 5f;
    public float initialZ = -3f;
    public float initTimeMax = 2f;
    void Start () {
        gameObject.transform.localRotation = Quaternion.Euler(0f,0f,initialZ);
        float random = Random.Range(0f, initTimeMax);
        Debug.Log(random);
        LeanTween.delayedCall(gameObject, random, StartAnim);
    }

    void StartAnim()
    {
        LeanTween.scale(gameObject, new Vector3(1f + scaleChange, 1f, 1f + scaleChange), scaleChangeTime).setEase(LeanTweenType.easeInOutCubic).setLoopPingPong();
        LeanTween.rotateZ(gameObject, rotateZto, rotationTime).setEase(LeanTweenType.easeInOutCubic).setLoopPingPong();
    }
}
