using UnityEngine;

[CreateAssetMenu(fileName = "ShipPiece", menuName = "Scriptable Objects/ShipPiece")]
public class ShipPieceData : ScriptableObject
{
    public GameObject pieceObject;
    public Sprite pieceSprite;
    public int pieceCost;
    public ShipPieceType shipPieceType;

    [Header("Gameplay Data")]
    public int pieceHealth;
    public int pieceDefense;
}
