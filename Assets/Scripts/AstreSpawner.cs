using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AstreSpawner : MonoBehaviour
{
    public GameObject astrePrefab;
    public int nombreAstres = 6;

    public float vitesseNormaleMin = 0.8f;
    public float vitesseNormaleMax = 1.2f;
    public float vitesseAnormaleMin = 2f;   // trop rapide
    public float vitesseAnormaleMax = 3f;
    public float vitesseAnormaleLenteMin = 0.1f; // ou trop lente
    public float vitesseAnormaleLenteMax = 0.4f;

    [Range(0f, 1f)] public float probabiliteAnomalieVitesse = 0.35f;
    [Range(0f, 1f)] public float probabiliteAnomalieCouleur = 0.35f;
    [Range(0f, 1f)] public float probabiliteAnomalieDirection = 0.35f;

    public float margeHorsChamp = 1f;
    public float margeVerticale = 0.5f;
    public float variationVerticale = 0.3f;
    public float espacementMinimum = 1.5f;

    public float delaiMinEntreSpawns = 2f;
    public float delaiMaxEntreSpawns = 8f;

    private List<float> hauteursUtilisees = new List<float>();
    private float bordGaucheX;
    private float hauteurMin;
    private float hauteurMax;

    void Start()
    {
        CalculerLimitesCamera();
        StartCoroutine(SpawnerProgressivement());
    }

    IEnumerator SpawnerProgressivement()
    {
        for (int i = 0; i < nombreAstres; i++)
        {
            SpawnUnAstre();
            float delai = Random.Range(delaiMinEntreSpawns, delaiMaxEntreSpawns);
            yield return new WaitForSeconds(delai);
        }
    }

    void CalculerLimitesCamera()
    {
        Camera cam = Camera.main;
        float hauteurVisible = cam.orthographicSize * 2f;
        float largeurVisible = hauteurVisible * cam.aspect;

        bordGaucheX = -(largeurVisible / 2f) - margeHorsChamp;
        hauteurMax = (hauteurVisible / 2f) - margeVerticale;
        hauteurMin = -hauteurMax;
    }

    void SpawnUnAstre()
    {
        float hauteur = TrouverHauteurValide();
        hauteursUtilisees.Add(hauteur);

        Vector3 position = new Vector3(bordGaucheX, hauteur, 0f);
        GameObject nouvelAstre = Instantiate(astrePrefab, position, Quaternion.identity);

        MoveAstre move = nouvelAstre.GetComponent<MoveAstre>();

        // --- Vitesse : anomalie fixe décidée une fois pour toutes, pas de Lerp ---
        bool aAnomalieVitesse = Random.value < probabiliteAnomalieVitesse;
        if (aAnomalieVitesse)
        {
            // 50/50 entre trop rapide et trop lent
            if (Random.value < 0.5f)
                move.speed = Random.Range(vitesseAnormaleMin, vitesseAnormaleMax);
            else
                move.speed = Random.Range(vitesseAnormaleLenteMin, vitesseAnormaleLenteMax);
        }
        else
        {
            move.speed = Random.Range(vitesseNormaleMin, vitesseNormaleMax);
        }

        Vector2 direction = new Vector2(1f, Random.Range(-variationVerticale, variationVerticale));
        move.direction = direction.normalized;

        // --- Couleur : anomalie probabiliste, gérée par une coroutine dans MoveAstre ---
        move.aAnomalieCouleur = Random.value < probabiliteAnomalieCouleur;
        move.aAnomalieDirection = Random.value < probabiliteAnomalieDirection;

        SpriteRenderer sr = nouvelAstre.GetComponent<SpriteRenderer>();
        sr.color = new Color(Random.value, Random.value, Random.value);
    }

    float TrouverHauteurValide()
    {
        int essaisMax = 30;
        for (int essai = 0; essai < essaisMax; essai++)
        {
            float candidat = Random.Range(hauteurMin, hauteurMax);
            bool tropProche = false;
            foreach (float h in hauteursUtilisees)
            {
                if (Mathf.Abs(candidat - h) < espacementMinimum)
                {
                    tropProche = true;
                    break;
                }
            }
            if (!tropProche) return candidat;
        }
        Debug.LogWarning("Impossible de trouver une hauteur bien espacée, espace peut-être trop contraint.");
        return Random.Range(hauteurMin, hauteurMax);
    }
}