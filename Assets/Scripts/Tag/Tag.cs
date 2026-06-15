using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TagButton : MonoBehaviour
{
    public bool tagHomeIsOn = false;
    public bool tagUserIsOn = false;
    public bool tagSkillIsOn = false;
    public bool tagSpellIsOn = false;
    public bool tagGemIsOn = false;
    public bool tagSettingsIsOn = false;
    public bool tagInstructIsOn = false;

    public Button buttonHome;
    public Button buttonUser;
    public Button buttonSkill;
    public Button buttonSpell;
    public Button buttonGem;
    public Button buttonSettings;
    public Button buttonInstruct;

    public Image imageHome;
    public Image imageUser;
    public Image imageSkill;
    public Image imageSpell;
    public Image imageGem;
    public Image imageSettings;
    public Image imageInstruct;

    public Animator animatorTagHome;
    public Animator animatorTagUser;
    public Animator animatorTagSkill;
    public Animator animatorTagSpell;
    public Animator animatorTagGem;
    public Animator animatorTagSettings;
    public Animator animatorTagInstruct;
    public Animator animatorBook;

    private int numFlip = 0;
    private bool canChangeTag = false;

    public GameObject _champ;
    public GameObject _skill;
    public GameObject _uiHome;
    public GameObject _uiUser;
    public GameObject _uiSkill;
    public GameObject _uiSpell;
    public GameObject _uiGem;
    public GameObject _uiSettings;
    public GameObject _uiInstruct;
 
    void Start()
    {
        StartCoroutine(TagHomeStart());
        StartCoroutine(TagUserStart());
        StartCoroutine(TagSkillStart());
        StartCoroutine(TagSpellStart());
        StartCoroutine(TagGemStart());
        StartCoroutine(TagSettingsStart());
        StartCoroutine(TagInstructStart());

        buttonHome.onClick.AddListener(TagHome);
        buttonUser.onClick.AddListener(TagUser);
        buttonSkill.onClick.AddListener(TagSkill);
        buttonSpell.onClick.AddListener(TagSpell);
        buttonGem.onClick.AddListener(TagGem);
        buttonSettings.onClick.AddListener(TagSettings);
        buttonInstruct.onClick.AddListener(TagInstruct);
    }
    private IEnumerator TagHomeStart()
    {
        yield return new WaitForSeconds(2.1f);
        animatorTagHome.SetBool("isStart", false);
    }

    private IEnumerator TagUserStart()
    {
        yield return new WaitForSeconds(2.1f);
        animatorTagUser.SetBool("isStart", false);
    }

    private IEnumerator TagSkillStart()
    {
        yield return new WaitForSeconds(2.3f);
        animatorTagSkill.SetBool("isStart", false);
    }

    private IEnumerator TagSpellStart()
    {
        yield return new WaitForSeconds(2.5f);
        animatorTagSpell.SetBool("isStart", false);
    }

    private IEnumerator TagGemStart()
    {
        yield return new WaitForSeconds(2.7f);
        animatorTagGem.SetBool("isStart", false);
    }

    private IEnumerator TagSettingsStart()
    {
        yield return new WaitForSeconds(2.9f);
        animatorTagSettings.SetBool("isStart", false);
    }

    private IEnumerator TagInstructStart()
    {
        yield return new WaitForSeconds(3.1f);
        animatorTagInstruct.SetBool("isStart", false);
        tagHomeIsOn = true;
        animatorTagHome.SetBool("isChoose", tagHomeIsOn);
        StartCoroutine(DelayShowUI(_uiHome, true));
        StartCoroutine(DelayShowChamp());
        StartCoroutine(DelayShowSkill());
    }

    public void AnimatorFlip()
    {
        if(numFlip >= 3)
            numFlip = 0;

        numFlip++;
        animatorBook.SetInteger("numFlip", numFlip);
    }

    private IEnumerator DelayShowUI(GameObject UI, bool isOn)
    {
        yield return new WaitForSeconds(0.8f);
        UI.SetActive(isOn);
        canChangeTag = true;
    }

    private IEnumerator DelayShowChamp()
    {
        yield return new WaitForSeconds(0.8f);
        _champ.SetActive(true);
    }
    private IEnumerator DelayShowSkill()
    {
        yield return new WaitForSeconds(0.8f);
        _skill.SetActive(true);
    }

    private void HiddenAllUI()
    {
        _uiHome.SetActive(false);
        _uiUser.SetActive(false);
        _uiSkill.SetActive(false);
        _uiSpell.SetActive(false);
        _uiGem.SetActive(false);
        _uiSettings.SetActive(false);
        _uiInstruct.SetActive(false);
    }

    public void TagHome()
    {
        if(!tagHomeIsOn && canChangeTag)
        {
            _champ.SetActive(false);
            _skill.SetActive(false);
            canChangeTag = false;
            tagHomeIsOn = true;
            tagUserIsOn = false;
            tagSkillIsOn = false;
            tagSpellIsOn = false;
            tagGemIsOn = false;
            tagSettingsIsOn = false;
            tagInstructIsOn = false;
            animatorTagHome.SetBool("isChoose", tagHomeIsOn);
            animatorTagUser.SetBool("isChoose", tagUserIsOn);
            animatorTagSkill.SetBool("isChoose", tagSkillIsOn);
            animatorTagSpell.SetBool("isChoose", tagSpellIsOn);
            animatorTagGem.SetBool("isChoose", tagGemIsOn);
            animatorTagSettings.SetBool("isChoose", tagSettingsIsOn);
            animatorTagInstruct.SetBool("isChoose", tagInstructIsOn);
            HiddenAllUI();
            AnimatorFlip();
            StartCoroutine(DelayShowChamp());
            StartCoroutine(DelayShowSkill());
            StartCoroutine(DelayShowUI(_uiHome, true));
        }
    }

    public void TagUser()
    {
        if(!tagUserIsOn && canChangeTag)
        {
            _champ.SetActive(false);
            _skill.SetActive(false);
            canChangeTag = false;
            tagHomeIsOn = false;
            tagUserIsOn = true;
            tagSkillIsOn = false;
            tagSpellIsOn = false;
            tagGemIsOn = false;
            tagSettingsIsOn = false;
            tagInstructIsOn = false;
            animatorTagHome.SetBool("isChoose", tagHomeIsOn);
            animatorTagUser.SetBool("isChoose", tagUserIsOn);
            animatorTagSkill.SetBool("isChoose", tagSkillIsOn);
            animatorTagSpell.SetBool("isChoose", tagSpellIsOn);
            animatorTagGem.SetBool("isChoose", tagGemIsOn);
            animatorTagSettings.SetBool("isChoose", tagSettingsIsOn);
            animatorTagInstruct.SetBool("isChoose", tagInstructIsOn);
            HiddenAllUI();
            AnimatorFlip();
            StartCoroutine(DelayShowChamp());
            StartCoroutine(DelayShowSkill());
            StartCoroutine(DelayShowUI(_uiUser, true));
        }
    }

    public void TagSkill()
    {
        if(!tagSkillIsOn  && canChangeTag)
        {
            _champ.SetActive(false);
            _skill.SetActive(false);
            canChangeTag = false;
            tagHomeIsOn = false;
            tagUserIsOn = false;
            tagSkillIsOn = true;
            tagSpellIsOn = false;
            tagGemIsOn = false;
            tagSettingsIsOn = false;
            tagInstructIsOn = false;
            animatorTagHome.SetBool("isChoose", tagHomeIsOn);
            animatorTagUser.SetBool("isChoose", tagUserIsOn);
            animatorTagSkill.SetBool("isChoose", tagSkillIsOn);
            animatorTagSpell.SetBool("isChoose", tagSpellIsOn);
            animatorTagGem.SetBool("isChoose", tagGemIsOn);
            animatorTagSettings.SetBool("isChoose", tagSettingsIsOn);
            animatorTagInstruct.SetBool("isChoose", tagInstructIsOn);
            HiddenAllUI();
            AnimatorFlip();
            StartCoroutine(DelayShowChamp());
            StartCoroutine(DelayShowSkill());
            StartCoroutine(DelayShowUI(_uiSkill, true));
        }
    }

    public void TagSpell()
    {
        if(!tagSpellIsOn  && canChangeTag)
        {
            _champ.SetActive(false);
            _skill.SetActive(false);
            canChangeTag = false;
            tagHomeIsOn = false;
            tagUserIsOn = false;
            tagSkillIsOn = false;
            tagSpellIsOn = true;
            tagGemIsOn = false;
            tagSettingsIsOn = false;
            tagInstructIsOn = false;
            animatorTagHome.SetBool("isChoose", tagHomeIsOn);
            animatorTagUser.SetBool("isChoose", tagUserIsOn);
            animatorTagSkill.SetBool("isChoose", tagSkillIsOn);
            animatorTagSpell.SetBool("isChoose", tagSpellIsOn);
            animatorTagGem.SetBool("isChoose", tagGemIsOn);
            animatorTagSettings.SetBool("isChoose", tagSettingsIsOn);
            animatorTagInstruct.SetBool("isChoose", tagInstructIsOn);
            HiddenAllUI();
            AnimatorFlip();
            StartCoroutine(DelayShowChamp());
            StartCoroutine(DelayShowSkill());
            StartCoroutine(DelayShowUI(_uiSpell, true));
        }
    }

    public void TagGem()
    {
        if(!tagGemIsOn  && canChangeTag)
        {
            _champ.SetActive(false);
            _skill.SetActive(false);
            canChangeTag = false;
            tagHomeIsOn = false;
            tagUserIsOn = false;
            tagSkillIsOn = false;
            tagSpellIsOn = false;
            tagGemIsOn = true;
            tagSettingsIsOn = false;
            tagInstructIsOn = false;
            animatorTagHome.SetBool("isChoose", tagHomeIsOn);
            animatorTagUser.SetBool("isChoose", tagUserIsOn);
            animatorTagSkill.SetBool("isChoose", tagSkillIsOn);
            animatorTagSpell.SetBool("isChoose", tagSpellIsOn);
            animatorTagGem.SetBool("isChoose", tagGemIsOn);
            animatorTagSettings.SetBool("isChoose", tagSettingsIsOn);
            animatorTagInstruct.SetBool("isChoose", tagInstructIsOn);
            HiddenAllUI();
            AnimatorFlip();
            StartCoroutine(DelayShowUI(_uiGem, true));
        }
    }

    public void TagSettings()
    {
        if(!tagSettingsIsOn  && canChangeTag)
        {
            _champ.SetActive(false);
            _skill.SetActive(false);
            canChangeTag = false;
            tagHomeIsOn = false;
            tagUserIsOn = false;
            tagSkillIsOn = false;
            tagSpellIsOn = false;
            tagGemIsOn = false;
            tagSettingsIsOn = true;
            tagInstructIsOn = false;
            animatorTagHome.SetBool("isChoose", tagHomeIsOn);
            animatorTagUser.SetBool("isChoose", tagUserIsOn);
            animatorTagSkill.SetBool("isChoose", tagSkillIsOn);
            animatorTagSpell.SetBool("isChoose", tagSpellIsOn);
            animatorTagGem.SetBool("isChoose", tagGemIsOn);
            animatorTagSettings.SetBool("isChoose", tagSettingsIsOn);
            animatorTagInstruct.SetBool("isChoose", tagInstructIsOn);
            HiddenAllUI();
            AnimatorFlip();
            StartCoroutine(DelayShowUI(_uiSettings, true));
        }
    }

    public void TagInstruct()
    {
        if(!tagInstructIsOn && canChangeTag)
        {
            _champ.SetActive(false);
            _skill.SetActive(false);
            canChangeTag = false;
            tagHomeIsOn = false;
            tagUserIsOn = false;
            tagSkillIsOn = false;
            tagSpellIsOn = false;
            tagGemIsOn = false;
            tagSettingsIsOn = false;
            tagInstructIsOn = true;
            animatorTagHome.SetBool("isChoose", tagHomeIsOn);
            animatorTagUser.SetBool("isChoose", tagUserIsOn);
            animatorTagSkill.SetBool("isChoose", tagSkillIsOn);
            animatorTagSpell.SetBool("isChoose", tagSpellIsOn);
            animatorTagGem.SetBool("isChoose", tagGemIsOn);
            animatorTagSettings.SetBool("isChoose", tagSettingsIsOn);
            animatorTagInstruct.SetBool("isChoose", tagInstructIsOn);
            HiddenAllUI();
            AnimatorFlip();
            StartCoroutine(DelayShowUI(_uiInstruct, true));
        }
    }

}
