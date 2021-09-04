using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pingEffect : MonoBehaviour
{
    private Rigidbody rb;
    public float lifespan;
    private Image image; private Vector4 imageCol;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();

    }

    private void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (lifespan >= 0)
        {
            lifespan = lifespan - Time.deltaTime;
            imageCol = new Vector4(image.color.r, image.color.g, image.color.b, Mathf.Lerp(0f, 1f, lifespan));
            image.color = imageCol;
        }
        else
        {
            lifespan = 0;
            Destroy(gameObject);
        }
        
    }

    private void FixedUpdate()
    {
        
    }
}
