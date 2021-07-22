using System;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    private static ParticleManager instance;

    [Header ("Prefab reference")]
    [SerializeField] private GameObject p_Chomp;
    [SerializeField] private GameObject p_BigChomp;

    [Header ("Particle Array (Generated)")]
    [SerializeField] private ParticleSystem[] p_ChompArray;
    [SerializeField] private ParticleSystem[] p_BigChompArray;

    private const int MAX_CHOMP_PARTICLES = 10;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null)
		{
            Destroy(gameObject);
            return;
		}

        instance = this;

        // Normal chomp particles
        p_ChompArray = new ParticleSystem[MAX_CHOMP_PARTICLES];

        for (int i = 0; i < MAX_CHOMP_PARTICLES; i++)
		{
            p_ChompArray[i] = Instantiate(p_Chomp, transform).GetComponent<ParticleSystem>();
		}

        // Big chomp particles
        p_BigChompArray = new ParticleSystem[MAX_CHOMP_PARTICLES / 2];

        for (int i = 0; i < MAX_CHOMP_PARTICLES / 2; i++)
        {
            p_BigChompArray[i] = Instantiate(p_BigChomp, transform).GetComponent<ParticleSystem>();
        }
    }

    /// <summary>
    /// Find inactive and available chomp particle system and play it
    /// </summary>
    /// <param name="_pos"></param>
    public static void Particle_Chomp(Vector3 _pos)
	{
        if (instance != null)
		{
            ParticleSystem p = Array.Find(instance.p_ChompArray, x => !x.gameObject.activeInHierarchy);

            if (p != null)
			{
                p.transform.position = _pos;
                p.gameObject.SetActive(true);
			}
		}
	}

    /// <summary>
    /// Find inactive and available BIG chomp particle system and play it
    /// </summary>
    /// <param name="_pos"></param>
    public static void Particle_BigChomp(Vector3 _pos)
    {
        if (instance != null)
        {
            ParticleSystem p = Array.Find(instance.p_BigChompArray, x => !x.gameObject.activeInHierarchy);

            if (p != null)
            {
                p.transform.position = _pos;
                p.gameObject.SetActive(true);
            }
        }
    }
}
