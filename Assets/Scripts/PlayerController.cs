using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Componentes")]
    private Rigidbody2D rb;
    private CapsuleCollider2D col;
    private Animator anim;
    private SpriteRenderer sprite;

    [Header("Movimento Horizontal")]
    [SerializeField] private float moveSpeed = 8f;          // Velocidade de movimento
    private float horizontalInput;

    [Header("Pulo")]
    [SerializeField] private float jumpForce = 12f;         // Força do pulo
    [SerializeField] private float doubleJumpForce = 10f;   // Força do pulo duplo
    [SerializeField] private int extraJumps = 1;            // Quantidade de pulos extras (1 = pulo duplo)
    private int jumpCount;
    private bool jumpPressed;

    [Header("Checagem de Chão")]
    [SerializeField] private Transform groundCheck;         // Ponto de checagem (objeto vazio nos pés)
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;         // Layer do chão

    [Header("Checagem de Parede")]
    [SerializeField] private Transform wallCheck;           // Ponto de checagem lateral
    [SerializeField] private float wallCheckRadius = 0.2f;
    [SerializeField] private LayerMask wallLayer;           // Layer das paredes

    [Header("Deslize na Parede")]
    [SerializeField] private float wallSlideSpeed = .5f;     // Velocidade máxima de queda ao deslizar
    [SerializeField] private float wallJumpForceX = 8f;     // Força horizontal do pulo da parede (opcional)
    [SerializeField] private float wallJumpForceY = 12f;    // Força vertical do pulo da parede (opcional)

    // Estados
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool facingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Captura inputs no Update para não perder nenhum clique
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            jumpPressed = true;
        }

        // Verifica estados
        CheckWorld();
        CheckWallSlide();
        Flip();
        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        // Aplica movimento horizontal
        Move();

        // Aplica pulo
        if (jumpPressed)
        {
            Jump();
            jumpPressed = false;
        }
    }

    private void CheckWorld()
    {
        // Verifica se está no chão
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Verifica se está tocando parede
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, wallLayer);

        // Reseta contagem de pulos ao tocar o chão
        if (isGrounded)
        {
            jumpCount = 0;
        }
    }

    private void CheckWallSlide()
    {
        // Só desliza se estiver tocando a parede, não estiver no chão e estiver caindo
        if (isTouchingWall && !isGrounded && rb.linearVelocity.y < 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }

        // Aplica a velocidade de deslize
        if (isWallSliding)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -wallSlideSpeed, float.MaxValue));
        }
    }

    private void Move()
    {
        // Movimento horizontal
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpCount = 0;
        }
        else if (jumpCount < extraJumps) // Seu Desafio do Pulo Duplo
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, doubleJumpForce);
            jumpCount++;

            // NOVA LINHA: Dispara a animação da cambalhota!
            anim.SetTrigger("doubleJump");
        }

    }

    private void Flip()
    {
        // Vira o personagem conforme a direção do movimento
        if (horizontalInput > 0 && !facingRight)
        {
            facingRight = true;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (horizontalInput < 0 && facingRight)
        {
            facingRight = false;
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void UpdateAnimations()
    {
        // 1. Correndo (True se o Input não for zero)
        bool isRunning = horizontalInput != 0;
        anim.SetBool("isRunning", isRunning);

        // 2. Chão (Usa a mesma variável do nosso CheckWorld!)
        anim.SetBool("isGrounded", isGrounded);

        // 3. Pulo e Queda (Envia a velocidade vertical do Rigidbody)
        anim.SetFloat("yVelocity", rb.linearVelocity.y);

        anim.SetBool("isWallSliding", isWallSliding);

        // NOVA LÓGICA: Inverte a pintura do sprite se estiver na parede
        if (isWallSliding)
        {
            // flipX = true inverte o desenho em relação à física atual
            sprite.flipX = true;
        }
        else
        {
            // Volta ao normal quando sair da parede
            sprite.flipX = false;
        }

    }


    // Desenha gizmos para facilitar a configuração no Editor
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
        if (wallCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);
        }
    }
}