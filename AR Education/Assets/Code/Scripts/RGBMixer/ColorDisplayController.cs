using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

public class ColorDisplayController : MonoBehaviour
{
    [SerializeField] private UnityEngine.Color cardsColor;
    [SerializeField] private Material displayMaterial;
    [SerializeField] private TextMeshProUGUI colorValueText;
    [SerializeField] private TextMeshProUGUI colorAmountText;

    private int colorAmount = 0;
    string colorText;

	Camera mainCamera;
	[SerializeField] private GameObject arCameraGameObject;
	private Player playerScript;



	// Start is called before the first frame update
	void Start()
    {
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		playerScript = arCameraGameObject.GetComponent<Player>();

		displayMaterial.color = cardsColor;

        colorText = "#" + ColorUtility.ToHtmlStringRGB(displayMaterial.color);

        colorValueText.text = colorText;
        colorAmountText.text = colorAmount.ToString();

		
	}

	/// <summary>
	/// Adds 1 color to mixer
	/// </summary> 
	public void AddColor()
    {
        if (colorAmount < 9) 
        {
			colorAmount += 1;
			colorAmountText.text = colorAmount.ToString();
			playerScript.MixColors(displayMaterial.color);
			Debug.Log("Added color #"+ ColorUtility.ToHtmlStringRGB(displayMaterial.color) + " to mixer");
		}
        
	}

	/// <summary>
	/// Removes 1 color from mixer
	/// </summary>  
	public void RemoveColor()
	{
        if (colorAmount > 0) 
        {
			colorAmount -= 1;
			colorAmountText.text = colorAmount.ToString();
		}
			
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
