using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionManipulator : MonoBehaviour
{
    [Header("������ ������ (���� ����� ������")]
    public float leftCameraSize;
    [Header("������ ������ (���� ����� �������")]
    public float rightCameraSize;

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform.position.x > transform.position.x) //���� ����� �������
            {
                CamController.changeCameraSizeEvent?.Invoke(rightCameraSize);
            }
            else //���� ����� ������
            {
                CamController.changeCameraSizeEvent?.Invoke(leftCameraSize);
            }
        }
    }
}
