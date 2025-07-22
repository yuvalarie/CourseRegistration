using UnityEngine;
using UnityEngine.UI;

public class CAUICourseOptions : MonoBehaviour
{
    public Image Image { get; private set; }

    private void Start()
    {
        Image = GetComponent<Image>();
        if (Image == null)
        {
            Debug.LogError("Image component not found on the course options.");
        }
    }
    
    public void EnableImage(bool enable)
    {
        if (Image != null)
        {
            Image.enabled = enable;
        }
        else
        {
            Debug.LogError("Image component is not assigned.");
        }
    }
}
