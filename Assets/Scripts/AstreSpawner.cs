using UnityEngine;

public class AstreSpawner : MonoBehaviour
{
    public GameObject astrePrefab;
    public int nombreAstres = 20;
    public float vitesseMin = 0.5f;
    public float vitesseMax = 3f;

    void Start()
    {
        for (int i = 0; i < nombreAstres; i++)
        {
            SpawnUnAstre();
        }
    }

    void SpawnUnAstre()
    {
        Vector3 position = new Vector3(
            Random.Range(-8f, 8f),
            Random.Range(-4f, 4f),
            0f
        );

        GameObject nouvelAstre = Instantiate(astrePrefab, position, Quaternion.identity);

        MoveAstre move = nouvelAstre.GetComponent<MoveAstre>();
        move.speed = Random.Range(vitesseMin, vitesseMax);
        move.direction = Random.insideUnitCircle.normalized;

        SpriteRenderer sr = nouvelAstre.GetComponent<SpriteRenderer>();
        sr.color = new Color(Random.value, Random.value, Random.value);
    }
}