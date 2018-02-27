using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RestoreButton : MonoBehaviour {

    void Start() {
        GetComponent<Button>().onClick.AddListener(() => {
            Purchaser.Instance.restore();
        });
    }
}
