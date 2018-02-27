using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Purchaser : IStoreListener
{
    private static Purchaser instance = new Purchaser();
    public static Purchaser Instance { get { return instance;  } }
    private Purchaser(){ }

    private static IStoreController controller = null;
    private static IExtensionProvider provider = null;
    private bool IsInitialized { get { return controller != null && provider != null; } }


    public void initialize(PurchaseScriptable[] list)
    {
        if (IsInitialized) { return; }

        var module = StandardPurchasingModule.Instance();
#if UNITY_EDITOR
        module.useFakeStoreUIMode = FakeStoreUIMode.StandardUser;
#endif
        var builder = ConfigurationBuilder.Instance(module);

        foreach (var item in list)
        {
            builder.AddProduct(item.product_id, item.type, new IDs
            {
                {item.product_id, GooglePlay.Name},
                {item.product_id, AppleAppStore.Name},
            });
        }

        // async -> OnInitialized or OnInitializeFailed
        UnityPurchasing.Initialize(this, builder);
    }

    public void buy(string product_id)
    {
        if (!IsInitialized)
        {
            Debug.Log("[ERR] initialize failed.");
            return;
        }

        var product = controller.products.WithID(product_id);
        bool canPurchase = (product != null && product.availableToPurchase);
        if (!canPurchase)
        {
            Debug.Log("[ERR] can't purchase.");
            return;
        }

        // async -> ProcessPurchase or OnPurchaseFailed
        controller.InitiatePurchase(product);
    }

    public void restore()
    {
        if (!IsInitialized)
        {
            Debug.Log("[ERR] initialize failed.");
            return;
        }

        bool isIOS = (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer);
        if (!isIOS)
        {
            Debug.Log("[WARN] not ios.");
            return;
        }

        var apple = provider.GetExtension<IAppleExtensions>();
        apple.RestoreTransactions((result) =>
        {
            Debug.Log("Restore " + result);
        });
    }

    #region IStoreListener
    public void OnInitialized(IStoreController _controller, IExtensionProvider _provider)
    {
        Debug.Log("[SUCCESS] OnInitialized");
        controller = _controller;
        provider = _provider;
    }
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("[ERR] OnInitializeFailed " + error);
    }
    public void OnPurchaseFailed(Product product, PurchaseFailureReason error)
    {
        Debug.Log("[ERR] OnPurchaseFailed " + error);
    }
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Product product = args.purchasedProduct;

        try
        {
            validateReceipt(product);
        }
        catch (Exception e)
        {
            Debug.Log("[ERR] validateReceipt " + e.Message);
        }

        return PurchaseProcessingResult.Pending;
    }
    #endregion

    private void validateReceipt(Product product)
    {
        var isSuccess = true;

        if (!isSuccess)
        {
            Debug.Log("[ERR] validateReceipt");
            return;
        }

        Debug.Log("[SUCCESS]: validateReceipt " + product.definition.id);
        controller.ConfirmPendingPurchase(product);
    }
}