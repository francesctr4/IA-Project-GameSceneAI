using UnityEngine;

public class AIVision : MonoBehaviour
{
    public Camera cam;
    public GameObject target;
    public bool playerDetected = false;

    private Plane[] cameraFrustrum;
    private MeshRenderer targetRenderer;

    private void Start()
    {
        targetRenderer = target.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        var targetBounds = targetRenderer.bounds;

        cameraFrustrum = GeometryUtility.CalculateFrustumPlanes(cam);

        if (GeometryUtility.TestPlanesAABB(cameraFrustrum, targetBounds))
        {
            playerDetected = true;
        }
        else
        {
            playerDetected = false;
        }

    }

}
