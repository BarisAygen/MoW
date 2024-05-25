using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static Teams;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    private Team currentTeam;
    private List<CharacterBase> characters;
    public CharacterBase[] characterPrefabs;
    private Coroutine turnTimerCoroutine;
    public TextMeshProUGUI winnerTeamText;
    private float turnTime = 30f;
    public Image timerCircle;
    private float maxTurnTime;
    public RectTransform timerCircleTransform;
    public RectTransform backgroundCircleTransform;
    private Color startColor = Color.green;
    private Color endColor = Color.red;
    protected CharacterBase selectedCharacter;
    public GameObject endButton;
    public GameObject endGamePanel;
    public Button rematchButton;
    public Button mainMenuButton;
    public Sprite[] possibleBackgrounds;
    public Image backgroundCanvasImage;
    public GameObject canvas;
    public Sprite graveyardSprite;
    public Button skillButton;
    public TextMeshProUGUI skillButtonText;
    public TextMeshProUGUI instructionText;
    public Button resignButtonForTeamA;
    public Button resignButtonForTeamB;
    public bool isPerforming;
    private string teamAName;
    private string teamBName;
    [SerializeField] private Sprite greenPlusSprite;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        characters = new List<CharacterBase>();
        maxTurnTime = turnTime;
        StartTurnTimer();
    }

    void Start()
    {
        if (UnityEngine.Random.value < 1.0f / 2.0f)
        {
            currentTeam = Team.TeamA;
        }

        else
        {
            currentTeam = Team.TeamB;
        }

        int selectedBackgroundId = PlayerPrefs.GetInt("SelectedBackground", 0);
        if (selectedBackgroundId >= 0 && selectedBackgroundId < possibleBackgrounds.Length)
        {
            backgroundCanvasImage.sprite = possibleBackgrounds[selectedBackgroundId];
        }

        teamAName = PlayerPrefs.GetString("TeamAName", "Team A");
        teamBName = PlayerPrefs.GetString("TeamBName", "Team B");
        string[] teamACharactersIDs = PlayerPrefs.GetString("TeamACharacters", "").Split(',');
        string[] teamBCharactersIDs = PlayerPrefs.GetString("TeamBCharacters", "").Split(',');
        int count = 1;


        foreach (string charID in teamACharactersIDs)
        {
            if (!string.IsNullOrEmpty(charID))
            {
                int id = int.Parse(charID);
                CharacterBase newCharacter = Instantiate(characterPrefabs[id]);
                Transform teamAParent = GameObject.Find("Character Team A" + count).transform;
                newCharacter.transform.SetParent(teamAParent, false);
                newCharacter.transform.localPosition = Vector3.zero;
                newCharacter.team = Team.TeamA;
                characters.Add(newCharacter);
                if (newCharacter.getName() == "Kenny")
                {
                    currentTeam = Team.TeamA;
                }
            }

            count++;
        }

        count = 1;

        foreach (string charID in teamBCharactersIDs)
        {
            if (!string.IsNullOrEmpty(charID))
            {
                int id = int.Parse(charID);
                CharacterBase newCharacter = Instantiate(characterPrefabs[id]);
                Transform teamBParent = GameObject.Find("Character Team B" + count).transform;
                newCharacter.transform.SetParent(teamBParent, false);
                newCharacter.transform.localPosition = Vector3.zero;
                newCharacter.team = Team.TeamB;
                characters.Add(newCharacter);
                if (newCharacter.getName() == "Kenny")
                {
                    currentTeam = Team.TeamB;
                }
            }

            count++;
        }

        resignButtonForTeamA.onClick.AddListener(() => ResignButtonClicked(Team.TeamA));
        resignButtonForTeamB.onClick.AddListener(() => ResignButtonClicked(Team.TeamB));
        UpdateResignButtons();
        MoveTimerCircle();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            endGamePanel.SetActive(!endGamePanel.gameObject.activeSelf);
        }

        CheckForGameOver();
    }

    public void setCurrentTeam(Team newTeam)
    {
        currentTeam = newTeam;
    }

    public Team getCurrentTeam()
    {
        return currentTeam;
    }

    public void setSelectedCharacter(CharacterBase newCharacter)
    {
        selectedCharacter = newCharacter;
    }

    public CharacterBase getSelectedCharacter()
    {
        return selectedCharacter;
    }

    public void SelectCharacter(CharacterBase character)
    {
        if (character.team == currentTeam)
        {
            if (selectedCharacter != null)
            {
                selectedCharacter.DeactivateCircle();
            }

            selectedCharacter = character;
            selectedCharacter.ActivateCircle();
            skillButton.gameObject.SetActive(true);
            skillButtonText.text = character.GetSkillDescription();
        }
    }

    public void OnSkillButtonClicked()
    {
        isPerforming = true;
        ModifyInstructionText();
        if (getSelectedCharacter().getName() == "White_Angel")
        {
            getSelectedCharacter().PerformSkill();
            EndTurn();
        }
    }

    public void ModifyInstructionText()
    {
        // Implement this for each character accordingly
    }

    public void TriggerHealingEffect(Vector3 characterPosition, CharacterBase healedCharacter)
    {
        StartCoroutine(PlayHealingEffect(characterPosition, healedCharacter));
    }

    private IEnumerator PlayHealingEffect(Vector3 characterPosition, CharacterBase healedCharacter)
    {
        var spriteRenderer = healedCharacter.GetComponent<SpriteRenderer>();
        var originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.green;
        GameObject[] symbols = new GameObject[5];

        for (int i = 0; i < 5; i++)
        {
            symbols[i] = new GameObject("HealingSymbol" + i);
            symbols[i].transform.SetParent(canvas.transform, false);
            Image image = symbols[i].AddComponent<Image>();
            image.sprite = greenPlusSprite;
            image.rectTransform.sizeDelta = new Vector2(25, 25);
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(characterPosition);
            float offset = (25f * (i - (5 - 1) / 2.0f));
            symbols[i].transform.position = new Vector3(screenPosition.x + offset, screenPosition.y + 90f);
        }

        Vector3 originalScale = spriteRenderer.transform.localScale;
        Vector3 enlargedScale = originalScale * 1.10f;

        float scaleEndTime = Time.time + 1f;
        float startY = symbols[0].transform.position.y;
        float endY = startY + 40f;

        while (Time.time < scaleEndTime)
        {
            float t = (Time.time - (scaleEndTime - 1f)) / 1f;
            spriteRenderer.transform.localScale = Vector3.Lerp(originalScale, enlargedScale, t);
            for (int i = 0; i < 5; i++)
            {
                float newY = Mathf.Lerp(startY, endY, t);
                symbols[i].transform.position = new Vector3(symbols[i].transform.position.x, newY, symbols[i].transform.position.z);
            }
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        float shrinkEndTime = Time.time + 1f;
        while (Time.time < shrinkEndTime)
        {
            float t = (Time.time - (shrinkEndTime - 1f)) / 1f;
            spriteRenderer.transform.localScale = Vector3.Lerp(enlargedScale, originalScale, t);
            yield return null;
        }

        foreach (GameObject symbol in symbols)
        {
            Destroy(symbol);
        }

        spriteRenderer.color = originalColor;
    }

    public void OnEndTurnButton()
    {
        if (turnTimerCoroutine != null)
        {
            StopCoroutine(turnTimerCoroutine);
            turnTimerCoroutine = null;
        }

        selectedCharacter = null;
        skillButton.gameObject.SetActive(false);
        isPerforming = false;
        instructionText.gameObject.SetActive(false);
        SwitchTurns();
        StartTurnTimer();
    }

    public void EndTurn()
    {
        if (turnTimerCoroutine != null)
        {
            StopCoroutine(turnTimerCoroutine);
            turnTimerCoroutine = null;
        }

        selectedCharacter = null;
        skillButton.gameObject.SetActive(false);
        isPerforming = false;
        instructionText.gameObject.SetActive(false);
        StartCoroutine(DelayNextTurnStart());
    }

    private IEnumerator DelayNextTurnStart()
    {
        yield return new WaitForSeconds(2);
        SwitchTurns();
        StartTurnTimer();
    }

    public void SwitchTurns()
    {
        foreach (CharacterBase character in characters)
        {
            character.DeactivateCircle();
            character.DeactivateTargetCircle();
            character.DecrementUntargetableTurns();
            if (character.isTargetable == false)
            {
                character.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
            }
            else
            {
                character.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            }
        }

        currentTeam = currentTeam == Team.TeamA ? Team.TeamB : Team.TeamA;
        UpdateResignButtons();
        turnTime = 30f;
        MoveTimerCircle();
        StartCoroutine(DisplayTurnIndicator());
    }

    private IEnumerator DisplayTurnIndicator()
    {
        instructionText.text = $"{(currentTeam == Team.TeamA ? teamAName : teamBName)}'s Turn";
        instructionText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        instructionText.gameObject.SetActive(false);
    }

    private void StartTurnTimer()
    {
        if (turnTimerCoroutine != null)
        {
            StopCoroutine(turnTimerCoroutine);
            turnTimerCoroutine = null;
        }

        timerCircle.fillAmount = 1;
        timerCircle.color = startColor;
        turnTimerCoroutine = StartCoroutine(TurnTimer());
    }

    private IEnumerator TurnTimer()
    {
        while (turnTime > 0)
        {
            yield return new WaitForSeconds(1f);
            turnTime--;
            timerCircle.fillAmount = turnTime / maxTurnTime;
            timerCircle.color = Color.Lerp(endColor, startColor, turnTime / maxTurnTime);
        }
        SwitchTurns();
    }

    private void MoveTimerCircle()
    {
        var newPosition = timerCircleTransform.anchoredPosition;
        newPosition.x = currentTeam == Team.TeamB ? Mathf.Abs(newPosition.x) : -Mathf.Abs(newPosition.x);
        timerCircleTransform.anchoredPosition = newPosition;
        backgroundCircleTransform.anchoredPosition += newPosition;
    }

    public void CharacterDied(CharacterBase character)
    {
        GameObject graveyardImageObject = new GameObject("GraveyardImage");
        graveyardImageObject.transform.SetParent(canvas.transform, false);

        Image graveyardImage = graveyardImageObject.AddComponent<Image>();
        graveyardImage.sprite = graveyardSprite;
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(character.transform.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPosition, canvas.GetComponent<Canvas>().worldCamera, out Vector2 localPointInCanvas);
        RectTransform crossRectTransform = graveyardImageObject.GetComponent<RectTransform>();
        crossRectTransform.anchoredPosition = localPointInCanvas;
        crossRectTransform.sizeDelta = new Vector2(150, 150);
        character.isTargetable = false;
        character.isSelectable = false;
        character.isDead = true;
        character.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void CheckForGameOver()
    {
        bool isTeamADead = true;
        bool isTeamBDead = true;

        foreach (CharacterBase character in characters)
        {
            if (character.team == Team.TeamA && character.getHealth() > 0)
                isTeamADead = false;
            if (character.team == Team.TeamB && character.getHealth() > 0)
                isTeamBDead = false;
        }

        if (isTeamADead || isTeamBDead)
        {
            HandleGameOver(isTeamADead);
        }
    }


    private void HandleGameOver(bool isTeamADead)
    {
        if (turnTimerCoroutine != null)
        {
            StopCoroutine(turnTimerCoroutine);
            turnTimerCoroutine = null;
        }

        Team winningTeam = isTeamADead ? Team.TeamB : Team.TeamA;
        string winningTeamTextString = winningTeam == Team.TeamA ? teamAName + " Wins!" : teamBName + " Wins!";
        winnerTeamText.text = winningTeamTextString;
        endGamePanel.SetActive(true);
        endButton.SetActive(false);
        timerCircle.gameObject.SetActive(false);
        foreach (CharacterBase character in characters)
        {
            if (character.team != winningTeam)
            {
                // character.gameObject.SetActive(false);
            }
            else
            {
                character.ActivateWinnerCircle();
            }
        }
    }

    private void UpdateResignButtons()
    {
        resignButtonForTeamA.gameObject.SetActive(currentTeam == Team.TeamA);
        resignButtonForTeamB.gameObject.SetActive(currentTeam == Team.TeamB);
    }

    public void ResignButtonClicked(Team teamResigned)
    {
        HandleGameOver(teamResigned == Team.TeamA);
    }

    public void OnRematch()
    {
        ResetGameState();
        endGamePanel.SetActive(false);
    }

    private void ResetGameState()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
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