using TMPro;
using UnityEngine;

public class ColetaItens : MonoBehaviour
{
    private int macasColetadas = 0;
    public TMP_Text textoMaca;

    // Esta função é chamada automaticamente quando o Player atravessa um "Trigger"
    private void OnTriggerEnter2D(Collider2D colisao)
    {
        // Verifica se o objeto que encostamos tem a etiqueta "Coletavel"
        if (colisao.CompareTag("Coletavel"))
        {
            // Destrói o objeto que foi tocado (a fruta desaparece!)
            Destroy(colisao.gameObject);
            macasColetadas++;
            textoMaca.text = "Maçãs: " + macasColetadas;
        }
    }
}