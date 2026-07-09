using UnityEngine;
using System.Collections;

public class MoveAstre : MonoBehaviour
{
    public float speed = 2f;
    public Vector2 direction = Vector2.right;

    public bool aAnomalieCouleur = false;
    public bool aAnomalieDirection = false;

    void Start()
    {
        if (aAnomalieCouleur)
        {
            StartCoroutine(DeclencherChangementCouleur());
        }

        if (aAnomalieDirection)
        {
            StartCoroutine(DeclencherChangementDirection());
        }
    }

    void Update()
    {
        transform.position += (Vector3)(direction.normalized * speed * Time.deltaTime);
    }

    IEnumerator DeclencherChangementCouleur()
    {
        float delaiAvantChangement = Random.Range(3f, 10f);
        yield return new WaitForSeconds(delaiAvantChangement);

        Color couleurCible = new Color(Random.value, Random.value, Random.value);
        float dureeTransition = 2f;

        yield return StartCoroutine(ChangerCouleurProgressivement(couleurCible, dureeTransition));
    }

    IEnumerator ChangerCouleurProgressivement(Color couleurCible, float duree)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color couleurDepart = sr.color;
        float tempsEcoule = 0f;

        while (tempsEcoule < duree)
        {
            tempsEcoule += Time.deltaTime;
            float t = tempsEcoule / duree;
            sr.color = Color.Lerp(couleurDepart, couleurCible, t);
            yield return null;
        }

        sr.color = couleurCible;
    }

    IEnumerator DeclencherChangementDirection()
    {
        float delaiAvantChangement = Random.Range(3f, 10f);
        yield return new WaitForSeconds(delaiAvantChangement);

        // Nouvelle direction : on garde une composante X positive pour ne pas repartir en arrière,
        // mais on autorise une déviation verticale plus marquée qu'à l'origine
        Vector2 directionCible = new Vector2(1f, Random.Range(-1f, 1f)).normalized;
        float dureeTransition = 2.5f;

        yield return StartCoroutine(ChangerDirectionProgressivement(directionCible, dureeTransition));
    }

    IEnumerator ChangerDirectionProgressivement(Vector2 directionCible, float duree)
    {
        Vector2 directionDepart = direction;
        float tempsEcoule = 0f;

        while (tempsEcoule < duree)
        {
            tempsEcoule += Time.deltaTime;
            float t = tempsEcoule / duree;
            direction = Vector2.Lerp(directionDepart, directionCible, t).normalized;
            yield return null;
        }

        direction = directionCible;
    }
}