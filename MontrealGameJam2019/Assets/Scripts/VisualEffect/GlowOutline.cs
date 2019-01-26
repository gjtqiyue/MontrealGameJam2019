using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GlowOutline : MonoBehaviour
{
    [SerializeField]
    List<Material> glowMats;
    public Color targetColor;
    public float factor;

    Color currentColor;

    // Start is called before the first frame update
    void Start()
    {
        glowMats = new List<Material>();
    }

    // Update is called once per frame
    void Update()
    {
        currentColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * factor);

        for (int i=0; i<glowMats.Count; i++)
        {
            glowMats[i].SetColor("_GlowColor", currentColor);
        }
    }
}
