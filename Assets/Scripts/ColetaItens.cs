using UnityEngine;

public class ColetaItens : MonoBehaviour
{
    // Esta função é chamada automaticamente quando o Player atravessa um "Trigger"
    private void OnTriggerEnter2D(Collider2D colisao)
    {
        // Verifica se o objeto que encostamos tem a etiqueta "Coletavel"
        if (colisao.CompareTag("Coletavel"))
        {
            // Destrói o objeto que foi tocado (a fruta desaparece!)
            Destroy(colisao.gameObject);
        }
    }
}