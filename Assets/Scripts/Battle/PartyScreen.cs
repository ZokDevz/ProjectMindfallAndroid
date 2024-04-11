using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class PartyScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI messageText;
    [SerializeField] Button backButton;
    [SerializeField] GameObject partyButtonContainer;
    PartyMemberUI[] memberSlots;

    List<Fables> fables;
    int selectedMemberIndex = -1;

    public void Init()
    {
        Debug.Log("PartyScreen initialized.");
        memberSlots = GetComponentsInChildren<PartyMemberUI>();
    }

    public void SetPartyData(List<Fables> fables)
    {
        this.fables = fables;

        for (int i = 0; i < memberSlots.Length; i++)
        {
            if (i < fables.Count)
            {
                memberSlots[i].gameObject.SetActive(true); // Ensure the slot is active
                memberSlots[i].SetData(fables[i]); // Set fable data for the slot
            }
            else
            {
                memberSlots[i].gameObject.SetActive(false); // Disable the slot if no fable data
            }
        }

        messageText.text = "Choose a Fable";

        foreach (var fable in fables)
        {
            Debug.Log("Fable Name: " + fable.Base.FableName); // Added debug log
        }
    }


    public void UpdateMemberSelection(int selectedMember)
    {
        DeselectAllPartyMembers();
        if (selectedMember >= 0 && selectedMember < memberSlots.Length)
        {
            memberSlots[selectedMember].SetSelected(true);
        }
    }

    public void SetMessageText(string message)
    {
        messageText.text = message;
    }

    public void DeselectAllPartyMembers()
    {
        foreach (PartyMemberUI member in memberSlots)
        {
            member.SetSelected(false);
        }
    }

    public void ShowPartyScreen()
    {
        gameObject.SetActive(true);
        partyButtonContainer.SetActive(true);
        ShowBackButton();
        HideBackButtonInBattle();
    }

    public void HidePartyScreen()
    {
        gameObject.SetActive(false);
        partyButtonContainer.SetActive(false);
        HideBackButton();
        ShowBackButtonInBattle();
    }

    void ShowBackButton()
    {
        if (backButton != null)
        {
            backButton.gameObject.SetActive(true);
        }
    }

    void HideBackButton()
    {
        if (backButton != null)
        {
            backButton.gameObject.SetActive(false);
        }
    }

    void HideBackButtonInBattle()
    {
        if (backButton != null)
        {
            backButton.gameObject.SetActive(false);
        }
    }

    void ShowBackButtonInBattle()
    {
        if (backButton != null)
        {
            backButton.gameObject.SetActive(true);
        }
    }

    public void OnBackButtonClick()
    {
        HidePartyScreen();
    }

    public void SelectPartyMember(int selectedIndex)
    {
        if (selectedIndex >= 0 && selectedIndex < memberSlots.Length)
        {
            selectedMemberIndex = selectedIndex;
            for (int i = 0; i < memberSlots.Length; i++)
            {
                memberSlots[i].SetSelected(i == selectedIndex);
            }
        }
    }

    public PartyMemberUI[] GetMemberSlots()
    {
        return memberSlots;
    }
}
