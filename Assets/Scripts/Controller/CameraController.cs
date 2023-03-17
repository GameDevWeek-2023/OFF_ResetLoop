using UnityEngine;

namespace Controller
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform followTarget; // Object to be followed
        [SerializeField] private float minX = 1.57f; // Minimum position of camera movement
        [SerializeField] private float maxX = 19.42f; // Maximum position of camera movement
        [SerializeField] private float followSpeed = 2.14f; // Speed of camera follow

        private void Update()
        {
            // Check if follow target is assigned
            if (followTarget != null)
            {
                // Get the current position of the camera
                Vector3 currentPosition = transform.position;

                // Set the target x-position of the camera to the x-position of the follow target
                float targetX = followTarget.position.x;

                // Clamp the target x-position of the camera between the min and max values
                targetX = Mathf.Clamp(targetX, minX, maxX);

                // Calculate the new x-position of the camera using Lerp for smooth transition
                float newX = Mathf.Lerp(currentPosition.x, targetX, followSpeed * Time.deltaTime);

                // Set the new position of the camera
                transform.position = new Vector3(newX, currentPosition.y, currentPosition.z);
            }
        }
    }
}