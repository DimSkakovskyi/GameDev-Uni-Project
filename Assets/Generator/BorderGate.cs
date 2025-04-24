using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderGate : MonoBehaviour
{
    public Vector3 openOffset = new Vector3(0, 3, 0); // How far it moves when opening
    public float openDuration = 1f;
    public float waitBeforeClose = 2f;

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + openOffset;
    }

    public void OpenAndClose()
    {
        StartCoroutine(OpenCloseRoutine());
    }

    private IEnumerator OpenCloseRoutine()
    {
        // Open
        float elapsed = 0f;
        while (elapsed < openDuration)
        {
            transform.position = Vector3.Lerp(closedPosition, openPosition, elapsed / openDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = openPosition;

        // Wait
        yield return new WaitForSeconds(waitBeforeClose);

        // Close
        elapsed = 0f;
        while (elapsed < openDuration)
        {
            transform.position = Vector3.Lerp(openPosition, closedPosition, elapsed / openDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = closedPosition;
    }
}

