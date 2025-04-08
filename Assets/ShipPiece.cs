using UnityEngine;

public class ShipPiece : MonoBehaviour
{
    public ShipPieceData shipPieceData;
    public PlayerHandler playerHandler;

    public ParticleSystem particles;

    public void TakeDamage(float damage)
    {
        if (playerHandler != null)
            playerHandler = this.GetComponentInParent<PlayerHandler>();
        
        // get damage amount
        float finalDamage = damage;
        finalDamage -= shipPieceData.pieceDefense;
        finalDamage = Mathf.Clamp(finalDamage, 1, Mathf.Infinity);
    }

    public void ActivateParticles(bool active)
    {
        if (particles != null)
        {
            if (active == true) particles.Play();
            else particles.Stop();
        }
    }
}
