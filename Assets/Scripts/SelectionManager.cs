
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Drawing;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SelectionManager : MonoBehaviour {
    private enum SelectionState { Selecting, Banning }
    private SelectionState currentState = SelectionState.Banning;
    private List<int> teamABannedCharacters = new List<int>();
    private List<int> teamBBannedCharacters = new List<int>();
    private List<int> teamASelectedCharacters = new List<int>();
    private List<int> teamBSelectedCharacters = new List<int>();
    private int teamASelectionCount = 0;
    private int teamBSelectionCount = 0;
    private int maxSelectionsPerTeam;
    private int teamAClassCCount = 0;
    private int teamAClassBCount = 0;
    private int teamAClassACount = 0;
    private int teamBClassCCount = 0;
    private int teamBClassBCount = 0;
    private int teamBClassACount = 0;
    private bool isTeamATurn = true;
    private int gameMode;
    private Coroutine countdownCoroutine;
    [SerializeField] private Sprite redCrossSprite;
    [SerializeField] private GameObject canvas;
    public Image timerImage;
    public float countdownTime = 15f;
    public GameObject characterSelectionCanvas;
    public GameObject arenaSelectionCanvas;
    public List<Button> characterButtons;
    public TMP_InputField teamAInputField;
    public TMP_InputField teamBInputField;
    public GameObject teamNameEntryCanvas;
    public TextMeshProUGUI instructionText;

    public void Start()
    {
        gameMode = PlayerPrefs.GetInt("GameMode", 2);
        if (gameMode == 2)
        {
            maxSelectionsPerTeam = 1;
        }

        else if (gameMode == 3)
        {
            maxSelectionsPerTeam = 3;
        }

        else
        {
            maxSelectionsPerTeam = 5;
        }

        SetAllButtonsInteractable(true);
        timerImage.fillAmount = 1f;
        ResetAndStartCountdown();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            canvas.SetActive(!canvas.gameObject.activeSelf);
        }
    }

    private IEnumerator StartCountdown()
    {
        float duration = countdownTime;
        float normalizedTime = 1f;
        timerImage.fillAmount = normalizedTime;

        while (normalizedTime > 0f)
        {
            normalizedTime -= Time.deltaTime / duration;
            timerImage.fillAmount = normalizedTime;
            timerImage.color = UnityEngine.Color.Lerp(UnityEngine.Color.red, UnityEngine.Color.green, normalizedTime);
            yield return null;
        }

        timerImage.fillAmount = 0f;
        OnCountdownFinished();
    }

    private void OnCountdownFinished()
    {
        List<int> eligibleCharacters = new List<int>();
        for (int i = 0; i < characterButtons.Count; i++)
        {
            if (CanSelectCharacter(i) && characterButtons[i].interactable)
            {
                eligibleCharacters.Add(i);
            }
        }
        if (eligibleCharacters.Count > 0)
        {
            int randomIndex = eligibleCharacters[Random.Range(0, eligibleCharacters.Count)];
            CharacterSelected(randomIndex);
        }
    }


    public void OnReadyButtonClicked()
    {
        if (AreBothTeamsReady())
        {
            PlayerPrefs.SetString("TeamAName", teamAInputField.text);
            PlayerPrefs.SetString("TeamBName", teamBInputField.text);
            teamNameEntryCanvas.SetActive(false);
            characterSelectionCanvas.SetActive(true);
            UpdateInstructionText($"{(isTeamATurn ? teamAInputField.text : teamBInputField.text)} is banning");
        }
    }

    private bool AreBothTeamsReady()
    {
        return !string.IsNullOrWhiteSpace(teamAInputField.text) && !string.IsNullOrWhiteSpace(teamBInputField.text);
    }

    public void SelectBackground(int backgroundId)
    {
        PlayerPrefs.SetInt("SelectedBackground", backgroundId);
        int gameMode = PlayerPrefs.GetInt("GameMode", 2);
        SceneManager.LoadScene(gameMode);
    }

    private void SetAllButtonsInteractable(bool interactable)
    {
        foreach (var button in characterButtons)
        {
            button.interactable = interactable;
        }
    }

    private int GetSizeOfCharacter(int characterIndex)
    {
        if (characterIndex == 59) return 5;
        if (characterIndex == 58) return 4;
        if (characterIndex == 57 || characterIndex == 56) return 3;
        if (characterIndex == 55 || characterIndex == 54) return 2;
        return 1;
    }

    public void CharacterSelected(int characterIndex)
    {
        if (currentState == SelectionState.Selecting)
        {
            if (CanSelectCharacter(characterIndex))
            {
                SelectCharacter(characterIndex);
                currentState = SelectionState.Banning;
            }

            else
            {
                return;
            }
        }

        else if (currentState == SelectionState.Banning)
        {
            BanCharacter(characterIndex);
            currentState = SelectionState.Selecting;
            isTeamATurn = !isTeamATurn;
        }

        UpdateButtonInteractability();
        CheckSelectionComplete();
        ResetAndStartCountdown();
    }

    private void ResetAndStartCountdown()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }

        countdownCoroutine = StartCoroutine(StartCountdown());
    }

    private void SelectCharacter(int characterIndex)
    {
        if (!CanSelectCharacter(characterIndex)) return;

        var button = characterButtons[characterIndex];

        if (isTeamATurn)
        {
            teamASelectedCharacters.Add(characterIndex);
            teamASelectionCount++;
            IncrementCharacterIndexCount(characterIndex, true);
            button.transform.localScale *= 1.4f;
        }

        else
        {
            teamBSelectedCharacters.Add(characterIndex);
            teamBSelectionCount++;
            IncrementCharacterIndexCount(characterIndex, false);
            button.transform.localScale *= 1.4f;
        }

        button.interactable = false;
        UpdateInstructionText($"{(isTeamATurn ? teamAInputField.text : teamBInputField.text)} is banning");
    }

    private bool CanSelectCharacter(int characterIndex)
    {
        int characterSize = GetSizeOfCharacter(characterIndex);
        bool isSizeAllowed = (gameMode == 2 && characterSize <= 1) || (gameMode == 3 && characterSize <= 3) || (gameMode == 4 && characterSize <= 5);

        if (!isSizeAllowed) return false;

        if (isTeamATurn)
        {
            return characterIndex < 20 ? teamAClassCCount < GetLimit(0, 19) :
                   characterIndex < 40 ? teamAClassBCount < GetLimit(20, 39) :
                                         teamAClassACount < GetLimit(40, 60);
        }
        else
        {
            return characterIndex < 20 ? teamBClassCCount < GetLimit(0, 19) :
                   characterIndex < 40 ? teamBClassBCount < GetLimit(20, 39) :
                                         teamBClassACount < GetLimit(40, 60);
        }
    }

    private void IncrementCharacterIndexCount(int characterIndex, bool isTeamA)
    {
        if (characterIndex < 20)
        {
            if (isTeamA) teamAClassCCount++; else teamBClassCCount++;
        }
        else if (characterIndex < 40)
        {
            if (isTeamA) teamAClassBCount++; else teamBClassBCount++;
        }
        else
        {
            if (isTeamA) teamAClassACount++; else teamBClassACount++;
        }
    }

    private int GetLimit(int startIndex, int endIndex)
    {
        if (startIndex == 0 && endIndex == 19)
        {
            return gameMode == 3 ? 2 : gameMode == 4 ? 3 : 1;
        }

        else if (startIndex == 20 && endIndex == 39)
        {
            return gameMode == 4 ? 2 : 1;
        }

        else if (startIndex == 40 && endIndex == 60)
        {
            return 1;
        }

        return 0;
    }

    private void BanCharacter(int characterIndex)
    {
        if (isTeamATurn)
        {
            teamABannedCharacters.Add(characterIndex);
        }
        else
        {
            teamBBannedCharacters.Add(characterIndex);
        }

        Button button = characterButtons[characterIndex];
        button.interactable = false;
        GameObject crossImageObject = new GameObject("CrossImage");
        Image crossImage = crossImageObject.AddComponent<Image>();
        crossImage.sprite = redCrossSprite;
        crossImage.transform.SetParent(button.transform, false);
        crossImage.rectTransform.anchoredPosition = Vector2.zero;
        crossImage.rectTransform.sizeDelta = button.GetComponent<RectTransform>().sizeDelta;
        UpdateInstructionText($"{(!isTeamATurn ? teamAInputField.text : teamBInputField.text)} is selecting");
    }

    private void UpdateButtonInteractability()
    {
        for (int i = 0; i < characterButtons.Count; i++)
        {
            bool isBanned = teamABannedCharacters.Contains(i) || teamBBannedCharacters.Contains(i);
            bool isSelected = teamASelectedCharacters.Contains(i) || teamBSelectedCharacters.Contains(i);
            characterButtons[i].interactable = !isBanned && !isSelected;
        }
    }

    private void CheckSelectionComplete()
    {
        if (teamASelectionCount >= maxSelectionsPerTeam && teamBSelectionCount >= maxSelectionsPerTeam)
        {
            PlayerPrefs.SetString("TeamACharacters", string.Join(",", teamASelectedCharacters));
            PlayerPrefs.SetString("TeamBCharacters", string.Join(",", teamBSelectedCharacters));
            PlayerPrefs.SetString("TeamAName", teamAInputField.text);
            PlayerPrefs.SetString("TeamBName", teamBInputField.text);
            characterSelectionCanvas.SetActive(false);
            arenaSelectionCanvas.SetActive(true);
        }
    }

    private void UpdateInstructionText(string text)
    {
        if (instructionText != null)
        {
            instructionText.text = text;
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        // If running in a build, quit the application.
        Application.Quit();
#endif
    }
}