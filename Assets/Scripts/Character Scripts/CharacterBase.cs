using System.Collections;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static Teams;

public abstract class CharacterBase : MonoBehaviour {
    public GameObject missedText;
    public Team team;
    public GameObject selectedCircle;
    public GameObject targetedCircle;
    public GameObject winnerCircle;
    public TextMeshProUGUI healthText;
    public Image healthFill;
    public Image healthBackground;
    public bool isTargetable = true;
    public bool isSelectable = true;
    public bool isDead;
    private int untargetableTurnsRemaining = 0;
    private float maxHealth;
    private int size;
    private char clas;
    private string characterName;
    private float health;
    protected CharacterBase targetEnemy;

    void Start()
    {
        maxHealth = health;
        healthText.text = $"{getHealth()} / {maxHealth}";
        healthFill.color = Color.green;
    }

    private void OnMouseDown()
    {
        if (GameManager.instance.getCurrentTeam() == team)
        {
            if (!GameManager.instance.isPerforming && this.isSelectable)
            {
                GameManager.instance.SelectCharacter(this);
            }
        }

        else if (GameManager.instance.getSelectedCharacter() != null && GameManager.instance.getSelectedCharacter().SetTarget(this))
        {
            GameManager.instance.getSelectedCharacter().PerformSkill();
            GameManager.instance.EndTurn();
        }
    }

    public void setClass(char newClas)
    {
        clas = newClas;
    }

    public char getClas()
    {
        return clas;
    }

    public void setSize(int newSize)
    {
        size = newSize;
    }

    public int getSize()
    {
        return size;
    }

    public void setName(string newName)
    {
        characterName = newName;
    }

    public string getName()
    {
        return characterName;
    }

    public void setHealth(float newHealth)
    {
        health = newHealth;
    }

    public float getHealth()
    {
        if(health < 0)
        {
            health = 0;
        }

        return health;
    }

    public float getMaxHealth()
    {
        return maxHealth;
    }

    public Team getTeam()
    {
        return team;
    }

    public abstract string GetSkillDescription();

    public abstract void PerformSkill();

    public virtual bool SetTarget(CharacterBase target)
    {
        if (target.IsEnemy(this.team) && target.isTargetable == true && GameManager.instance.isPerforming == true)
        {
            targetEnemy = target;
            targetEnemy.ActivateTargetCircle();
            return true;
        }

        return false;
    }

    public virtual void TakeDamage(float damage, CharacterBase attacker)
    {
        health -= damage;
        UpdateHealthUI();
        StartCoroutine(DamageAnimation());
        if (health <= 0) Die(attacker);
    }

    IEnumerator DamageAnimation()
    {
        Vector3 originalPosition = transform.localPosition;
        var originalColor = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = Color.red;
        float shakeDuration = 0.5f;
        float shakeMagnitude = 10f;

        for (float elapsed = 0; elapsed < shakeDuration; elapsed += Time.deltaTime)
        {
            float x = originalPosition.x + Random.Range(-shakeMagnitude, shakeMagnitude);
            float y = originalPosition.y + Random.Range(-shakeMagnitude, shakeMagnitude);

            transform.localPosition = new Vector3(x, y, originalPosition.z);

            yield return null;
        }
        transform.localPosition = originalPosition;
        GetComponent<SpriteRenderer>().color = originalColor;
    }

    public void Heal(float healAmount)
    {
        health += healAmount;
        if(health >= maxHealth) health = maxHealth;
        UpdateHealthUI();
        GameManager.instance.TriggerHealingEffect(transform.position, this);
    }

    public void UpdateHealthUI()
    {
        if (healthFill != null)
        {
            float healthPercentage = getHealth() / maxHealth;
            healthFill.fillAmount = healthPercentage;
            healthText.text = $"{getHealth()} / {maxHealth}";
            Color healthColor = Color.Lerp(Color.red, Color.green, healthPercentage);
            healthFill.color = healthColor;
        }
    }

    protected virtual void Die(CharacterBase killer)
    {
        StartCoroutine(DieAfterDelay(killer));
    }

    private IEnumerator DieAfterDelay(CharacterBase killer)
    {
        yield return new WaitForSeconds(1.5f);
        GameManager.instance.CharacterDied(this);
        GameManager.instance.CheckForGameOver();
    }
    

    public void MakeUntargetableForTurns(int turns)
    {
        isTargetable = false;
        untargetableTurnsRemaining = turns;
    }

    public void DecrementUntargetableTurns()
    {
        if (untargetableTurnsRemaining > 0)
        {
            untargetableTurnsRemaining--;

            if (untargetableTurnsRemaining == 0)
            {
                isTargetable = true;
            }
        }
    }

    public void ShowMissedAndMove()
    {
        if (missedText != null)
        {
            missedText.SetActive(true);
            StartCoroutine(MoveMissedText());
        }
    }

    private IEnumerator MoveMissedText()
    {
        float elapsedTime = 0;

        while (elapsedTime < 1f)
        {
            missedText.transform.localPosition += new Vector3(0, 120f * Time.deltaTime, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        elapsedTime = 0;

        while (elapsedTime < 0.5f)
        {
            missedText.transform.localPosition -= new Vector3(0, 240f * Time.deltaTime, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        missedText.SetActive(false);
    }


    public bool IsEnemy(Team team)
    {
        return this.team != team;
    }

    public void ActivateCircle()
    {
        selectedCircle.SetActive(true);
    }

    public void DeactivateCircle()
    {
        selectedCircle.SetActive(false);
    }

    public void ActivateTargetCircle()
    {
        targetedCircle.SetActive(true);
    }

    public void DeactivateTargetCircle()
    {
        targetedCircle.SetActive(false);
    }

    public void ActivateWinnerCircle()
    {
        winnerCircle.SetActive(true);
    }
}