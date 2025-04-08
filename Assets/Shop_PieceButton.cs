using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop_PieceButton : MonoBehaviour
{
    public ShipPieceData pieceData;

    [Header("References")]
    public TMP_Text costText;
    public Image pieceSprite;
    public Button pieceButton;

    public void Apply()
    {
        costText.text = pieceData.pieceCost.ToString();
        pieceSprite.sprite = pieceData.pieceSprite;
    }
}

public enum ShipPieceType
{
    Body,
    Wing,
    Engine,
    Gun
}