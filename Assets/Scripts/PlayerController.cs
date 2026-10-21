using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Componentes")]
    private Rigidbody2D rb;
    private CapsuleCollider2D col;

    [Header("Movimento Horizontal")]
    [SerializeField] private float velocidadeMovimento = 8f;
    private float horizontalInput;

    // A direção para a qual o personagem está olhando
    private bool olhandoParaDireita = true;

    private void Awake()
    {
        // O Awake roda antes mesmo do jogo começar. Usamos para pegar os componentes.
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        // Captura os botões do teclado (A/D ou Setas) no Update para não perder nenhum clique.
        // Retorna -1 (esquerda), 0 (parado) ou 1 (direita).
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Chama a função que vira o personagem
        Flip();
    }

    private void FixedUpdate()
    {
        // FixedUpdate roda em intervalos fixos e é o lugar OBRIGATÓRIO para aplicar física.
        Mover();
    }

    private void Mover()
    {
        // Usamos rb.linearVelocity (o padrão da Unity 6) para mover o personagem.
        // No Eixo X: multiplicamos a direção pela velocidade.
        // No Eixo Y: mantemos a velocidade de queda que a gravidade já está aplicando.
        rb.linearVelocity = new Vector2(horizontalInput * velocidadeMovimento, rb.linearVelocity.y);
    }

    private void Flip()
    {
        // Vira o personagem fisicamente (usando localScale) conforme a direção do movimento
        if (horizontalInput > 0 && !olhandoParaDireita)
        {
            olhandoParaDireita = true;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (horizontalInput < 0 && olhandoParaDireita)
        {
            olhandoParaDireita = false;
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
}