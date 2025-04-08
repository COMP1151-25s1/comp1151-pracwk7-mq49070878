using UnityEngine;
using System.Collections;

public class CameraHandler : MonoBehaviour
{
    public static CameraHandler Instance; 
    
    public Transform target;
    public MeshRenderer bgRenderer;
    
    public float targetZoom = 5f; 
    private Camera cam;

    private void Awake()
    {
        // singleton setup
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }
    
    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.rotation = Quaternion.Euler(0, 0, target.eulerAngles.z);
        
        bgRenderer.transform.rotation = Quaternion.Euler(0, 0, 0);
        
        bgRenderer.sharedMaterial.mainTextureOffset = new Vector2(transform.position.x/2, transform.position.y/2);
    }

    public static void LerpZoom(float newZoom, float zoomSpeed)
    {
        Instance.StartCoroutine(Instance.ZoomCoroutine(newZoom, zoomSpeed));
    }

    private IEnumerator ZoomCoroutine(float newZoom, float zoomSpeed)
    {
        float currentZoom = cam.orthographicSize; 
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            cam.orthographicSize = Mathf.Lerp(currentZoom, newZoom, elapsedTime); 
            elapsedTime += Time.deltaTime * zoomSpeed;
            yield return null;
        }

        cam.orthographicSize = newZoom; 
    }
}