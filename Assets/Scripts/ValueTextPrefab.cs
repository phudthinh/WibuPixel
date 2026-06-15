using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Text;
using System.Linq;

public class ValueText : MonoBehaviourPunCallbacks
{
    private PhotonView _photonView;
    private string _type;
    private float _value;

    private float timeDestroy = 2f, valueBlur = 1f, valueScale = 1f;

    private TextMeshPro _textMeshPro;
    private TextMeshPro TextMeshProComponent
    {
        get
        {
            if (_textMeshPro == null)
            {
                _textMeshPro = GetComponent<TextMeshPro>();
            }
            return _textMeshPro;
        }
    }

    [PunRPC]
    public void SetValueTextInfoRPC(string type, float value)
    {
        _type = type;
        _value = value;
        UpdateText();
    }

    private void UpdateText()
    {
        TextMeshProComponent.text = GetFormattedAttributeText(_value);
        ChangeColor();
    }

    private string GetFormattedAttributeText(float attributeValue)
    {
        if (attributeValue == 0)
        {
            return "0";
        }
        else if (attributeValue < 1 && attributeValue > 0)
        {
            return "0" + attributeValue.ToString("###,###.#");
        }
        else
        {
            return attributeValue.ToString("###,###");
        }
    }

    void Start()
    {
        _photonView = photonView;
    }

    void Update()
    {
        Destroy();
        Blur();
    }


    public void Destroy()
    {
        timeDestroy -= Time.deltaTime;
        if (timeDestroy <= 0)
        {
            _photonView.RPC("DestroyThisRPC", RpcTarget.All);
        }
    }

    public void Blur()
    {
        if (timeDestroy <= 1.8)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 0.01f);
            valueScale -= 0.005f;
            valueBlur -= 0.005f;
            transform.localScale = new Vector2(valueScale, valueScale);
        }
        if (valueScale <= 0)
        {
            valueScale = 0;
        }
        if (valueBlur <= 0)
        {
            valueBlur = 0;
        }
    }

    void ChangeColor()
    {
        var tmp = TextMeshProComponent;
        if(_type == "Damage")
        {
            tmp.color = new Color32(255, 100, 0, 255);
            tmp.fontSize = 3.0f;
        }
        else if(_type == "CriticalDamage")
        {
            tmp.color = new Color32(255, 0, 0, 255);
            tmp.fontSize = 4.0f;
            tmp.fontStyle = FontStyles.Bold;
        }
        else if(_type == "Healing")
        {
            tmp.color = new Color32(114, 187, 78, 255);
            tmp.fontSize = 5.0f;
            tmp.fontStyle = FontStyles.Bold;
        }
        else if(_type == "MiniHealing")
        {
            tmp.color = new Color32(114, 187, 78, 255);
            tmp.fontSize = 3.0f;
            tmp.fontStyle = FontStyles.Bold;
        }
        else if(_type == "Mana")
        {
            tmp.color = new Color32(33, 157, 242, 255);
            tmp.fontSize = 5.0f;
            tmp.fontStyle = FontStyles.Bold;
        }
        else if(_type == "MiniMana")
        {
            tmp.color = new Color32(33, 157, 242, 255);
            tmp.fontSize = 3.0f;
            tmp.fontStyle = FontStyles.Bold;
        }
        else if(_type == "Bloodsucking")
        {
            tmp.color = new Color32(255, 0, 0, 255);
            tmp.fontSize = 3.0f;
            tmp.fontStyle = FontStyles.Bold;
        }
    }

    [PunRPC]
    public void DestroyThisRPC()
    {
        Destroy(this.gameObject);
    }
}
