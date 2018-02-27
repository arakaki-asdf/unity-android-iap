using UnityEngine;
using System.Collections;

public class ShopScene : MonoBehaviour {

    [SerializeField] PurchaseScriptable[] product;

    void Start()
    {
        Purchaser.Instance.initialize(product);
    }
}
