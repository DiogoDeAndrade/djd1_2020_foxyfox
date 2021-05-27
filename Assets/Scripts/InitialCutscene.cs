using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialCutscene : MonoBehaviour
{
    [SerializeField]
    Transform startPoint;
    [SerializeField]
    Transform endPoint;
    [SerializeField]
    Player player;
    [SerializeField]
    CameraFollow mainCamera;

    void Start()
    {
        StartCoroutine(CutsceneCR());
    }

    IEnumerator CutsceneCR()
    {
        yield return new WaitForSeconds(0.250f);

        player.enabled = false;

        yield return new WaitForSeconds(2.0f);

        mainCamera.transform.position = startPoint.position;
        mainCamera.SetTarget(null);

        float t = 0.0f;
        while (t <= 1.0f)
        {
            mainCamera.transform.position = Vector3.Lerp(startPoint.position, endPoint.position, t);

            yield return null;

            t += Time.deltaTime / 5.0f;
        }

        yield return new WaitForSeconds(1.0f);
        mainCamera.SetTarget(player.transform);

        player.enabled = true;
    }
}
