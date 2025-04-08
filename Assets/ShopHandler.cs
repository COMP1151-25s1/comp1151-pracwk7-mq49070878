using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
public class ShopHandler : MonoBehaviour
{
    public Transform shopPieceParent;
    public GameObject shopPieceButton;
    public TMP_Text categoryText;

    private Shop_PieceCategory currentCategory;
    private Shop_PieceCategory previousCategory;
    public List<Shop_PieceCategory> pieceCategories;

    private ShipPieceData selectedPieceData;
    public Color normalColor;
    public Color selectedColor;

    public GameObject shopUI;
    public GameObject goButton;

    private void Start()
    {
        OpenShop();
    }

    public void OpenShop()
    {
        GameHandler.SetTimeOverDuration(0f, 0.5f);
        PlayerHandler.allowMovement = false;
        
        shopUI.SetActive(true);
        ChangeSelectedCategory(0);
        CameraHandler.LerpZoom(3.5f, 2.5f);
    }

    public void CloseShop()
    {
        GameHandler.SetTimeOverDuration(1f, 0.5f);
        PlayerHandler.allowMovement = true;
        
        shopUI.SetActive(false);
        CameraHandler.LerpZoom(11f, 2.5f);
    }

    public void ChangeSelectedCategory(int typeCatIndex)
    {
        if (typeCatIndex < 0 || typeCatIndex >= System.Enum.GetValues(typeof(ShipPieceType)).Length)
        {
            return;
        }

        ShipPieceType typeCat = (ShipPieceType)typeCatIndex;
        currentCategory = pieceCategories.Find(category => category.categoryType == typeCat);

        if (currentCategory != null)
        {
            UpdateShopUI();
        }
    }
    
    public void UpdateShopUI()
    {
        if (currentCategory != previousCategory)
        {
            categoryText.text = currentCategory.categoryName;
            
            foreach (Transform child in shopPieceParent)
            {
                Destroy(child.gameObject);
            }
            
            foreach (ShipPieceData data in currentCategory.pieceDatas)
            {
                GameObject newButton = Instantiate(shopPieceButton, shopPieceParent);
                Shop_PieceButton newShopButton = newButton.GetComponent<Shop_PieceButton>();
                newShopButton.pieceData = data;
                newShopButton.pieceButton.onClick.AddListener(() => OnShopPieceButtonClicked(newShopButton));
                newShopButton.Apply();

                newShopButton.GetComponent<Image>().color = newShopButton.pieceData == selectedPieceData ? selectedColor : normalColor;
            }
            
            previousCategory = currentCategory;
        }
        
        // go button
        goButton.SetActive(PlayerShipHandler.Instance.IsShipComplete());
    }

    public void OnShopPieceButtonClicked(Shop_PieceButton button)
    {
        selectedPieceData = button.pieceData;
        
        switch (button.pieceData.shipPieceType)
        {
            case ShipPieceType.Body:
                PlayerShipHandler.Instance.ChangeBody(button.pieceData.pieceObject ,button.pieceData);
                break;
            case ShipPieceType.Wing:
                PlayerShipHandler.Instance.ChangeWing(button.pieceData.pieceObject ,button.pieceData);
                break;
            case ShipPieceType.Engine:
                PlayerShipHandler.Instance.ChangeEngine(button.pieceData.pieceObject ,button.pieceData);
                break;
            case ShipPieceType.Gun:
                PlayerShipHandler.Instance.ChangeGun(button.pieceData.pieceObject ,button.pieceData);
                break;
        }
        
        // go button
        goButton.SetActive(PlayerShipHandler.Instance.IsShipComplete());
    }

}

[System.Serializable]
public class Shop_PieceCategory
{
    public string categoryName;
    public ShipPieceType categoryType;
    public List<ShipPieceData> pieceDatas;
}
