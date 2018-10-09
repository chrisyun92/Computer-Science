using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TextMeshProUGUI))]
//Code from https://deltadreamgames.com/unity-tmp-hyperlinks
public class HyperlinkInText : MonoBehaviour, IPointerClickHandler
{
    TextMeshProUGUI textMeshPro;
    Canvas canvas;
    Camera m_camera;

    void Awake()
    {
        textMeshPro = gameObject.GetComponent<TextMeshProUGUI>();
        canvas = gameObject.GetComponentInParent<Canvas>();
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            m_camera = null;
        }
        else
        {
            m_camera = canvas.worldCamera;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(textMeshPro, Input.GetTouch(0).position, m_camera);
        if (linkIndex != -1)
        {
            Debug.Log("inside");
            TMP_LinkInfo linkInfo = textMeshPro.textInfo.linkInfo[linkIndex];
            Debug.Log(linkInfo.GetLinkID());
            Application.OpenURL(linkInfo.GetLinkID());
        }
    }
}