using UnityEngine;
using DG.Tweening;
using System.Collections;

public class PopUpManager : MBSingleton<PopUpManager>
{
    public GameObject popup;
    [SerializeField] private GameObject pileOfItemParent;
    public RectTransform buttonRectTransform;
    public float animationDuration = 0.5f;
    public float angle = 30f;

    private Vector3 originalScale;
    private Vector3 originalPosition;

    private bool isPopupOpen = false;

    [SerializeField] private Vector3[] initialPosition;
    [SerializeField] private Quaternion[] initialRotation;
    [SerializeField] private int itemCount;

    void Start()
    {
        originalScale = popup.transform.localScale;
        originalPosition = popup.GetComponent<RectTransform>().anchoredPosition;
        popup.SetActive(false);


        initialPosition = new Vector3[itemCount];
        initialRotation = new Quaternion[itemCount];

        for (int i = 0; i < pileOfItemParent.transform.childCount; i++)
        {
            initialPosition[i] = pileOfItemParent.transform.GetChild(i).position;
            initialRotation[i] = pileOfItemParent.transform.GetChild(i).rotation;
        }
    }

    private void Reset()
    {
        for (int i = 0; i < pileOfItemParent.transform.childCount; i++)
        {
            pileOfItemParent.transform.GetChild(i).position = initialPosition[i];
            pileOfItemParent.transform.GetChild(i).rotation = initialRotation[i];
        }
    }

    public void RewardPileOfItem()
    {
        Reset();

        var delay = 0f;

        pileOfItemParent.SetActive(true);

        for (int i = 0; i < pileOfItemParent.transform.childCount; i++)
        {
            pileOfItemParent.transform.GetChild(i).DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);

            pileOfItemParent.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-110, 950f), 1f).SetDelay(delay + 0.5f).SetEase(Ease.OutBack);

            pileOfItemParent.transform.GetChild(i).DOScale(0f, 0.3f).SetDelay(delay + 1f).SetEase(Ease.OutBack);

            delay += 0.1f;
        }
    }

    public void ShowPopup()
    {
        popup.SetActive(true);
        popup.GetComponent<RectTransform>().anchoredPosition = buttonRectTransform.anchoredPosition;
        popup.transform.localScale = Vector3.zero;

        Vector2 targetPosition = originalPosition;
        Vector3 direction = (targetPosition - buttonRectTransform.anchoredPosition).normalized;
        Vector2 offset = Quaternion.Euler(0, 0, angle) * direction;

        popup.GetComponent<RectTransform>().DOAnchorPos(targetPosition + offset, animationDuration).SetEase(Ease.OutBack);
        popup.transform.DOScale(originalScale, animationDuration).SetEase(Ease.OutBack);
    }

    public void HidePopup()
    {
        Vector3 targetPosition = buttonRectTransform.anchoredPosition;
        Vector3 direction = (targetPosition - originalPosition).normalized;
        Vector3 offset = Quaternion.Euler(0, 0, -angle) * direction;

        popup.GetComponent<RectTransform>().DOAnchorPos(targetPosition + offset, animationDuration).SetEase(Ease.InBack).OnComplete(() =>
        {
            popup.SetActive(false);
        });
        popup.transform.DOScale(Vector3.zero, animationDuration).SetEase(Ease.InBack);
    }

    public void TogglePopup()
    {
        if (isPopupOpen)
        {
            HidePopup();
        }
        else
        {
            ShowPopup();
        }

        isPopupOpen = !isPopupOpen;
    }

    
}


