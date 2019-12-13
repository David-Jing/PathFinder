using UnityEngine;
using UnityEngine.UI;

public class ButtonTrigger : MonoBehaviour {

    public void TriggerButton()
    {
        gameObject.GetComponent<Button>().onClick.Invoke();
    }
}
