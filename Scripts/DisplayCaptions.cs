using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayCaptions : MonoBehaviour {

    private TextMeshProUGUI text;
    public List<CaptionItem> captionItems = new List<CaptionItem>();
    public float CaptionLifetime = 5f;
    public BoolVariable showCaptions;

    public void AddCaption(Color _color, float _volume, string _caption)
    {
        foreach (CaptionItem item in captionItems)
            item.lifetime -= item.lifetime / CaptionLifetime;
        captionItems.Add(new CaptionItem() { color = _color, volume = _volume, caption = _caption, lifetime = CaptionLifetime});
    }

	// Use this for initialization
	void Start () {
        text = GetComponent<TextMeshProUGUI>();
	}
	
	// Update is called once per frame
	void Update () {
        string txt = "";
        List<CaptionItem> removeList = new List<CaptionItem>();
        int i = 0;
        foreach(CaptionItem item in captionItems)
        {
            Color color = new Color(item.color.r,item.color.g,item.color.b,Mathf.Lerp(0.5f,1f,item.volume));
            if (i++ > 0)
                txt += "\n";
            txt += "<color=#" + ColorUtility.ToHtmlStringRGBA(color) + ">" + item.caption + "</color>";
            item.lifetime -= Time.deltaTime;
            if (item.lifetime < 0)
                removeList.Add(item);
        }
        foreach (CaptionItem r_item in removeList)
            if (captionItems.Contains(r_item))
                captionItems.Remove(r_item);
        //
        if (showCaptions != null && !showCaptions.RuntimeValue)
        {
            text.text = "";
        }
        else
            text.text = txt;
	}
    [System.Serializable]
    public class CaptionItem
    {
        public Color color;
        public float volume;
        public string caption;
        public float lifetime;
    }
}
