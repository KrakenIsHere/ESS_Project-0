using Vexe.Runtime.Types;
using UnityEngine;

public class RanbowCubeScript : BaseBehaviour
{
    private string _color;

    [Show] public string color
    {
        get
        {
            return _color;
        }
        set
        {
            _color = value;
        }
    }
    Renderer rend;

    [fMin(0.1f)]
    [fMax(100f)]
    public float speed = 20f;    

    void Start()
    {
        rend = gameObject.GetComponent<Renderer>();
        color = rend.material.color.ToString();
    }

    void Update()
    {
        rend.material.SetColor("_Color", HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time * speed, 1), 1, 1)));
        color = rend.material.color.ToString();
    }
}
