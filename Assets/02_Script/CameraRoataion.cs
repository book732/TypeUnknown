using UnityEngine;

public class CameraRoataion : MonoBehaviour
{
    private void CameraRotation()
    {
        Vector3 mousePos = Input.mousePosition;
        if (mousePos.x < 0 || mousePos.x > Screen.width ) return;
        Vector3 targetPos = transform.position;
        
    }
}
