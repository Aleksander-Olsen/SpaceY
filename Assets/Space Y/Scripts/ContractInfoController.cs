using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContractInfoController : MonoBehaviour
{
    [SerializeField]
    private Contract contractInfo;

    [SerializeField]
    private Text incomeText;
    [SerializeField]
    private Text bonusText;
    [SerializeField]
    private Text typeText;
    [SerializeField]
    private Text lifetimeText;
    [SerializeField]
    private Text deadlineText;
    [SerializeField]
    private Text sizeText;

    private ResourceController rCtrl;

    public Contract ContractInfo { get => contractInfo; set => contractInfo = value; }

    private void Awake() {
        rCtrl = GameObject.FindGameObjectsWithTag("Resources")[0].GetComponent<ResourceController>();
    }

    public void Init() {
        if (contractInfo == null) {
            Debug.LogError("Contract is empty.");
            return;
        } 

        incomeText.text = contractInfo.Income.TextFormatting();
        bonusText.text = contractInfo.Bonus.TextFormatting();
        lifetimeText.text = contractInfo.GetTimeText(contractInfo.Lifetime);
        deadlineText.text = contractInfo.GetTimeText(contractInfo.Deadline);
    }

    public void AcceptContract() {
        rCtrl.AcceptContract(this);
    }
}
