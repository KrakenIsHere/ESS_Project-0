using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour {

	public ScriptManager scriptManager;
	private UIManager ui;

	[Header("Health Settings")]
	[Tooltip("Hit points")]
	public float Health = 100.0f;
	[Tooltip("Character maximum hit points")]
	public float maximumHealth = 200.0f;
    public bool regeneration = false;
    public float regenerationSpeed;

	private Text HealthText;
	public Texture PainTexture;

	public AudioClip HealSound;
	public AudioClip AudioDamage;
	
	public Color HealtColor = new Color(255, 255, 255);
    public Color LowHealtColor = new Color(0.9f, 0, 0);
	
	private bool isDead;
    private Color CurColor = new Color(0, 0, 0);
	private float FadeDamage;

	[HideInInspector]
	public bool isMaximum;
	
	void Start()
	{		
		ui = scriptManager.uiManager;
		HealthText = ui.HealthText;
		CurColor = HealtColor;
        FadeDamage = 0.0f;
	}
	
	void Update()
	{
		if(HealthText)
		{
			HealthText.text = "Health: " + System.Convert.ToInt32(Health).ToString();
			HealthText.color = CurColor;
		}

        if (Health <= 15)
        {
            CurColor = Color.Lerp(CurColor, LowHealtColor, (Seno(6.0f, 0.1f, 0.0f) * 5) + 0.5f);
        }
		
		if (FadeDamage > 0)
		{
			FadeDamage -= Time.deltaTime;
		}
		
		if(Health <= 0 || Health <= 0.9)
		{
			Health = 0f;
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}

		if (Health >= maximumHealth) {
			Health = maximumHealth;
			isMaximum = true;
		} else {
			isMaximum = false;
		}
    }
	
	public void ApplyDamage(float damage)
	{
		if(Health <= 0) return;
		Health -= damage;
		
		if(AudioDamage){AudioSource.PlayClipAtPoint(AudioDamage,transform.position,1.0f);}
		FadeDamage = 2.0f;
	}
	
	public void ApplyHeal(float heal)
	{
		if (Health > 0 && !isMaximum)
        {
            Health += heal;
        }
		if (isMaximum) {
			ui.WarningMessage ("You have maximum health");
		}
    }
	
    public static float Seno(float rate, float amp, float offset = 0.0f)
    {
        return (Mathf.Cos((Time.time + offset) * rate) * amp);
    }
	
    void OnGUI()
    {
		if(FadeDamage > 0)
		{
			GUI.color = new Color(1, 1, 1, FadeDamage);
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), PainTexture);
		}
    }
}
