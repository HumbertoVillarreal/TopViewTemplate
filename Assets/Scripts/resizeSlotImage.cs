using UnityEngine;
using UnityEngine.UI;

public class resizeSlotImage : MonoBehaviour
{

    [SerializeField] float resizeScale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        //Reszize the image to half its w and h
        foreach (Transform child in transform)
        {
            Image img = child.GetComponent<Image>();

            if (img != null)
            {
                RectTransform rt = img.GetComponent<RectTransform>();
                if (rt != null)
                {
                    rt.sizeDelta = new Vector2(rt.sizeDelta.x / 2f, rt.sizeDelta.y / 2f);
                }
            }

            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                child.localScale = child.localScale * resizeScale;
            }
        }


    }


}
