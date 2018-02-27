using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PurchaseButton : MonoBehaviour {

    [SerializeField] Text text;
    [SerializeField] PurchaseScriptable scriptable;

    void Start()
    {
        var ary = scriptable.product_id.Split('.');
        var name = ary[ary.Length - 1];
        text.text = name;

        GetComponent<Button>().onClick.AddListener(() => {
            Purchaser.Instance.buy(scriptable.product_id);
        });
    }
}
