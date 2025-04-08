using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerHandler : MonoBehaviour
{
    public static bool allowMovement = true;
    public PlayerShipHandler shipHandler;
    public Rigidbody2D rigidbody;
    
    [HideInInspector] public float currentHP = 100f;
    [HideInInspector] public float maxHP = 100f;
    
    [HideInInspector] public float currentTurbo = 0f;
    [HideInInspector] public float maxTurbo = 5f;

    public float topSpeed = 15f;

    public Slider healthSlider;
    public Slider turboSlider;

    //[HideInInspector] 
    public BodyPiece bodyPiece;
    //[HideInInspector] 
    public List<WingPiece> wingPieces;
    //[HideInInspector] 
    public EnginePiece enginePiece;
    //[HideInInspector] 
    public GunPiece gunPiece;

    public void Start()
    {
        if (shipHandler == null)
            shipHandler = this.GetComponentInChildren<PlayerShipHandler>();
    
     
        currentHP = maxHP = 100f; 
        currentTurbo = 0f; 
        maxTurbo = 5f; 
    
        healthSlider.maxValue = maxHP;
        turboSlider.maxValue = maxTurbo;
    }

    public void FixedUpdate()
    {
        if (!shipHandler.IsShipComplete()) return;
        Movement();
        //GunBehaviour();
    }

    private void Update()
    {
        GunBehaviour();
        
        currentTurbo += Time.deltaTime / 2;
        currentTurbo = Mathf.Clamp(currentTurbo, 0, maxTurbo);
        
        healthSlider.value = currentHP;
        healthSlider.maxValue = maxHP;
        
        turboSlider.value = currentTurbo;
        turboSlider.maxValue = maxTurbo;
    }

    private void Movement()
    {
        float inputX = allowMovement ? Input.GetAxis("Horizontal") : 0;
        float inputY = allowMovement ? Input.GetAxis("Vertical") : 0;
        
        float currentSpeed = rigidbody.linearVelocity.magnitude;
        float finalTopSpeed = topSpeed;
        float finalAccell = (enginePiece.engineAcceleration * Time.deltaTime);

        // turbo
        if (Input.GetKey(KeyCode.LeftShift) && currentTurbo > 0.1f)
        {
            finalAccell *= 2;
            finalAccell *= 2;
            
            currentTurbo -= Time.deltaTime * 1.5f;
        }

        // acceleration
        if (inputY > 0.0f)
        {
            if (currentSpeed < topSpeed) rigidbody.linearVelocity += (Vector2)(transform.up * (finalAccell * inputY)); 
            enginePiece.ActivateParticles(true);
        }

        else if (inputY < 0.0f)
        {
            if (currentSpeed < topSpeed/2) rigidbody.linearVelocity += (Vector2)(transform.up * ((finalAccell/2) * inputY));
            enginePiece.ActivateParticles(true);
        }
        
        else enginePiece.ActivateParticles(false);
        

        if (inputX != 0.0f)
        {
            float rotationPower = 0.0f;

            foreach (WingPiece wingPiece in wingPieces)
            {
                rotationPower += wingPiece.wingRotationSpeed;
            }

            transform.Rotate(0,0, rotationPower * Time.deltaTime * -inputX);
        }
    }

    public void GunBehaviour()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gunPiece.FireButtonPressed();
        }

    }

    public void PullDataFromShipHandler()
    {
        bodyPiece = shipHandler.bodyObject.GetComponent<BodyPiece>();
        
        wingPieces.Clear();
        foreach (GameObject obj in shipHandler.wingObjects)
        {
            wingPieces.Add(obj.GetComponent<WingPiece>());
        }

        enginePiece = shipHandler.engineObject.GetComponent<EnginePiece>();
        gunPiece = shipHandler.gunObject.GetComponent<GunPiece>();
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
    }
}
