using UnityEngine;
using UnityEngine.UI;

namespace AG.UI
{
    public class DynamicInfoBox : MonoBehaviour
    {
        private RectTransform parentRect;
        private RectTransform infoBoxRect;
        private RectTransform rectTransform;

        void Start()
        {
            parentRect = transform.parent.GetComponent<RectTransform>();
            infoBoxRect = transform.Find("InfoBox").GetComponent<RectTransform>();
            rectTransform = GetComponent<RectTransform>();

            // Überprüfen, ob die rechte Seite der Infobox die rechte Seite des Elternobjekts überschreitet
            float infoBoxRightX = rectTransform.anchoredPosition.x + rectTransform.rect.width/2 + infoBoxRect.anchoredPosition.x + infoBoxRect.rect.width/2;
            float parentRightX = parentRect.rect.width;
            Debug.Log(infoBoxRightX + " " + parentRightX);
            if (infoBoxRightX > parentRightX)
            {
                // Verschieben Sie die Infobox zur linken Seite des Elternobjekts
                float newXPosition = -infoBoxRect.anchoredPosition.x;
                infoBoxRect.anchoredPosition = new Vector2(newXPosition, infoBoxRect.anchoredPosition.y);
            }
        }
    }
}
