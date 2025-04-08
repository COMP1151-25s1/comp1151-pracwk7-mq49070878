using UnityEngine;
using System.Collections.Generic;

public class PlayerShipHandler : MonoBehaviour
{
    public static PlayerShipHandler Instance;
    public PlayerHandler playerHandler;

    private void Awake()
    {
        // Singleton setup
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }
    
    [HideInInspector] public ShipPieceData bodyData;
    [HideInInspector] public ShipPieceData wingData;
    [HideInInspector] public ShipPieceData engineData;
    [HideInInspector] public ShipPieceData gunData;

    public GameObject bodyObject;
    public List<GameObject> wingObjects = new List<GameObject>();
    public GameObject engineObject;
    public GameObject gunObject;
    
    public List<GameObject> wingPoints = new List<GameObject>();
    public GameObject enginePoint;
    public GameObject gunPoint;

    public bool isShipComplete
    {
        get
        {
            return IsShipComplete();
        }
    }

    public void ChangeBody(GameObject newBody, ShipPieceData newBodyData)
    {
        bodyData = newBodyData;

        // Destroy existing parts when body changes
        DestroyParts();

        if (bodyObject != null)
        {
            Destroy(bodyObject);
        }

        // Instantiate the new body
        bodyObject = Instantiate(newBody, transform);
        Debug.Log(bodyObject.GetComponent<BodyPiece>() != null ? bodyObject.name : "NULL");
        bodyObject.GetComponent<BodyPiece>().shipPieceData = newBodyData;
        UpdateBodyPoints();

        // Recreate parts with existing data if they exist
        if (wingData != null) ChangeWing(wingData.pieceObject, wingData);
        if (engineData != null) ChangeEngine(engineData.pieceObject, engineData);
        if (gunData != null) ChangeGun(gunData.pieceObject, gunData);

        playerHandler.PullDataFromShipHandler();
    }

    private void DestroyParts()
    {
        foreach (var wingObj in wingObjects)
        {
            Destroy(wingObj);
        }
        wingObjects.Clear();

        if (engineObject != null) Destroy(engineObject);

        if (gunObject != null) Destroy(gunObject);
    }

    private void UpdateBodyPoints()
    {
        wingPoints.Clear();
        Transform wingPointsParent = bodyObject.transform.Find("WingPoints");
        if (wingPointsParent != null)
        {
            foreach (Transform point in wingPointsParent)
            {
                wingPoints.Add(point.gameObject);
            }
        }

        Transform enginePointTransform = bodyObject.transform.Find("EnginePoint");
        enginePoint = enginePointTransform != null ? enginePointTransform.gameObject : null;

        Transform gunPointTransform = bodyObject.transform.Find("GunPoint");
        gunPoint = gunPointTransform != null ? gunPointTransform.gameObject : null;
    }

    public void ChangeWing(GameObject newWing, ShipPieceData newWingData)
    {
        wingData = newWingData;

        foreach (var wingObj in wingObjects)
        {
            Destroy(wingObj);
        }
        wingObjects.Clear();

        foreach (var point in wingPoints)
        {
            GameObject wingInstance = Instantiate(newWing, point.transform.position, point.transform.rotation, point.transform);
            wingInstance.GetComponent<WingPiece>().shipPieceData = newWingData;
            wingObjects.Add(wingInstance);
        }
        
        playerHandler.PullDataFromShipHandler();
    }

    public void ChangeEngine(GameObject newEngine, ShipPieceData newEngineData)
    {
        engineData = newEngineData;

        if (engineObject != null)
        {
            Destroy(engineObject);
        }

        if (enginePoint != null)
        {
            engineObject = Instantiate(newEngine, enginePoint.transform.position, enginePoint.transform.rotation, enginePoint.transform);
            engineObject.GetComponent<EnginePiece>().shipPieceData = newEngineData;
        }
        else
        {
            Debug.LogError("Engine point not found on the body.");
        }
        
        playerHandler.PullDataFromShipHandler();
    }

    public void ChangeGun(GameObject newGun, ShipPieceData newGunData)
    {
        gunData = newGunData;

        if (gunObject != null)
        {
            Destroy(gunObject);
        }

        if (gunPoint != null)
        {
            gunObject = Instantiate(newGun, gunPoint.transform.position, gunPoint.transform.rotation, gunPoint.transform);
            gunObject.GetComponent<GunPiece>().shipPieceData = newGunData;
        }
        else
        {
            Debug.LogError("Gun point not found on the body.");
        }
        
        playerHandler.PullDataFromShipHandler();
    }

    public bool IsShipComplete()
    {
        if (bodyObject != null && 
            wingObjects.Count > 0 && 
            engineObject != null && 
            gunObject != null)
        {
            return true;
        }

        return false;
    }

}
