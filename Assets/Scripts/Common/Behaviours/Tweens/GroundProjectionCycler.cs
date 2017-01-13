using UnityEngine;

public class GroundProjectionCycler : MonoBehaviour
{
	private Projector proj;
	private Material mat;
	private int powerID;
	private float currentPower;
	private float elapsedTime;
	private float progress;

	public float StartingPower;
	public float EndingPower;
	public float FadeCycleTime;

	// Use this for initialization
	void Start ()
	{
		proj = gameObject.GetComponent<Projector>();
		mat = proj.material;
		powerID = Shader.PropertyToID("_Power");
		//StartingPower = mat.GetFloat(powerID);
		currentPower = StartingPower;
		elapsedTime = 0;
	}

	void OnDestroy()
	{
		mat.SetFloat(powerID, 1.0f);
	}

	// Update is called once per frame
	void Update ()
	{
		elapsedTime += Time.deltaTime;
		progress = elapsedTime / FadeCycleTime;
		if (progress > 1.0)
		{
			progress = 0;
			elapsedTime = 0;
		}

		currentPower = progress > 0.5f 
			? Mathf.Lerp(StartingPower, EndingPower, (progress - 0.5f) * 2f) 
			: Mathf.Lerp(EndingPower, StartingPower, progress * 2f);

		mat.SetFloat(powerID, currentPower);

	}
}
