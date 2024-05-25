using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInformation : MonoBehaviour
{
    [SerializeField] private GameObject infoCanvas;
    [SerializeField] private Image characterInfoImage;
    [SerializeField] private TextMeshProUGUI characterInfoText;
    [SerializeField] private Sprite[] characterSprites;
    [SerializeField] Button[] characterButtons;
    [SerializeField] private Image backgroundImage;

    public void OnCharacterButtonRightClick(int characterIndex)
    {
        if (Input.GetMouseButton(1))
        {
            OpenCharacterInfo(characterIndex);
        }
    }

    public void OpenCharacterInfo(int characterIndex)
    {
        infoCanvas.SetActive(true);
        characterInfoImage.sprite = characterSprites[characterIndex];
        ChangeBackgroundImageColor(characterIndex);

        switch (characterIndex)
        {
            case 0:
                characterInfoText.text = "Josh";
                break;
            case 1:
                characterInfoText.text = "Markov";
                break;
            case 2:
                characterInfoText.text = "Lio";
                break;
            case 3:
                characterInfoText.text = "Kenny";
                break;
            case 4:
                characterInfoText.text = "Tiny";
                break;
            case 5:
                characterInfoText.text = "Andy";
                break;
            case 6:
                characterInfoText.text = "Murph";
                break;
            case 7:
                characterInfoText.text = "Woh";
                break;
            case 8:
                characterInfoText.text = "Harry";
                break;
            case 9:
                characterInfoText.text = "Tull";
                break;
            case 10:
                characterInfoText.text = "Zoi";
                break;
            case 11:
                characterInfoText.text = "Trio";
                break;
            case 12:
                characterInfoText.text = "Bobdiot";
                break;
            case 13:
                characterInfoText.text = "Elektro";
                break;
            case 14:
                characterInfoText.text = "Fortress";
                break;
            case 15:
                characterInfoText.text = "Rose";
                break;
            case 16:
                characterInfoText.text = "Rosie";
                break;
            case 17:
                characterInfoText.text = "Mahmut";
                break;
            case 18:
                characterInfoText.text = "Brody";
                break;
            case 19:
                characterInfoText.text = "Pen";
                break;
            case 20:
                characterInfoText.text = "Po";
                break;
            case 21:
                characterInfoText.text = "Scott";
                break;
            case 22:
                characterInfoText.text = "Sharker";
                break;
            case 23:
                characterInfoText.text = "A";
                break;
            case 24:
                characterInfoText.text = "Bukulele";
                break;
            case 25:
                characterInfoText.text = "Volcano";
                break;
            case 26:
                characterInfoText.text = "White Angel";
                break;
            case 27:
                characterInfoText.text = "Dark Angel";
                break;
            case 28:
                characterInfoText.text = "Gas";
                break;
            case 29:
                characterInfoText.text = "Seen";
                break;
            case 30:
                characterInfoText.text = "Kane";
                break;
            case 31:
                characterInfoText.text = "Lily";
                break;
            case 32:
                characterInfoText.text = "Mark";
                break;
            case 33:
                characterInfoText.text = "Arthur";
                break;
            case 34:
                characterInfoText.text = "Selo.By";
                break;
            case 35:
                characterInfoText.text = "Cask";
                break;
            case 36:
                characterInfoText.text = "Ms. Z";
                break;
            case 37:
                characterInfoText.text = "Chan";
                break;
            case 38:
                characterInfoText.text = "Aren";
                break;
            case 39:
                characterInfoText.text = "Snot";
                break;
            case 40:
                characterInfoText.text = "Crash";
                break;
            case 41:
                characterInfoText.text = "Dayz";
                break;
            case 42:
                characterInfoText.text = "Jane";
                break;
            case 43:
                characterInfoText.text = "Buman";
                break;
            case 44:
                characterInfoText.text = "Jim";
                break;
            case 45:
                characterInfoText.text = "Zelda";
                break;
            case 46:
                characterInfoText.text = "Mr. Acid";
                break;
            case 47:
                characterInfoText.text = "Joey";
                break;
            case 48:
                characterInfoText.text = "Serious Man";
                break;
            case 49:
                characterInfoText.text = "Abortus";
                break;
            case 50:
                characterInfoText.text = "Ouch";
                break;
            case 51:
                characterInfoText.text = "Rough";
                break;
            case 52:
                characterInfoText.text = "Cowboy";
                break;
            case 53:
                characterInfoText.text = "Spooky";
                break;
            case 54:
                characterInfoText.text = "Deadly Twins";
                break;
            case 55:
                characterInfoText.text = "Soul Hunter";
                break;
            case 56:
                characterInfoText.text = "Tiss";
                break;
            case 57:
                characterInfoText.text = "Krueger";
                break;
            case 58:
                characterInfoText.text = "Vioragon";
                break;
            case 59:
                characterInfoText.text = "Ray";
                break;
        }
    }

    private void ChangeBackgroundImageColor(int characterIndex)
    {
        if (characterIndex < 20)
        {
            backgroundImage.color = Color.green; 
        }
        else if (characterIndex < 40)
        {
            backgroundImage.color = Color.blue;
        }
        else
        {
            backgroundImage.color = new Color(1f, 0.64f, 0f);
        }
    }

    public void CloseCharacterInfo()
    {
        infoCanvas.SetActive(false);
    }
}
