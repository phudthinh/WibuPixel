using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class NarutoProperties : MonoBehaviour
{
    private float _damage = 90.0f;
    private float _defense = 0f;
    private float _health = 2700.0f;
    private float _healing = 1.0f;
    private float _mana = 2000.0f;
    private float _restoreMana = 1.0f;
    private float _critical = 10;
    private float _criticalDamage = 100;
    private float _dodge = 0;
    private float _bloodsucking = 0;
    private float _reducedHealing = 0;
    private float _speed = 3.0f;
    private float _resistance = 0;
    private float _penetrate = 0;
    private float _attackSpeed = 0.6f;
    private float _reducedTime = 0;
    private float _stun = 0;

    private float _cooldownIntrinsic = 10; // 10
    private float _cooldownSkill01 = 3; // 3
    private float _cooldownSkill02 = 12; // 12
    private float _cooldownSkill03 = 25; // 25
    private float _cooldownSkill04 = 30; // 30

    private float _manaIntrinsic = 10;
    private float _manaSkill01 = 20;
    private float _manaSkill02 = 150;
    private float _manaSkill03 = 250;
    private float _manaSkill04 = 300;

    private float _damIntrinsic = 0;
    private float _damSkill01 = 20;
    private float _damSkill02 = 0;
    private float _damSkill03 = 350;
    private float _damSkill04 = 500;

    private string _titleIntrinsic;
    private string _titleSkill01;
    private string _titleSkill02;
    private string _titleSkill03;
    private string _titleSkill04;

    private string _descriptionIntrinsic;
    private string _descriptionSkill01;
    private string _descriptionSkill02;
    private string _descriptionSkill03;
    private string _descriptionSkill04;
    
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

    public TextMeshProUGUI _titleIntrinsicText;
    public TextMeshProUGUI _titleSkill01Text;
    public TextMeshProUGUI _titleSkill02Text;
    public TextMeshProUGUI _titleSkill03Text;
    public TextMeshProUGUI _titleSkill04Text;

    public TextMeshProUGUI _descriptionIntrinsicText;
    public TextMeshProUGUI _descriptionSkill01Text;
    public TextMeshProUGUI _descriptionSkill02Text;
    public TextMeshProUGUI _descriptionSkill03Text;
    public TextMeshProUGUI _descriptionSkill04Text;

    public TextMeshProUGUI _cooldownIntrinsicText;
    public TextMeshProUGUI _cooldownSkill01Text;
    public TextMeshProUGUI _cooldownSkill02Text;
    public TextMeshProUGUI _cooldownSkill03Text;
    public TextMeshProUGUI _cooldownSkill04Text;

    public TextMeshProUGUI _manaIntrinsicText;
    public TextMeshProUGUI _manaSkill01Text;
    public TextMeshProUGUI _manaSkill02Text;
    public TextMeshProUGUI _manaSkill03Text;
    public TextMeshProUGUI _manaSkill04Text;

    public Image _imageIntrinsic;
    public Image _imageSkill01;
    public Image _imageSkill02;
    public Image _imageSkill03;
    public Image _imageSkill04;

    public Image _imageIntrinsicFrame;
    public Image _imageSkill01Frame;
    public Image _imageSkill02Frame;
    public Image _imageSkill03Frame;
    public Image _imageSkill04Frame;

    public GameObject _UIUser;

    void Start()
    {
        if(SceneManager.GetActiveScene().name == "Menu")
        {
            _titleIntrinsic = "Chakra";
            _titleSkill01 = "Kunai";
            _titleSkill02 = "Ảnh phân thân chỉ thuật";
            _titleSkill03 = "Rasengan";
            _titleSkill04 = "Rasenshuriken";

            _descriptionIntrinsic = "Naruto tăng 2 sát thương mỗi " + _cooldownIntrinsic + " giây.";
            _descriptionSkill01 = "Naruto ném 1 kunai, gây " + _damSkill01 + " sát thương. Khi ở trên không sẽ ném 3 kunai.";
            _descriptionSkill02 = "Naruto tạo ra 2 ảnh phân thân đánh lừa đối thủ. Mỗi ảnh phân ra chạy 1 hướng ngẫu nhiên và phân thân tồn tại trong 10 giây.";
            _descriptionSkill03 = "Naruto tạo ra 1 quả Rasengan, gây " + _damSkill03 + " sát thương.";
            _descriptionSkill04 = "Naruto tạo ra 1 quả Rasenshuriken, gây " + _damSkill04 + " sát thương.";

            _imageIntrinsicFrame = GameObject.Find("IntrinsicFrame").GetComponent<Image>();
            _imageSkill01Frame = GameObject.Find("Skill01Frame").GetComponent<Image>();
            _imageSkill02Frame = GameObject.Find("Skill02Frame").GetComponent<Image>();
            _imageSkill03Frame = GameObject.Find("Skill03Frame").GetComponent<Image>();
            _imageSkill04Frame = GameObject.Find("Skill04Frame").GetComponent<Image>();
        }
    }
     void FixedUpdate()
    {
        if(SceneManager.GetActiveScene().name == "Menu")
        {
            PlayerPrefs.SetFloat("Damage", _damage + PlayerPrefs.GetFloat("PlusGemDamage"));
            PlayerPrefs.SetFloat("Defense", _defense + PlayerPrefs.GetFloat("PlusGemDefense"));
            PlayerPrefs.SetFloat("Health", _health + PlayerPrefs.GetFloat("PlusGemHealth"));
            PlayerPrefs.SetFloat("Healing", _healing + PlayerPrefs.GetFloat("PlusGemHealing"));
            PlayerPrefs.SetFloat("Mana", _mana + PlayerPrefs.GetFloat("PlusGemMana"));
            PlayerPrefs.SetFloat("RestoreMana", _restoreMana + PlayerPrefs.GetFloat("PlusGemRestoreMana"));
            PlayerPrefs.SetFloat("Critical", _critical + PlayerPrefs.GetFloat("PlusGemCritical"));
            PlayerPrefs.SetFloat("CriticalDamage", _criticalDamage + PlayerPrefs.GetFloat("PlusGemCriticalDamage"));
            PlayerPrefs.SetFloat("Dodge", _dodge + PlayerPrefs.GetFloat("PlusGemDodge"));
            PlayerPrefs.SetFloat("Bloodsucking", _bloodsucking + PlayerPrefs.GetFloat("PlusGemBloodsucking"));
            PlayerPrefs.SetFloat("ReducedHealing", _reducedHealing + PlayerPrefs.GetFloat("PlusGemReducedHealing"));
            PlayerPrefs.SetFloat("Speed", _speed + PlayerPrefs.GetFloat("PlusGemSpeed"));
            PlayerPrefs.SetFloat("Resistance", _resistance + PlayerPrefs.GetFloat("PlusGemResistance"));
            PlayerPrefs.SetFloat("Penetrate", _penetrate + PlayerPrefs.GetFloat("PlusGemPenetrate"));
            PlayerPrefs.SetFloat("AttackSpeed", _attackSpeed + PlayerPrefs.GetFloat("PlusGemAttackSpeed"));
            PlayerPrefs.SetFloat("ReducedTime", _reducedTime + PlayerPrefs.GetFloat("PlusGemReducedTime"));
            PlayerPrefs.SetFloat("Stun", _stun + PlayerPrefs.GetFloat("PlusGemStun"));

            _damageText.text = "Công: " + (PlayerPrefs.GetFloat("Damage")).ToString();
            _defenseText.text = "Thủ: " + (PlayerPrefs.GetFloat("Defense")).ToString();
            _healthText.text = "Máu: " + (PlayerPrefs.GetFloat("Health")).ToString();
            _healingText.text = "Hồi máu: " + (PlayerPrefs.GetFloat("Healing")).ToString() + "/s";
            _manaText.text = "Mana: " + (PlayerPrefs.GetFloat("Mana")).ToString();
            _restoreManaText.text = "Hồi mana: " + (PlayerPrefs.GetFloat("RestoreMana")).ToString() + "/s";
            _criticalText.text = "Bạo kích: " + (PlayerPrefs.GetFloat("Critical")).ToString() + "%";
            _criticalDamageText.text = "Bạo thương: " + (PlayerPrefs.GetFloat("CriticalDamage")).ToString() + "%";
            _dodgeText.text = "Né: " + (PlayerPrefs.GetFloat("Dodge")).ToString() + "%";
            _bloodsuckingText.text = "Hút máu: " + (PlayerPrefs.GetFloat("Bloodsucking")).ToString() + "%";
            _reducedHealingText.text = "Giảm hồi máu: " + (PlayerPrefs.GetFloat("ReducedHealing")).ToString() + "%";
            _speedText.text = "Tốc độ di chuyển: " + (PlayerPrefs.GetFloat("Speed")).ToString();
            _resistanceText.text = "Kháng hiệu ứng: " + (PlayerPrefs.GetFloat("Resistance")).ToString();
            _penetrateText.text = "Xuyên giáp: " + (PlayerPrefs.GetFloat("Penetrate")).ToString();
            _attackSpeedText.text = "Tốc độ đánh: " + (PlayerPrefs.GetFloat("AttackSpeed")).ToString() + "%";
            _reducedTimeText.text = "Giảm hồi chiêu: " + (PlayerPrefs.GetFloat("ReducedTime")).ToString() + "%";
            _stunText.text = "Bất động: " + (PlayerPrefs.GetFloat("Stun")).ToString() + "%";

            SetIntrinsic();
            SetSkill01();
            SetSkill02();
            SetSkill03();
            SetSkill04();
            SetInfoSkill();
        }
    }

    public void SetInfoSkill()
    {
        PlayerPrefs.SetString("TitleIntrinsic", _titleIntrinsic);
        PlayerPrefs.SetString("DescriptionIntrinsic", _descriptionIntrinsic);
        PlayerPrefs.SetFloat("ManaIntrinsic", _manaIntrinsic);
        PlayerPrefs.SetFloat("CooldownIntrinsic", _cooldownIntrinsic);

        PlayerPrefs.SetString("TitleSkill01", _titleSkill01);
        PlayerPrefs.SetString("DescriptionSkill01", _descriptionSkill01);
        PlayerPrefs.SetFloat("ManaSkill01", _manaSkill01);
        PlayerPrefs.SetFloat("CooldownSkill01", _cooldownSkill01);

        PlayerPrefs.SetString("TitleSkill02", _titleSkill02);
        PlayerPrefs.SetString("DescriptionSkill02", _descriptionSkill02);
        PlayerPrefs.SetFloat("ManaSkill02", _manaSkill02);
        PlayerPrefs.SetFloat("CooldownSkill02", _cooldownSkill02);

        PlayerPrefs.SetString("TitleSkill03", _titleSkill03);
        PlayerPrefs.SetString("DescriptionSkill03", _descriptionSkill03);
        PlayerPrefs.SetFloat("ManaSkill03", _manaSkill03);
        PlayerPrefs.SetFloat("CooldownSkill03", _cooldownSkill03);

        PlayerPrefs.SetString("TitleSkill04", _titleSkill04);
        PlayerPrefs.SetString("DescriptionSkill04", _descriptionSkill04);
        PlayerPrefs.SetFloat("ManaSkill04", _manaSkill04);
        PlayerPrefs.SetFloat("CooldownSkill04", _cooldownSkill04);

        PlayerPrefs.SetFloat("DamIntrinsic", _damIntrinsic);
        PlayerPrefs.SetFloat("DamSkill01", _damSkill01);
        PlayerPrefs.SetFloat("DamSkill02", _damSkill02);
        PlayerPrefs.SetFloat("DamSkill03", _damSkill03);
        PlayerPrefs.SetFloat("DamSkill04", _damSkill04);

        _imageIntrinsicFrame.sprite = Resources.Load<Sprite>("SkillNaruto/Intrinsic");
        _imageSkill01Frame.sprite = Resources.Load<Sprite>("SkillNaruto/Skill01");
        _imageSkill02Frame.sprite = Resources.Load<Sprite>("SkillNaruto/Skill02");
        _imageSkill03Frame.sprite = Resources.Load<Sprite>("SkillNaruto/Skill03");
        _imageSkill04Frame.sprite = Resources.Load<Sprite>("SkillNaruto/Skill04");
    }
    public void SetIntrinsic()
    {
        _titleIntrinsicText.text = _titleIntrinsic;
        _descriptionIntrinsicText.text = _descriptionIntrinsic;
        _manaIntrinsicText.text = "Mana: " + _manaIntrinsic.ToString();
        _cooldownSkill01Text.text = "Hồi: " + _cooldownIntrinsic.ToString() + " giây";
        _imageIntrinsic.sprite = Resources.Load<Sprite>("SkillNaruto/Intrinsic");
    }

    public void SetSkill01()
    {
        _titleSkill01Text.text = _titleSkill01;
        _descriptionSkill01Text.text = _descriptionSkill01;
        _cooldownSkill01Text.text = "Hồi: " + _cooldownSkill01.ToString() + " giây";
        _manaSkill01Text.text = "Mana: " + _manaSkill01.ToString();
        _imageSkill01.sprite = Resources.Load<Sprite>("SkillNaruto/Skill01");
    }

    public void SetSkill02()
    {
        _titleSkill02Text.text = _titleSkill02;
        _descriptionSkill02Text.text = _descriptionSkill02;
        _cooldownSkill02Text.text = "Hồi: " + _cooldownSkill02.ToString() + " giây";
        _manaSkill02Text.text = "Mana: " + _manaSkill02.ToString();
        _imageSkill02.sprite = Resources.Load<Sprite>("SkillNaruto/Skill02");
    }

    public void SetSkill03()
    {
        _titleSkill03Text.text = _titleSkill03;
        _descriptionSkill03Text.text = _descriptionSkill03;
        _cooldownSkill03Text.text = "Hồi: " + _cooldownSkill03.ToString() + " giây";
        _manaSkill03Text.text = "Mana: " + _manaSkill03.ToString();
        _imageSkill03.sprite = Resources.Load<Sprite>("SkillNaruto/Skill03");
    }

    public void SetSkill04()
    {
        _titleSkill04Text.text = _titleSkill04;
        _descriptionSkill04Text.text = _descriptionSkill04;
        _cooldownSkill04Text.text = "Hồi: " + _cooldownSkill04.ToString() + " giây";
        _manaSkill04Text.text = "Mana: " + _manaSkill04.ToString();
        _imageSkill04.sprite = Resources.Load<Sprite>("SkillNaruto/Skill04");
    }
}
