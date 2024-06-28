using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ColorDisplayController : MonoBehaviour
{
    [SerializeField] private Color cardsColor;
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


		SetColor(cardsColor);	
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
			playerScript.AddColor(displayMaterial.color);
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
			playerScript.RemoveColor(displayMaterial.color);
		}
			
	}

	/// <summary>
	/// Sets cards color
	/// </summary>
	/// <param name="color"> Color to be set on card</param>
	public void SetColor(Color color)
	{
		cardsColor = color;

		displayMaterial.color = cardsColor;

		int r = Mathf.RoundToInt(color.r * 255);
		int g = Mathf.RoundToInt(color.g * 255);
		int b = Mathf.RoundToInt(color.b * 255);

		colorText = $"({r}, {g}, {b})";

		colorValueText.text = colorText;
		colorAmountText.text = colorAmount.ToString();
	}


	/// <summary>
	/// Resets whole ColorDisplay
	/// </summary>
	public void ResetColor()
	{
		colorAmount = 0;
		colorAmountText.text = colorAmount.ToString();
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
