using Cinemachine;
using System;
using System.Collections;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public static Action<float, float, float> cameraShake;
    public static Action<float> changeCameraSizeEvent;
    public static Action<Transform> changeFollowTargetEvent;

    [HideInInspector] public CinemachineFramingTransposer transposer;
    private CinemachineBasicMultiChannelPerlin channelPerlin;

    private CinemachineVirtualCamera cam;

    private float camSize;
    public float leftOffset, rightOffset;
    private float targetScreenX;
    private float targetScreenY;
    public float botOffset, topOfsett;

    void OnEnable()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        transposer = cam.GetCinemachineComponent<CinemachineFramingTransposer>();
        channelPerlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cameraShake += shake;
        changeCameraSizeEvent += changeCameraSize;
        changeFollowTargetEvent += changeFollowTarget;
    }
    void OnDisable()
    {
        cameraShake -= shake;
        changeCameraSizeEvent -= changeCameraSize;
        changeFollowTargetEvent -= changeFollowTarget;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            targetScreenX = leftOffset;
            StopCoroutine(MoveCameraToOffset());
            StartCoroutine(MoveCameraToOffset());
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            targetScreenX = rightOffset;
            StopCoroutine(MoveCameraToOffset());
            StartCoroutine(MoveCameraToOffset());
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            targetScreenY = topOfsett;
            StopCoroutine(MoveCameraToOffset());
            StartCoroutine(MoveCameraToOffset());
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            targetScreenY = botOffset;
            StopCoroutine(MoveCameraToOffset());
            StartCoroutine(MoveCameraToOffset());
        }
    }

    private IEnumerator MoveCameraToOffset()
    {
        float elapsedTime = 0f;
        float duration = 1f; // “ривал≥сть плавного перем≥щенн€ камери
        float startingScreenX = transposer.m_ScreenX;

        while (elapsedTime < duration)
        {
            transposer.m_ScreenX = Mathf.Lerp(startingScreenX, targetScreenX, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transposer.m_ScreenX = targetScreenX;
    }

    void shake(float strength, float time, float fadeTime)
    {
        StartCoroutine(shakeCam(strength, time, fadeTime));
    }

    void changeCameraSize(float newSize)
    {
        StopCoroutine(changeSize(newSize));
        camSize = cam.m_Lens.OrthographicSize;
        StartCoroutine(changeSize(newSize));
    }

    void changeFollowTarget(Transform followObject)
    {
        if (followObject != null) cam.m_Follow = followObject;
    }

    private IEnumerator changeSize(float newSize)
    {
        if (cam.m_Lens.OrthographicSize == newSize) yield break; // якщо новий розм≥р не в≥др≥зн€Їтьс€ в≥д старого, функц≥€ в≥дм≥н€Їтьс€

        for (float i = 0; i < 1f; i += Time.deltaTime)
        {
            cam.m_Lens.OrthographicSize = Mathf.Lerp(camSize, newSize, EaseInOut(i));
            yield return null;
        }
    }

    private IEnumerator shakeCam(float strength, float time, float fadeTime)
    {
        float originStrength = strength;
        channelPerlin.m_AmplitudeGain = strength;

        yield return new WaitForSeconds(time);

        for (float i = 0; i < fadeTime; i += Time.deltaTime)
        {
            strength -= Time.deltaTime * originStrength / fadeTime;
            channelPerlin.m_AmplitudeGain = strength;
            yield return null;
        }
        channelPerlin.m_AmplitudeGain = 0;
    }

    float EaseInOut(float x)
    {
        return x < 0.5 ? x * x * 2 : (1 - (1 - x) * (1 - x) * 2);
    }
}
