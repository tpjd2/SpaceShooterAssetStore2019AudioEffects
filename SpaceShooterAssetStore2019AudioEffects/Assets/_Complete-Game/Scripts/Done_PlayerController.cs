using UnityEngine;
using System.Collections;

[System.Serializable]
public class Done_Boundary 
{
	public float xMin, xMax, zMin, zMax;
}

public class Done_PlayerController : MonoBehaviour
{
	public float speed;
	public float tilt;
	public Done_Boundary boundary;

	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;
    [SerializeField] float volumeInicial;
    [SerializeField] float menorVolume;
    [SerializeField] float maiorVolume;
    [SerializeField] float menorPan;
    [SerializeField] float maiorPan;

    float incrementoAudio;
    Rigidbody rb;
    AudioSource audioSource;
	private float nextFire;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        incrementoAudio = maiorVolume - volumeInicial;
    }

    void Update ()
	{
		if (Input.GetButton("Fire1") && Time.time > nextFire) 
		{
			nextFire = Time.time + fireRate;
			Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            //GetComponent<AudioSource>().Play ();

            maiorVolume = 1.0f - volumeInicial;
            audioSource.volume = volumeInicial
                + ConversorEscala(boundary.xMin, boundary.xMax,
                Mathf.Abs(transform.position.x), menorVolume, maiorVolume);
            audioSource.panStereo =
                ConversorEscala(boundary.xMin, boundary.xMax, transform.position.x,
                menorPan, maiorPan);
            audioSource.Play();
		}
	}

	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		GetComponent<Rigidbody>().velocity = movement * speed;
		
		GetComponent<Rigidbody>().position = new Vector3
		(
			Mathf.Clamp (GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax), 
			0.0f, 
			Mathf.Clamp (GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
		);
		
		GetComponent<Rigidbody>().rotation = Quaternion.Euler (0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
	}

    private float ConversorEscala(float minimo, float maximo, float valor, float minimoFinal, float maximoFinal)
    {
        return ((valor - minimo) / (maximo - minimo))
            * (maximoFinal - minimoFinal) + minimoFinal;
    }
}
