using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardsManager : MonoBehaviour
{
    public Button btn_ClaimRewards;
    public TextMeshProUGUI cooldownTimeToNextReward_Text;
    public GameObject claimedRewardsIcon;

    private int rewardIntervalInHours = 24;
    private DateTime nextClaimTime;
    private DateTime lastClaimTime;

    public bool canClaimRewards = false;

    public Action onClaimRewards;


    private void Awake()
    {

    }

    private void Start()
    {
        btn_ClaimRewards.onClick.AddListener(OnClaimRewards);
        LoadLastClaimTime();

        if (!canClaimRewards)
            StartCoroutine(UpdateCooldownCoroutine());
        else
            UpdateUI();
    }

    private void UpdateUI()
    {
        btn_ClaimRewards.interactable = canClaimRewards;
        cooldownTimeToNextReward_Text.text = canClaimRewards
            ? "You can get Rewards"
            : FormatTime(nextClaimTime - DateTime.Now);

        claimedRewardsIcon.SetActive(!canClaimRewards);
    }

    private void OnClaimRewards()
    {
        if (!canClaimRewards) return;

        PopUpManager.Instance.RewardPileOfItem();

        lastClaimTime = DateTime.Now;
        nextClaimTime = lastClaimTime.AddHours(rewardIntervalInHours);

        PlayerPrefs.SetString("LastClaimTime", lastClaimTime.ToString());

        claimedRewardsIcon.SetActive(true);
        canClaimRewards = false;
        btn_ClaimRewards.interactable = false;

        onClaimRewards?.Invoke();  

        StartCoroutine(UpdateCooldownCoroutine());
    }

    private void LoadLastClaimTime()
    {
        string lastTime = PlayerPrefs.GetString("LastClaimTime", DateTime.MinValue.ToString());

        canClaimRewards = DateTime.TryParse(lastTime, out lastClaimTime)
            ? DateTime.Now >= (nextClaimTime = lastClaimTime.AddHours(rewardIntervalInHours))
            : true;

        UpdateUI();
    }

    private string FormatTime(TimeSpan remainingTime)
    {
        return string.Format("{0:D2}h{1:D2}m{2:D2}s",
            remainingTime.Hours, remainingTime.Minutes, remainingTime.Seconds);
    }

    private IEnumerator UpdateCooldownCoroutine()
    {
        while (!canClaimRewards)
        {
            TimeSpan remainingTime = nextClaimTime - DateTime.Now;

            canClaimRewards = remainingTime.TotalSeconds <= 0.1;

            UpdateUI();

            yield return new WaitForSeconds(1);
        }  
    }
}
