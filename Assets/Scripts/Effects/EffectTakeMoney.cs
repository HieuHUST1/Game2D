using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;


public class EffectTakeMoney : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textTakeMoney;
    [SerializeField] private GameObject effectTakeMoneyPrefab;
    [SerializeField] private float timeDestroy = 1f;
    private Transform targetPosition;
    [SerializeField] private float moveSpeed = 2f;

    private UIManager uiManager;
     
    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
    }
    

    private void StartMoveEffect(Vector3 _startPosition)
    {
        Vector3 startPosition = _startPosition;

        Vector3 targetUpPosition = startPosition + new Vector3(0f , -0.15f, 0);

        transform.DOMove(targetUpPosition, timeDestroy)
            .OnComplete(() => MoveToTarget());
    }

    private void MoveToTarget()
    {
        transform.DOMove(targetPosition.position, moveSpeed)
            .SetSpeedBased(true)
            .SetEase(Ease.InExpo)
            .OnComplete(() =>
            {
                uiManager.UpdateTotalMoney(int.Parse(textTakeMoney.text));
                Destroy(gameObject);
            });
    }

    public void SpawnMoneyEffect(Vector3 clientPosition, int amount)
    {
        // GameObject effectObj = Instantiate(effectTakeMoneyPrefab, clientPosition, Quaternion.identity);

        //effectObj.GetComponent<EffectTakeMoney>().SetMoneyEffect(amount * uiManager.profitStat, uiManager.totalMoneyText.transform);

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(clientPosition);


        Vector2 localPoint;


        RectTransform canvasRect = uiManager.canvas.GetComponent<RectTransform>();


        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect, screenPosition, Camera.main, out localPoint);


        GameObject effectObj = Instantiate(effectTakeMoneyPrefab, uiManager.canvas.transform);


        effectObj.GetComponent<RectTransform>().anchoredPosition = localPoint;


        effectObj.GetComponent<EffectTakeMoney>().SetMoneyEffect(amount * uiManager.profitStat, uiManager.totalMoneyText.transform);
    }

    public void SetMoneyEffect(int amount, Transform target)
    {
        textTakeMoney.text = "+" + amount.ToString();
        targetPosition = target;
        StartMoveEffect(target.position);
    }
}
