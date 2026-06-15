using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Spell : MonoBehaviour
{
    public GameObject[] _listSpell;
    public GameObject _spellContainers;

    public Button _spellHealth;
    public Button _spellMana;
    public Button _spellDash;
    public Button _spellTripleJump;
    public Button _spellResistance;
    public Button _spellShield;
    public Button _spellFinishingBlow;
    public Button _spellBloodThirsty;
    public Button _spellStandardDamage;

    public TextMeshProUGUI _textTitleSpell;
    public TextMeshProUGUI _textDescriptionSpell;
    public TextMeshProUGUI _textTimeCooldown;

    public const float _timeCooldownHealth = 100;
    public const float _timeCooldownMana = 80;
    public const float _timeCooldownDash = 15;
    public const float _timeCooldownTripleJump = 0;
    public const float _timeCooldownResistance = 100;
    public const float _timeCooldownShield = 80;
    public const float _timeCooldownFinishingBlow = 120;
    public const float _timeCooldownBloodThirsty = 120;
    public const float _timeCooldownStandardDamage = 120;
    public const float _valueHealth = 20;
    public const float _valueMana = 20;
    public const float _valueResistance = 10;
    public const float _timeResistance = 5;
    public const float _valueShield = 50;
    public const float _timeShield = 5;
    public const float _valueFinishingBlow = 200;
    public const float _timeFinishingBlow = 5;
    public const float _valueCritical = 20;
    public const float _valueBloodThirsty = 20;
    public const float _timeBloodThirsty = 5;
    public const float _timeStandardDamage = 7;

    private float _currentTimeCooldownSpell;
    private string _currentTitleSpell;
    private string _currentDescriptionSpell;

    void Start()
    {
        LoadSpell();
        _spellHealth.onClick.AddListener(() => AddSpellToContainer("Health"));
        _spellMana.onClick.AddListener(() => AddSpellToContainer("Mana"));
        _spellDash.onClick.AddListener(() => AddSpellToContainer("Dash"));
        _spellTripleJump.onClick.AddListener(() => AddSpellToContainer("TripleJump"));
        _spellResistance.onClick.AddListener(() => AddSpellToContainer("Resistance"));
        _spellShield.onClick.AddListener(() => AddSpellToContainer("Shield"));
        _spellFinishingBlow.onClick.AddListener(() => AddSpellToContainer("FinishingBlow"));
        _spellBloodThirsty.onClick.AddListener(() => AddSpellToContainer("BloodThirsty"));
        _spellStandardDamage.onClick.AddListener(() => AddSpellToContainer("StandardDamage"));
    }

    void LoadSpell()
    {
        if(PlayerPrefs.GetString("Spell") == "")
        {
            PlayerPrefs.SetString("Spell", "Health");
            AddSpellToContainer("Health");
        }
        else if(PlayerPrefs.GetString("Spell") == "Health")
        {
            AddSpellToContainer("Health");
        }
        else if(PlayerPrefs.GetString("Spell") == "Mana")
        {
            AddSpellToContainer("Mana");
        }
        else if(PlayerPrefs.GetString("Spell") == "Dash")
        {
            AddSpellToContainer("Dash");
        }
        else if(PlayerPrefs.GetString("Spell") == "TripleJump")
        {
            AddSpellToContainer("TripleJump");
        }
        else if(PlayerPrefs.GetString("Spell") == "Resistance")
        {
            AddSpellToContainer("Resistance");
        }
        else if(PlayerPrefs.GetString("Spell") == "Shield")
        {
            AddSpellToContainer("Shield");
        }
        else if(PlayerPrefs.GetString("Spell") == "FinishingBlow")
        {
            AddSpellToContainer("FinishingBlow");
        }
        else if(PlayerPrefs.GetString("Spell") == "BloodThirsty")
        {
            AddSpellToContainer("BloodThirsty");
        }
        else if(PlayerPrefs.GetString("Spell") == "StandardDamage")
        {
            AddSpellToContainer("StandardDamage");
        }
        else {
            AddSpellToContainer("Health");
        }
    }

    void AddSpellToContainer(string _nameSpell)
    {
        GameObject spellPrefab = FindSpellPrefabByName(_nameSpell);
        if (spellPrefab != null)
        {
            RemoveSpellFromContainer();
            GameObject spellInstance = Instantiate(spellPrefab, _spellContainers.transform);
            PlayerPrefs.SetString("Spell", _nameSpell);
        }

        if(_nameSpell == "Health")
        {
            _textTitleSpell.text = "Hồi máu";
            _textDescriptionSpell.text = "Hồi " + _valueHealth +"% máu tối đa.";
            _textTimeCooldown.text = _timeCooldownHealth + " giây";
            PlayerPrefs.SetString("ValueTimeCooldownSpell", _timeCooldownHealth.ToString());
            PlayerPrefs.SetString("TitleSpell", _textTitleSpell.text);
            PlayerPrefs.SetString("DescriptionSpell", _textDescriptionSpell.text);
            PlayerPrefs.SetString("TimeCooldownSpell", _timeCooldownHealth + " giây");
            PlayerPrefs.SetString("ValueSpell" , _valueHealth.ToString());
            PlayerPrefs.SetString("TimeSpell" , _timeCooldownHealth.ToString());
        }
        else if(_nameSpell == "Mana")
        {
            _textTitleSpell.text = "Hồi năng lượng";
            _textDescriptionSpell.text = "Hồi " + _valueMana + "% năng lượng tối đa.";
            _textTimeCooldown.text = _timeCooldownMana + " giây";
            PlayerPrefs.SetString("ValueTimeCooldownSpell", _timeCooldownMana.ToString());
            PlayerPrefs.SetString("TitleSpell", _textTitleSpell.text);
            PlayerPrefs.SetString("DescriptionSpell", _textDescriptionSpell.text);
            PlayerPrefs.SetString("TimeCooldownSpell", _textTimeCooldown.text);
            PlayerPrefs.SetString("ValueSpell" , _valueMana.ToString());
            PlayerPrefs.SetString("TimeSpell" , _timeCooldownMana.ToString());
        }
        else if(_nameSpell == "Dash")
        {
            _textTitleSpell.text = "Tốc biến";
            _textDescriptionSpell.text = "Dịch chuyển lên phía trước";
            _textTimeCooldown.text = _timeCooldownDash + " giây";
            PlayerPrefs.SetString("ValueTimeCooldownSpell", _timeCooldownDash.ToString());
            PlayerPrefs.SetString("TitleSpell", _textTitleSpell.text);
            PlayerPrefs.SetString("DescriptionSpell", _textDescriptionSpell.text);
            PlayerPrefs.SetString("TimeCooldownSpell", _textTimeCooldown.text);
            PlayerPrefs.SetString("ValueSpell" , _timeCooldownDash.ToString());
            PlayerPrefs.SetString("TimeSpell" , _timeCooldownDash.ToString());
        }
        else if(_nameSpell == "TripleJump")
        {
            _textTitleSpell.text = "TripleJump";
            _textDescriptionSpell.text = "Nhảy 3 lần liên tiếp";
            _textTimeCooldown.text = _timeCooldownTripleJump + " giây";
            PlayerPrefs.SetString("ValueTimeCooldownSpell", _timeCooldownTripleJump.ToString());
            PlayerPrefs.SetString("TitleSpell", _textTitleSpell.text);
            PlayerPrefs.SetString("DescriptionSpell", _textDescriptionSpell.text);
            PlayerPrefs.SetString("TimeCooldownSpell", _textTimeCooldown.text);
            PlayerPrefs.SetString("ValueSpell" , _timeCooldownTripleJump.ToString());
            PlayerPrefs.SetString("TimeSpell" , _timeCooldownTripleJump.ToString());
        }
        else if(_nameSpell == "Resistance")
        {
            _textTitleSpell.text = "Kháng";
            _textDescriptionSpell.text = "Giảm " + _valueResistance + "% sát thương, miễn nhiễm bạo kích, bất động, né tránh trong " + _timeResistance + " giây.";
            _textTimeCooldown.text = _timeCooldownResistance + " giây";
            PlayerPrefs.SetString("ValueTimeCooldownSpell", _timeCooldownResistance.ToString());
            PlayerPrefs.SetString("TitleSpell", _textTitleSpell.text);
            PlayerPrefs.SetString("DescriptionSpell", _textDescriptionSpell.text);
            PlayerPrefs.SetString("TimeCooldownSpell", _textTimeCooldown.text);
            PlayerPrefs.SetString("ValueSpell" , _valueResistance.ToString());
            PlayerPrefs.SetString("TimeSpell" , _timeResistance.ToString());
            PlayerPrefs.SetString("ValueTimeCooldownSpell", _timeCooldownResistance.ToString());
        }
        else if(_nameSpell == "Shield")
        {
            _textTitleSpell.text = "Lá chắn";
            _textDescriptionSpell.text = "Tạo ra 1 lớp lá chắn, giảm " + _valueShield + "% sát thương trong " + _timeShield + " giây.";
            _textTimeCooldown.text = _timeCooldownShield + " giây";
            PlayerPrefs.SetString("ValueTimeCooldownSpell", _timeCooldownShield.ToString());
            PlayerPrefs.SetString("TitleSpell", _textTitleSpell.text);
            PlayerPrefs.SetString("DescriptionSpell", _textDescriptionSpell.text);
            PlayerPrefs.SetString("TimeCooldownSpell", _textTimeCooldown.text);
            PlayerPrefs.SetString("ValueSpell" , _valueShield.ToString());
            PlayerPrefs.SetString("TimeSpell" , _timeShield.ToString());
            PlayerPrefs.SetString("ValueTimeCooldownSpell", _timeCooldownShield.ToString());
        }
        else if(_nameSpell == "FinishingBlow")
        {
            _textTitleSpell.text = "Đòn kết liễu";
            _textDescriptionSpell.text = "Gây " + _valueFinishingBlow + "% sát thương, tăng " + _valueCritical + "% bạo kích trong " + _timeFinishingBlow + " giây.";
            _textTimeCooldown.text = _timeCooldownFinishingBlow + " giây";
            PlayerPrefs.SetString("ValueTimeCooldownSpell", _timeCooldownFinishingBlow.ToString());
            PlayerPrefs.SetString("TitleSpell", _textTitleSpell.text);
            PlayerPrefs.SetString("DescriptionSpell", _textDescriptionSpell.text);
            PlayerPrefs.SetString("TimeCooldownSpell", _textTimeCooldown.text);
            PlayerPrefs.SetString("ValueSpell" , _valueFinishingBlow.ToString());
            PlayerPrefs.SetString("TimeSpell" , _timeFinishingBlow.ToString());
            PlayerPrefs.SetString("ValueTimeCooldownSpell", _timeCooldownFinishingBlow.ToString());
        }
        else if(_nameSpell == "BloodThirsty")
        {
            _textTitleSpell.text = "Khát máu";
            _textDescriptionSpell.text = "Hút máu " + _valueBloodThirsty + "% sát thương gây ra trong " + _timeBloodThirsty + " giây.";
            _textTimeCooldown.text = _timeCooldownBloodThirsty + " giây";
            PlayerPrefs.SetString("ValueTimeCooldownSpell", _timeCooldownBloodThirsty.ToString());
            PlayerPrefs.SetString("TitleSpell", _textTitleSpell.text);
            PlayerPrefs.SetString("DescriptionSpell", _textDescriptionSpell.text);
            PlayerPrefs.SetString("TimeCooldownSpell", _textTimeCooldown.text);
            PlayerPrefs.SetString("ValueSpell" , _valueBloodThirsty.ToString());
            PlayerPrefs.SetString("TimeSpell" , _timeBloodThirsty.ToString());
            PlayerPrefs.SetString("ValueTimeCooldownSpell", _timeCooldownBloodThirsty.ToString());
        }
        else if(_nameSpell == "StandardDamage")
        {
            _textTitleSpell.text = "Sát thương chuẩn";
            _textDescriptionSpell.text = "Sát thương sẽ bỏ qua giáp và né tránh của kẻ địch trong " + _timeStandardDamage + " giây.";
            _textTimeCooldown.text = _timeCooldownStandardDamage + " giây";
            PlayerPrefs.SetString("ValueTimeCooldownSpell", _timeCooldownStandardDamage.ToString());
            PlayerPrefs.SetString("TitleSpell", _textTitleSpell.text);
            PlayerPrefs.SetString("DescriptionSpell", _textDescriptionSpell.text);
            PlayerPrefs.SetString("TimeCooldownSpell", _textTimeCooldown.text);
            PlayerPrefs.SetString("ValueSpell" , _timeStandardDamage.ToString());
            PlayerPrefs.SetString("TimeSpell" , _timeStandardDamage.ToString());
            PlayerPrefs.SetString("ValueTimeCooldownSpell", _timeCooldownStandardDamage.ToString());
        }
        else{
            _textTitleSpell.text = "";
            _textDescriptionSpell.text = "";
            _textTimeCooldown.text = "";
            PlayerPrefs.SetString("ValueTimeCooldownSpell", "");
            PlayerPrefs.SetString("TitleSpell", "");
            PlayerPrefs.SetString("DescriptionSpell", "");
            PlayerPrefs.SetString("TimeCooldownSpell", "");
            PlayerPrefs.SetString("ValueSpell" , "");
            PlayerPrefs.SetString("TimeSpell" , "");
            PlayerPrefs.SetString("ValueTimeCooldownSpell", "");
        }
    }

    void RemoveSpellFromContainer()
    {
        foreach (Transform child in _spellContainers.transform)
        {
            Destroy(child.gameObject);
        }
    }

    GameObject FindSpellPrefabByName(string _nameSpell)
    {
        foreach (GameObject spellPrefab in _listSpell)
        {
            if (spellPrefab.name.Equals(_nameSpell, System.StringComparison.OrdinalIgnoreCase))
            {
                return spellPrefab;
            }
        }
        return null;
    }
}
