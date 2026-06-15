using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeKeyBoard : MonoBehaviour
{
    public Button _changeLeftButton;
    public Button _changeRightButton;
    public Button _changeJumpButton;
    public Button _changeFallButton;
    public Button _changeSkill01Button;
    public Button _changeSkill02Button;
    public Button _changeSkill03Button;
    public Button _changeSkill04Button;
    public Button _changeSpellButton;
    public Button _changeIntrinsicButton;
    public Button _changeAttackButton;

    private KeyCode _tempLeft;
    private KeyCode _tempRight;
    private KeyCode _tempJump;
    private KeyCode _tempFall;
    private KeyCode _tempSkill01;
    private KeyCode _tempSkill02;
    private KeyCode _tempSkill03;
    private KeyCode _tempSkill04;
    private KeyCode _tempSpell;
    private KeyCode _tempIntrinsic;
    private KeyCode _tempAttack;

    public GameObject _keyLeftContainer;
    public GameObject _keyRightContainer;
    public GameObject _keyJumpContainer;
    public GameObject _keyFallContainer;
    public GameObject _keySkill01Container;
    public GameObject _keySkill02Container;
    public GameObject _keySkill03Container;
    public GameObject _keySkill04Container;
    public GameObject _keySpellContainer;
    public GameObject _keyIntrinsicContainer;
    public GameObject _keyAttackContainer;

    public GameObject _panelChangeKeyBoard;
    public Button _cancelChangeButton;

    public GameObject[] _keyPrefab;

    private KeyCode[] validKeys = { KeyCode.Backspace, KeyCode.Delete, KeyCode.Tab, KeyCode.Return, KeyCode.Pause, KeyCode.Escape, KeyCode.Space,
    KeyCode.Keypad0, KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3, KeyCode.Keypad4, KeyCode.Keypad5, KeyCode.Keypad6, KeyCode.Keypad7, KeyCode.Keypad8, KeyCode.Keypad9,
    KeyCode.KeypadPeriod, KeyCode.KeypadDivide, KeyCode.KeypadMultiply, KeyCode.KeypadMinus, KeyCode.KeypadPlus, KeyCode.KeypadEnter, KeyCode.KeypadEquals,
    KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.RightArrow, KeyCode.LeftArrow,
    KeyCode.Insert, KeyCode.Home, KeyCode.End, KeyCode.PageUp, KeyCode.PageDown,
    KeyCode.F1, KeyCode.F2, KeyCode.F3, KeyCode.F4, KeyCode.F5, KeyCode.F6, KeyCode.F7, KeyCode.F8, KeyCode.F9, KeyCode.F10, KeyCode.F11, KeyCode.F12,
    KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9,
    KeyCode.Quote, KeyCode.Comma, KeyCode.Minus, KeyCode.Period, KeyCode.Slash, KeyCode.Semicolon, KeyCode.Equals, KeyCode.LeftBracket, KeyCode.Backslash, KeyCode.RightBracket, KeyCode.BackQuote,
    KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y, KeyCode.Z,
    KeyCode.Numlock, KeyCode.CapsLock, KeyCode.ScrollLock, KeyCode.RightShift, KeyCode.LeftShift, KeyCode.RightControl, KeyCode.LeftControl, KeyCode.RightAlt, KeyCode.LeftAlt, KeyCode.LeftWindows, KeyCode.RightWindows
    };

    void Start()
    {
        _changeLeftButton.onClick.AddListener(ChangeLeft);
        _changeRightButton.onClick.AddListener(ChangeRight);
        _changeJumpButton.onClick.AddListener(ChangeJump);
        _changeFallButton.onClick.AddListener(ChangeFall);
        _changeSkill01Button.onClick.AddListener(ChangeSkill01);
        _changeSkill02Button.onClick.AddListener(ChangeSkill02);
        _changeSkill03Button.onClick.AddListener(ChangeSkill03);
        _changeSkill04Button.onClick.AddListener(ChangeSkill04);
        _changeSpellButton.onClick.AddListener(ChangeSpell);
        _changeIntrinsicButton.onClick.AddListener(ChangeIntrinsic);
        _changeAttackButton.onClick.AddListener(ChangeAttack);

        _cancelChangeButton.onClick.AddListener(CancelChange);

        _tempLeft = (KeyCode)PlayerPrefs.GetInt("_left");
        _tempRight = (KeyCode)PlayerPrefs.GetInt("_right");
        _tempJump = (KeyCode)PlayerPrefs.GetInt("_jump");
        _tempFall = (KeyCode)PlayerPrefs.GetInt("_fall");
        _tempSkill01 = (KeyCode)PlayerPrefs.GetInt("_skill01");
        _tempSkill02 = (KeyCode)PlayerPrefs.GetInt("_skill02");
        _tempSkill03 = (KeyCode)PlayerPrefs.GetInt("_skill03");
        _tempSkill04 = (KeyCode)PlayerPrefs.GetInt("_skill04");
        _tempSpell = (KeyCode)PlayerPrefs.GetInt("_spell");
        _tempIntrinsic = (KeyCode)PlayerPrefs.GetInt("_intrinsic");
        _tempAttack = (KeyCode)PlayerPrefs.GetInt("_attack");
        SetKey();
        CancelChange();
    }

    private void SetKey()
    {
        if(_tempLeft == KeyCode.None)
        {
            _tempLeft = KeyCode.LeftArrow;
        }

        if (_tempRight == KeyCode.None)
        {
            _tempRight = KeyCode.RightArrow;
        }

        if (_tempJump == KeyCode.None)
        {
            _tempJump = KeyCode.UpArrow;
        }

        if (_tempFall == KeyCode.None)
        {
            _tempFall = KeyCode.DownArrow;
        }

        if (_tempSkill01 == KeyCode.None)
        {
            _tempSkill01 = KeyCode.Q;
        }

        if (_tempSkill02 == KeyCode.None)
        {
            _tempSkill02 = KeyCode.W;
        }

        if (_tempSkill03 == KeyCode.None)
        {
            _tempSkill03 = KeyCode.E;
        }

        if (_tempSkill04 == KeyCode.None)
        {
            _tempSkill04 = KeyCode.R;
        }

        if (_tempSpell == KeyCode.None)
        {
            _tempSpell = KeyCode.F;
        }

        if (_tempIntrinsic == KeyCode.None)
        {
            _tempIntrinsic = KeyCode.RightControl;
        }

        if (_tempAttack == KeyCode.None)
        {
            _tempAttack = KeyCode.Space;
        }

    }

    private void ChangeLeft()
    {
        _panelChangeKeyBoard.SetActive(true);
        StartCoroutine(ChangeKeyCode("_left", _keyLeftContainer));
    }

    private void ChangeRight()
    {
        _panelChangeKeyBoard.SetActive(true);
        StartCoroutine(ChangeKeyCode("_right", _keyRightContainer));
    }

    private void ChangeJump()
    {
        _panelChangeKeyBoard.SetActive(true);
        StartCoroutine(ChangeKeyCode("_jump", _keyJumpContainer));
    }

    private void ChangeFall()
    {
        _panelChangeKeyBoard.SetActive(true);
        StartCoroutine(ChangeKeyCode("_fall", _keyFallContainer));
    }

    private void ChangeSkill01()
    {
        _panelChangeKeyBoard.SetActive(true);
        StartCoroutine(ChangeKeyCode("_skill01", _keySkill01Container));
    }

    private void ChangeSkill02()
    {
        _panelChangeKeyBoard.SetActive(true);
        StartCoroutine(ChangeKeyCode("_skill02", _keySkill02Container));
    }

    private void ChangeSkill03()
    {
        _panelChangeKeyBoard.SetActive(true);
        StartCoroutine(ChangeKeyCode("_skill03", _keySkill03Container));
    }

    private void ChangeSkill04()
    {
        _panelChangeKeyBoard.SetActive(true);
        StartCoroutine(ChangeKeyCode("_skill04", _keySkill04Container));
    }

    private void ChangeSpell()
    {
        _panelChangeKeyBoard.SetActive(true);
        StartCoroutine(ChangeKeyCode("_spell", _keySpellContainer));
    }

    private void ChangeIntrinsic()
    {
        _panelChangeKeyBoard.SetActive(true);
        StartCoroutine(ChangeKeyCode("_intrinsic", _keyIntrinsicContainer));
    }

    private void ChangeAttack()
    {
        _panelChangeKeyBoard.SetActive(true);
        StartCoroutine(ChangeKeyCode("_attack", _keyAttackContainer));
    }

    private void CancelChange()
    {
        PlayerPrefs.SetInt("_left", (int)_tempLeft);
        PlayerPrefs.SetInt("_right", (int)_tempRight);
        PlayerPrefs.SetInt("_jump", (int)_tempJump);
        PlayerPrefs.SetInt("_fall", (int)_tempFall);
        PlayerPrefs.SetInt("_skill01", (int)_tempSkill01);
        PlayerPrefs.SetInt("_skill02", (int)_tempSkill02);
        PlayerPrefs.SetInt("_skill03", (int)_tempSkill03);
        PlayerPrefs.SetInt("_skill04", (int)_tempSkill04);
        PlayerPrefs.SetInt("_spell", (int)_tempSpell);
        PlayerPrefs.SetInt("_intrinsic", (int)_tempIntrinsic);
        PlayerPrefs.SetInt("_attack", (int)_tempAttack);
        PlayerPrefs.Save();
        CreateKeyPrefab(_tempLeft.ToString(), _keyLeftContainer);
        CreateKeyPrefab(_tempRight.ToString(), _keyRightContainer);
        CreateKeyPrefab(_tempJump.ToString(), _keyJumpContainer);
        CreateKeyPrefab(_tempFall.ToString(), _keyFallContainer);
        CreateKeyPrefab(_tempSkill01.ToString(), _keySkill01Container);
        CreateKeyPrefab(_tempSkill02.ToString(), _keySkill02Container);
        CreateKeyPrefab(_tempSkill03.ToString(), _keySkill03Container);
        CreateKeyPrefab(_tempSkill04.ToString(), _keySkill04Container);
        CreateKeyPrefab(_tempSpell.ToString(), _keySpellContainer);
        CreateKeyPrefab(_tempIntrinsic.ToString(), _keyIntrinsicContainer);
        CreateKeyPrefab(_tempAttack.ToString(), _keyAttackContainer);
        _panelChangeKeyBoard.SetActive(false);
    }

    private IEnumerator ChangeKeyCode(string keyName, GameObject _container)
    {
        yield return new WaitUntil(() => Input.anyKeyDown);

        foreach (KeyCode keyCode in validKeys)
        {
            if (Input.GetKeyDown(keyCode))
            {
                PlayerPrefs.SetInt(keyName, (int)keyCode);
                PlayerPrefs.Save();
                CreateKeyPrefab(keyCode.ToString(), _container);
                _tempLeft = (KeyCode)PlayerPrefs.GetInt("_left");
                _tempRight = (KeyCode)PlayerPrefs.GetInt("_right");
                _tempJump = (KeyCode)PlayerPrefs.GetInt("_jump");
                _tempFall = (KeyCode)PlayerPrefs.GetInt("_fall");
                _tempSkill01 = (KeyCode)PlayerPrefs.GetInt("_skill01");
                _tempSkill02 = (KeyCode)PlayerPrefs.GetInt("_skill02");
                _tempSkill03 = (KeyCode)PlayerPrefs.GetInt("_skill03");
                _tempSkill04 = (KeyCode)PlayerPrefs.GetInt("_skill04");
                _tempSpell = (KeyCode)PlayerPrefs.GetInt("_spell");
                _tempIntrinsic = (KeyCode)PlayerPrefs.GetInt("_intrinsic");
                _tempAttack = (KeyCode)PlayerPrefs.GetInt("_attack");
                _panelChangeKeyBoard.SetActive(false);
                break;
            }
        }
    }

    private void CreateKeyPrefab(string keyCode, GameObject _container)
    {
       foreach (Transform child in _container.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (GameObject child in _keyPrefab)
        {
            if(child.name == keyCode.ToString())
            {
                Instantiate(child, _container.transform);
            }
        }
    }
}
