using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraZoomHelper : MonoBehaviour
{

    public CinemachineCamera vcam;
    public float zoomSpeed = 0.5f;


    public void SetZoom(float size)
    {
        StopAllCoroutines();
        StartCoroutine(ZoomRoutine(size));
    }

    private IEnumerator ZoomRoutine(float size)
    {
        float startSize = vcam.Lens.OrthographicSize;
        float t = 0f;

        while (Mathf.Abs(startSize - size) > 0.01f)
        {
            t += Time.deltaTime * zoomSpeed;
            vcam.Lens.OrthographicSize = Mathf.Lerp(startSize, size, t);
            yield return null;
        }
    }
}
