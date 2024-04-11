using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove, Busy, PartyScreen }

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHUD playerHUD;
    [SerializeField] BattleHUD enemyHUD;
    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] Button backButton;
    [SerializeField] PartyScreen partyScreen;

    public event Action<bool> OnBattleOver;

    BattleState state;
    int selectedMoveIndex;
    bool moveSelected = false;
    int selectedMemberIndex;

    FableParty playerParty;
    Fables wildFables;

    PartyMemberUI[] memberSlots;

    void Start()
    {
        memberSlots = GameObject.FindObjectsOfType<PartyMemberUI>();
        foreach (PartyMemberUI member in memberSlots)
        {
            member.SetSelected(false);
        }

        backButton.onClick.AddListener(OnBackButtonClick);
    }


    public void StartBattle(FableParty playerParty, Fables wildFables)
    {
        this.playerParty = playerParty;
        List<Fables> fables = playerParty.Fables; 
        this.wildFables = wildFables;
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        playerUnit.Setup(playerParty.GetHealthyFable());
        enemyUnit.Setup(wildFables);
        playerHUD.SetData(playerUnit.fables);
        enemyHUD.SetData(enemyUnit.fables);

        partyScreen.Init();

        dialogBox.SetMoveNames(playerUnit.fables.Moves);

        yield return dialogBox.TypeDialog($"A Wild {enemyUnit.fables.Base.FableName} appeared.");

        PlayerAction();
    }

    void PlayerAction()
    {
        state = BattleState.PlayerAction;
        moveSelected = false;
        StartCoroutine(HandlePlayerAction());
    }

    void DisableMoveDetails()
    {
        dialogBox.EnableMoveDetails(false);
        dialogBox.EnableActionSelector(false);
    }

    IEnumerator HandlePlayerAction()
    {
        yield return StartCoroutine(dialogBox.TypeDialog("Choose an action"));
        dialogBox.EnableActionSelector(true);
    }

    void PlayerMove()
    {
        state = BattleState.PlayerMove;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
    }

    IEnumerator PerformPlayerMove()
    {
        state = BattleState.Busy;

        var move = playerUnit.fables.Moves[selectedMoveIndex];
        move.PP--;

        yield return dialogBox.TypeDialog($"{playerUnit.fables.Base.FableName} used {move.Base.Name}");

        playerUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);

        enemyUnit.PlayHitAnimation();
        var damageDetails = enemyUnit.fables.TakeDamage(move, playerUnit.fables);
        yield return enemyHUD.UpdateHP();
        yield return ShowDamageDetails(damageDetails);
        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{enemyUnit.fables.Base.FableName} Fainted");
            enemyUnit.PlayFaintAnimation();


            yield return new WaitForSeconds(2f);
            OnBattleOver(true);
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
        dialogBox.EnableMoveDetails(false);
    }

    IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;

        var move = enemyUnit.fables.GetRandomMove();
        move.PP--;
        yield return dialogBox.TypeDialog($"{enemyUnit.fables.Base.FableName} used {move.Base.Name}");

        enemyUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);

        playerUnit.PlayHitAnimation();
        var damageDetails = playerUnit.fables.TakeDamage(move, enemyUnit.fables);
        yield return playerHUD.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{playerUnit.fables.Base.FableName} Fainted");
            playerUnit.PlayFaintAnimation();


            yield return new WaitForSeconds(2f);

            var nextFable = playerParty.GetHealthyFable();
            if (nextFable != null)
            {
                OpenPartyScreen();
            }
            else
            {
                OnBattleOver(false);
            }

        }
        else
        {
            PlayerAction();
        }
    }

    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if (damageDetails.Critical > 1f)
            yield return dialogBox.TypeDialog("A critical hit!!");

        if (damageDetails.TypeEffectiveness > 1f)
            yield return dialogBox.TypeDialog("It's super effective!!");
        else if (damageDetails.TypeEffectiveness < 1f)
            yield return dialogBox.TypeDialog("It's not very effective!!");
    }

    public void HandleUpdate()
    {
        if (state == BattleState.PlayerAction)
        {
            HandlePlayerAction();
        }
        else if (state == BattleState.PlayerMove)
        {
            HandleMoveSelection();
        }
        else if (state == BattleState.PartyScreen)
        {
            OnFableButtonClicked(selectedMemberIndex);
        }
    }

    void HandleMoveSelection()
    {
        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
        {
            Vector2 inputPosition = (Input.touchCount > 0) ? Input.GetTouch(0).position : (Vector2)Input.mousePosition;
            selectedMoveIndex = dialogBox.CalculateSelectedMoveIndex(inputPosition);

            if (!moveSelected)
            {
                dialogBox.UpdateMoveSelection(selectedMoveIndex, playerUnit.fables.Moves[selectedMoveIndex]);
                dialogBox.EnableMoveDetails(true);
                moveSelected = true;
            }
            else
            {
                ConfirmMoveSelection(selectedMoveIndex);
                DisableMoveDetails();
            }
        }
    }

    IEnumerator SwitchFables(Fables newFable)
    {
        if (playerUnit.fables.HP > 0)
        {
            yield return dialogBox.TypeDialog($"Come back {playerUnit.fables.Base.FableName}");
            playerUnit.PlayFaintAnimation();
            yield return new WaitForSeconds(2f);
        }
        // Set up the new fable
        playerUnit.Setup(newFable);
        playerHUD.SetData(newFable);
        dialogBox.SetMoveNames(newFable.Moves);
        yield return dialogBox.TypeDialog($"Go {newFable.Base.FableName}!");

        // Return to the battle state after switching
        state = BattleState.PlayerAction;
        StartCoroutine(EnemyMove());
    }


    void DeselectAllPartyMembers()
        {
            foreach (PartyMemberUI member in memberSlots)
            {
                member.SetSelected(false);
            }
        }

        public void OnBackButtonClick()
        {
            ResetBattleState();
        }

        void ResetBattleState()
        {
            state = BattleState.PlayerAction;
            moveSelected = false;
            dialogBox.EnableActionSelector(true);
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            dialogBox.EnableMoveDetails(false);
        }

    public void OpenPartyScreen()
    {
        state = BattleState.PartyScreen;
        partyScreen.SetPartyData(playerParty.Fables);
        partyScreen.gameObject.SetActive(true);
        selectedMemberIndex = -1; 
        partyScreen.DeselectAllPartyMembers();
    }


    void ConfirmMoveSelection(int selectedMoveIndex)
    {
        moveSelected = false;
        dialogBox.UpdateMoveDetails(playerUnit.fables.Moves[selectedMoveIndex]);
        dialogBox.EnableDialogText(true);
        StartCoroutine(PerformPlayerMove());

        dialogBox.EnableMoveSelector(false);
    }
  //buttons
    void OnButtonClickRun()
        {
            print("The player has fled the scene!!");
        }
        void OnMoveButtonClick(int moveIndex)
        {
            selectedMoveIndex = moveIndex;
            ConfirmMoveSelection(moveIndex);
            DisableMoveDetails();
        }


    public void OnFableButtonClicked(int selectedIndex)
    {
        if (selectedIndex >= 0 && selectedIndex < playerParty.Fables.Count)
        {
            var selectedMember = playerParty.Fables[selectedIndex];

            if (selectedMember.HP <= 0)
            {
                partyScreen.SetMessageText("You can't send out a Fainted Fable");
                return;
            }

            if (selectedMember == playerUnit.fables)
            {
                partyScreen.SetMessageText("You can't switch with the same Fable");
                return;
            }

            partyScreen.gameObject.SetActive(false); 
            state = BattleState.Busy;
            selectedMemberIndex = selectedIndex; // Update selected member index
            StartCoroutine(SwitchFables(selectedMember));
        }
    }

}
