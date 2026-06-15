using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Gem : MonoBehaviour
{
    public GameObject[] _listGem;
    public GameObject[] _gemContainers;
    public Button[] _listRemoveGem;

    public Button _buttonGemNormal;
    public Button _buttonGemFighting;
    public Button _buttonGemSteel;
    public Button _buttonGemGrass;
    public Button _buttonGemBug;
    public Button _buttonGemWater;
    public Button _buttonGemIce;
    public Button _buttonGemElectric;
    public Button _buttonGemFire;
    public Button _buttonGemGhost;
    public Button _buttonGemDragon;
    public Button _buttonGemPoison;
    public Button _buttonGemFlying;
    public Button _buttonGemGround;
    public Button _buttonGemRock;
    public Button _buttonGemFairy;
    public Button _buttonGemDark;
    public Button _buttonGemPsychic;
    private string[] _gemSlots;

    public TextMeshProUGUI _damageText;
    public TextMeshProUGUI _defenseText;
    public TextMeshProUGUI _healthText;
    public TextMeshProUGUI _healingText;
    public TextMeshProUGUI _manaText;
    public TextMeshProUGUI _restoreManaText;
    public TextMeshProUGUI _criticalText;
    public TextMeshProUGUI _criticalDamageText;
    public TextMeshProUGUI _dodgeText;
    public TextMeshProUGUI _bloodsuckingText;
    public TextMeshProUGUI _reducedHealingText;
    public TextMeshProUGUI _speedText;
    public TextMeshProUGUI _resistanceText;
    public TextMeshProUGUI _penetrateText;
    public TextMeshProUGUI _attackSpeedText;
    public TextMeshProUGUI _reducedTimeText;
    public TextMeshProUGUI _stunText;

     private void Start()
    {
        _gemSlots = new string[_gemContainers.Length];
        for (int i = 0; i < _gemContainers.Length; i++)
        {
            string savedGem = PlayerPrefs.GetString($"GemSlot{i}", "");
            _gemSlots[i] = savedGem;

            if (!string.IsNullOrEmpty(savedGem))
            {
                GameObject gemPrefab = FindGemPrefabByName(savedGem);
                if (gemPrefab != null)
                {
                    GameObject gemInstance = Instantiate(gemPrefab, _gemContainers[i].transform);
                }
            }
        }

        _buttonGemNormal.onClick.AddListener(() => AddGemToContainer("Normal"));
        _buttonGemFighting.onClick.AddListener(() => AddGemToContainer("Fighting"));
        _buttonGemSteel.onClick.AddListener(() => AddGemToContainer("Steel"));
        _buttonGemGrass.onClick.AddListener(() => AddGemToContainer("Grass"));
        _buttonGemBug.onClick.AddListener(() => AddGemToContainer("Bug"));
        _buttonGemWater.onClick.AddListener(() => AddGemToContainer("Water"));
        _buttonGemIce.onClick.AddListener(() => AddGemToContainer("Ice"));
        _buttonGemElectric.onClick.AddListener(() => AddGemToContainer("Electric"));
        _buttonGemFire.onClick.AddListener(() => AddGemToContainer("Fire"));
        _buttonGemGhost.onClick.AddListener(() => AddGemToContainer("Ghost"));
        _buttonGemDragon.onClick.AddListener(() => AddGemToContainer("Dragon"));
        _buttonGemPoison.onClick.AddListener(() => AddGemToContainer("Poison"));
        _buttonGemFlying.onClick.AddListener(() => AddGemToContainer("Flying"));
        _buttonGemGround.onClick.AddListener(() => AddGemToContainer("Ground"));
        _buttonGemRock.onClick.AddListener(() => AddGemToContainer("Rock"));
        _buttonGemFairy.onClick.AddListener(() => AddGemToContainer("Fairy"));
        _buttonGemDark.onClick.AddListener(() => AddGemToContainer("Dark"));
        _buttonGemPsychic.onClick.AddListener(() => AddGemToContainer("Psychic"));
        for (int i = 0; i < _listRemoveGem.Length; i++)
        {
            int index = i;
            _listRemoveGem[i].onClick.AddListener(() => RemoveGemFromContainer(index));
        }
        PlusProperties();
    }

    void SetPlusProperties()
    {
        PlayerPrefs.SetFloat("PlusGemDamage", 0);
        PlayerPrefs.SetFloat("PlusGemDefense", 0);
        PlayerPrefs.SetFloat("PlusGemHealth", 0);
        PlayerPrefs.SetFloat("PlusGemHealing", 0);
        PlayerPrefs.SetFloat("PlusGemMana", 0);
        PlayerPrefs.SetFloat("PlusGemRestoreMana", 0);
        PlayerPrefs.SetFloat("PlusGemCritical", 0);
        PlayerPrefs.SetFloat("PlusGemCriticalDamage", 0);
        PlayerPrefs.SetFloat("PlusGemDodge", 0);
        PlayerPrefs.SetFloat("PlusGemBloodsucking", 0);
        PlayerPrefs.SetFloat("PlusGemReducedHealing", 0);
        PlayerPrefs.SetFloat("PlusGemSpeed", 0);
        PlayerPrefs.SetFloat("PlusGemResistance", 0);
        PlayerPrefs.SetFloat("PlusGemPenetrate", 0);
        PlayerPrefs.SetFloat("PlusGemAttackSpeed", 0);
        PlayerPrefs.SetFloat("PlusGemReducedTime", 0);
        PlayerPrefs.SetFloat("PlusGemStun", 0);
    }

    void PlusProperties()
    {
        SetPlusProperties();

        for (int i = 0; i < _gemSlots.Length; i++)
        {
            string gemName = _gemSlots[i];
            if (!string.IsNullOrEmpty(gemName))
            {
                if (gemName == "Normal")
                {
                    PlayerPrefs.SetFloat("PlusGemDamage", PlayerPrefs.GetFloat("PlusGemDamage") + 1.25f);
                    PlayerPrefs.SetFloat("PlusGemDefense", PlayerPrefs.GetFloat("PlusGemDefense") + 1.25f);
                    PlayerPrefs.SetFloat("PlusGemHealth", PlayerPrefs.GetFloat("PlusGemHealth") + 50);
                    PlayerPrefs.SetFloat("PlusGemMana", PlayerPrefs.GetFloat("PlusGemMana") + 50);
                    PlayerPrefs.SetFloat("PlusGemCritical", PlayerPrefs.GetFloat("PlusGemCritical") + 0.5f);
                    PlayerPrefs.SetFloat("PlusGemDodge", PlayerPrefs.GetFloat("PlusGemDodge") + 0.5f);
                }
                else if (gemName == "Fighting")
                {
                    PlayerPrefs.SetFloat("PlusGemDamage", PlayerPrefs.GetFloat("PlusGemDamage") + 5);
                }
                else if (gemName == "Steel")
                {
                    PlayerPrefs.SetFloat("PlusGemDefense", PlayerPrefs.GetFloat("PlusGemDefense") + 5);
                }
                else if (gemName == "Grass")
                {
                    PlayerPrefs.SetFloat("PlusGemHealth", PlayerPrefs.GetFloat("PlusGemHealth") + 200);
                }
                else if (gemName == "Bug")
                {
                    PlayerPrefs.SetFloat("PlusGemHealing", PlayerPrefs.GetFloat("PlusGemHealing") + 1);
                }
                else if (gemName == "Water")
                {
                    PlayerPrefs.SetFloat("PlusGemMana", PlayerPrefs.GetFloat("PlusGemMana") + 200);
                }
                else if (gemName == "Ice")
                {
                    PlayerPrefs.SetFloat("PlusGemRestoreMana", PlayerPrefs.GetFloat("PlusGemRestoreMana") + 3);
                }
                else if (gemName == "Electric")
                {
                    PlayerPrefs.SetFloat("PlusGemCritical", PlayerPrefs.GetFloat("PlusGemCritical") + 2.5f);
                }
                else if (gemName == "Fire")
                {
                    PlayerPrefs.SetFloat("PlusGemCriticalDamage", PlayerPrefs.GetFloat("PlusGemCriticalDamage") + 10);
                }
                else if (gemName == "Ghost")
                {
                    PlayerPrefs.SetFloat("PlusGemDodge", PlayerPrefs.GetFloat("PlusGemDodge") + 2.5f);
                }
                else if (gemName == "Dragon")
                {
                    PlayerPrefs.SetFloat("PlusGemBloodsucking", PlayerPrefs.GetFloat("PlusGemBloodsucking") + 2.5f);
                }
                else if (gemName == "Poison")
                {
                    PlayerPrefs.SetFloat("PlusGemReducedHealing", PlayerPrefs.GetFloat("PlusGemReducedHealing") + 5);
                }
                else if (gemName == "Flying")
                {
                    PlayerPrefs.SetFloat("PlusGemSpeed", PlayerPrefs.GetFloat("PlusGemSpeed") + 0.3f);
                }
                else if (gemName == "Ground")
                {
                    PlayerPrefs.SetFloat("PlusGemResistance", PlayerPrefs.GetFloat("PlusGemResistance") + 5);
                }
                else if (gemName == "Rock")
                {
                    PlayerPrefs.SetFloat("PlusGemPenetrate", PlayerPrefs.GetFloat("PlusGemPenetrate") + 5);
                }
                else if (gemName == "Fairy")
                {
                    PlayerPrefs.SetFloat("PlusGemAttackSpeed", PlayerPrefs.GetFloat("PlusGemAttackSpeed") + 0.1f);
                }
                else if (gemName == "Dark")
                {
                    PlayerPrefs.SetFloat("PlusGemReducedTime", PlayerPrefs.GetFloat("PlusGemReducedTime") + 5);
                }
                else if (gemName == "Psychic")
                {
                    PlayerPrefs.SetFloat("PlusGemStun", PlayerPrefs.GetFloat("PlusGemStun") + 2.5f);
                }
            }
        }

        _damageText.text = "Công: +" + PlayerPrefs.GetFloat("PlusGemDamage").ToString();
        _defenseText.text = "Thủ: +" + PlayerPrefs.GetFloat("PlusGemDefense").ToString();
        _healthText.text = "Máu: +" + PlayerPrefs.GetFloat("PlusGemHealth").ToString();
        _healingText.text = "Hồi máu: +" + PlayerPrefs.GetFloat("PlusGemHealing").ToString() + "/s";
        _manaText.text = "Mana: +" + PlayerPrefs.GetFloat("PlusGemMana").ToString();
        _restoreManaText.text = "Hồi mana: +" + PlayerPrefs.GetFloat("PlusGemRestoreMana").ToString() + "/s";
        _criticalText.text = "Bạo kích: +" + PlayerPrefs.GetFloat("PlusGemCritical").ToString() + "%";
        _criticalDamageText.text = "Bạo thương: +" + PlayerPrefs.GetFloat("PlusGemCriticalDamage").ToString() + "%";
        _dodgeText.text = "Né: +" + PlayerPrefs.GetFloat("PlusGemDodge").ToString() + "%";
        _bloodsuckingText.text = "Hút máu: +" + PlayerPrefs.GetFloat("PlusGemBloodsucking").ToString() + "%";
        _reducedHealingText.text = "Giảm hồi máu: +" + PlayerPrefs.GetFloat("PlusGemReducedHealing").ToString() + "%";
        _speedText.text = "Tốc độ di chuyển: +" + PlayerPrefs.GetFloat("PlusGemSpeed").ToString();
        _resistanceText.text = "Kháng hiệu ứng: +" + PlayerPrefs.GetFloat("PlusGemResistance").ToString();
        _penetrateText.text = "Xuyên giáp: +" + PlayerPrefs.GetFloat("PlusGemPenetrate").ToString();
        _attackSpeedText.text = "Tốc độ đánh: +" + PlayerPrefs.GetFloat("PlusGemAttackSpeed").ToString() + "%";
        _reducedTimeText.text = "Giảm hồi chiêu: +" + PlayerPrefs.GetFloat("PlusGemReducedTime").ToString() + "%";
        _stunText.text = "Bất động: +" + PlayerPrefs.GetFloat("PlusGemStun").ToString() + "%";
    }


    void AddGemToContainer(string _nameGem)
    {
        GameObject gemPrefab = FindGemPrefabByName(_nameGem);

        if (gemPrefab != null)
        {
            for (int i = 0; i < _gemContainers.Length; i++)
            {
                if (string.IsNullOrEmpty(_gemSlots[i]))
                {
                    GameObject gemInstance = Instantiate(gemPrefab, _gemContainers[i].transform);
                    _gemSlots[i] = _nameGem;
                    PlayerPrefs.SetString($"GemSlot{i}", _nameGem);
                    break;
                }
            }
        }
        PlusProperties();
    }

    void RemoveGemFromContainer(int containerIndex)
    {
        if (containerIndex >= 0 && containerIndex < _gemContainers.Length)
        {
            Transform container = _gemContainers[containerIndex].transform;

            if (container.childCount > 0)
            {
                GameObject gemToRemove = container.GetChild(0).gameObject;

                string gemName = _gemSlots[containerIndex];
                _gemSlots[containerIndex] = "";
                PlayerPrefs.DeleteKey($"GemSlot{containerIndex}");

                Destroy(gemToRemove);
            }
        }
        PlusProperties();
    }

    GameObject FindGemPrefabByName(string _nameGem)
    {
        foreach (GameObject gemPrefab in _listGem)
        {
            if (gemPrefab.name.Equals(_nameGem, System.StringComparison.OrdinalIgnoreCase))
            {
                return gemPrefab;
            }
        }
        return null;
    }
}