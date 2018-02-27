using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "MyGame/Create Product", fileName = "PurchaseScriptable")]
public class PurchaseScriptable : ScriptableObject {
    public string product_id = "";
    public UnityEngine.Purchasing.ProductType type = UnityEngine.Purchasing.ProductType.Consumable;
}
