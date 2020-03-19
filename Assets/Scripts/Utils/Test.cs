using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class Test : MonoBehaviour
{
    [TextArea]
    public string Value;

    private TMP_Text _text;

    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        _text.SetSpriteText(Value);
    }
}
