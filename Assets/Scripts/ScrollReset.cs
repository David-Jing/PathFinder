using UnityEngine;
using UnityEngine.UI;

public class ScrollReset : MonoBehaviour
{
    public void ResetScrollPosition()
    {
        gameObject.GetComponent<ScrollRect>().verticalNormalizedPosition = 1f;
    }
}
