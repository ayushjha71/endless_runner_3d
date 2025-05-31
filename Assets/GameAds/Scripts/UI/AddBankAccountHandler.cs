using EndlessCubeRunner.Constant;
using EndlessCubeRunner.Manager;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddBankAccountHandler : MonoBehaviour
{
    [SerializeField]
    private UIManager uimanager;
    [Header("Bank Details")]
    [SerializeField]
    private TMP_InputField accountInputField;
    [SerializeField]
    private TMP_InputField confirmAccountInputField;
    [SerializeField]
    private TMP_InputField ifscCodeInputField;
    [SerializeField]
    private TMP_InputField nameInputField;
    [SerializeField]
    private Button saveBtn;


    private string accountNumber;

    // Start is called before the first frame update
    void Start()
    {
        saveBtn.onClick.AddListener(OnClickSaveBtn);
    }

    private void OnClickSaveBtn()
    {
        if (string.IsNullOrEmpty(accountInputField.text) || string.IsNullOrEmpty(confirmAccountInputField.text) || string.IsNullOrEmpty(ifscCodeInputField.text) || string.IsNullOrEmpty(nameInputField.text))
        {
            Debug.Log("Field Empty");
            return;
        }
        uimanager.DisableAllParentPanel();
        uimanager.ProfilePanel.SetActive(true);
        GetBankAccountData();
    }

    private void GetBankAccountData()
    {
        accountNumber = accountInputField.text;
        PlayerPrefs.SetString(GameAdsConstant.ACCOUNT_NUMBER, accountNumber);
        PlayerPrefs.Save();
    }
}
