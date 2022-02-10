using UnityEngine;

public class SideIndicator : MonoBehaviour
{
    [SerializeField] private Transform suitcaseIndicator;
    [SerializeField] private Camera mainCamera;
    private Vector3 newPos;

    private void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        newPos = mainCamera.WorldToScreenPoint(this.transform.position);
        var currentPos = suitcaseIndicator.position;
        currentPos.y = newPos.y;
        currentPos.x = newPos.x;
        suitcaseIndicator.position = currentPos;
    }
}
