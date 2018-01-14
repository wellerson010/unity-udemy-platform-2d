# O quê é?

Esse é o projeto final do curso da Udemy [Aprenda a criar jogos com a Unity na prática](https://www.udemy.com/aprenda-criar-jogos-com-unity-na-pratica/), ministrado por Gustavo Larsen.

## Anotações

### Mathf.SmoothDamp

É uma forma simples de fazer um valor chegar gradualmente até um outro. Pode ser usado para suavizar o movimento da câmera, por exemplo. Bem semelhante ao método **Lerp**, mas o método **Lerp** é completamente estático, e faz essa troca de valor seguindo sempre o mesmo padrão.

```
float posX = Mathf.SmoothDamp(transform.position.x, Player.position.x, ref Velocity.x, SmoothTimeX);
float posY = Mathf.SmoothDamp(transform.position.y, Player.position.y, ref Velocity.y, SmoothTimeY);

transform.position = new Vector3(posX, posY, transform.position.z);
```

Primeiro parâmetro - Valor inicial
Segundo parâmetro - Valor final
Terceiro parâmetro - Referência que mantém o valor atual que está sendo transformado gradualmente
Quarto parâmetro - O tempo que leva para o valor ser transformado

### FixedUpdate vs Update

O método **Update** é chamada em cada frame. Ele deve ser usado para mover objetos não-físicos, para receber inputs e para timers simplificados. O tempo entre as chamadas pode ser irregular.

O método **FixedUpdate** deve ser usado para alterações físicas, já que ele é chamado a cada passo da física. Ele independe do **frame rate**. O tempo entre as chamadas é regular.

Um jogo com um baixo frame rate terá inúmeros updates físicos durante cada renderização do frame (ou seja, o FixedUpdate será, nesse caso, chamado mais vezes que o Update), enquanto um jogo com um frame rate muito algo, poderá resultar em FixedUpdate sendo chamado menos vezes que o Update, já que pode acontecer de não ocorrer cálculos físicos entre a renderização dos frames.

### GameObject.find

```
GameObject.Find("Player")
```

Encontra um objeto pelo nome.

### Random.insideUnitCircle

```
Vector2 shakePosition = Random.insideUnitCircle * ShakeAmount;
```

Retorna um ponto aleatório entre um círculo com raio 1.

### local position vs position

```
transform.position = new Vector3(transform.position.x + shakePosition.x, transform.position.y + shakePosition.y, transform.position.z);
```

position -> posição do **transform** no espaço global.

localPosition -> posição do **transform** relativo ao transform pai. Se não houver transform pai, é o mesmo do **position**.

### Time.deltaTime

Delta é a diferença ou intervalo entre dois valores. No Unity, o **deltaTime** nos diz quanto tempo levou para o computador carregar o próximo frame do jogo. É importante pois nos ajuda a realizar tarefa cronometrada, como por exemplo executar uma animação por exatos 10 segundos, ou calcular o FPS do jogo.

Deve-se multiplicar os valores que precisam ser constantes entre os quadros pelo **Time.deltaTime**.

Fonte: [Fábrica de Jogos](http://www.fabricadejogos.net/posts/tutorial-entendo-o-que-o-deltatime/) -
[Unity] (https://unity3d.com/pt/learn/tutorials/topics/scripting/delta-time)

### Colisor como trigger

```
Is trigger
```

Nesse caso, o colisor atua apenas como um gatilho pra execução de um código, que pode ser detectado via código através do método **OnTriggerEnter2D**:

```
private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Enemy")){
        PlayerScript.DamagePlayer();
    }

    if (collision.CompareTag("Coin"))
    {
        SoundManager.instance.PlaySound(FXCoin);
        Destroy(collision.gameObject);
    }
}
```

### Destruir um objeto assim que sua animação chegar ao fim

```
Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
```

O método **GetCurrentAnimatorStateInfo** retorna uma instância da classe **AnimatorStateInfo**. O parâmetro único desse método é o índice da layer.

**AnimatorStateInfo** possui algumas outras propriedades como **loop** e **speed**.

### Translate

```
transform.Translate(Vector2.right * Speed * Time.deltaTime);
```

Move o **transform** na direção passada.

### Vector2.right, etc...

O **Vector2** e o **Vector3** possuem propriedades estáticas chamadas **up**, **right**, **left** e **down**, que é um atalho. Exemplo:

```
//left
Vector3(-1, 0, 0)

//right
Vector3(1, 0, 0)
```

### Trocar o Sprite

```
gameObject.sprite = newSprite;
```

### Coroutines

Quando uma função é chamada, o próximo frame só é executado quando essa função termina. Ou seja, ela sempre é executada em apenas um **ÚNICO** frame. Isso impede que seja possível criar, em vias normais, funções que contenham animações procedurais ou uma sequência de eventos de acordo com o tempo. Exemplo: criar uma função que gradualmente reduz o alfa do objeto (opacidade) até ele se tornar completamente invisível.

```
void Fade() {
    for (float f = 1f; f >= 0; f -= 0.1f) {
        Color c = renderer.material.color;
        c.a = f;
        renderer.material.color = c;
    }
}
```

O exemplo acima não funciona por que a função **Fade** vai ser executada e terminada dentro de um único frame. Ou seja, o jogador não vai perceber a mudança. A solução pra isso é usar **coroutines**, uma função que tem a habilidade de pausar a execução e retornar o controle ao Unity. No próxima frame, o Unity irá retornar o controle a tal função, que poderá continuar a partir dali.

```
IEnumerator Fade() {
    for (float f = 1f; f >= 0; f -= 0.1f) {
        Color c = renderer.material.color;
        c.a = f;
        renderer.material.color = c;
        yield return null;
    }
}
```

### Physics2D.Linecast

**Linecast** é uma linha imaginária entre dois pontos. Qualquer objeto que colide com essa linha imaginária pode ser detectado e analisado.

```
Physics2D.Linecast(transform.position, GroundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
```

Primeiro parâmetro -> Início do ponto
Segundo parâmetro -> Fim do ponto
Terceiro parâmetro -> LayerMask. Filtra o colisor em layers específicas
Quarto parâmetro -> Incluir apenas objetos apenas com a coordenada Z maior ou igual a esse valor
Quinto parâmetro -> Incluir apenas objetos apenas com a coordenada Z menor ou igual a esse valor

Essa função retorna um objeto do tipo **RaycastHit2D**. Esse objeto implementa uma conversão implícita pro tipo booleana. Então é possível usar a função **Linecast** associando seu valor a uma variável do tipo booleano e assim verificar se algum objeto atingiu essa linha.

O terceiro parâmetro pode ser um pouco confuso. A função espera um **bit mask** da layer, não um índice. Com isso, é possível definir múltiplas layers, excluir alguma em especial...

Ex: **~(1<<8)**, significa todas as layers, exceto a **8**.

Fonte: [Gyanendu Shekhar's Blog](http://gyanendushekhar.com/2017/06/29/understanding-layer-mask-unity-5-tutorial/)

### Input

#### GetButton

Returna verdadeiro enquanto o botão estiver pressionado. Como por exemplo para atirar.

#### GetButtonDown / GetKeyDown

Deve ser chamado no método **Update**, pois ele é resetado a cada frame (se torna false no início de novo frame). Por isso, ele deve ser usado para ações que o pressionar do botão não seja apropriado, como a ação de pular. 

**GetKeyDown** aceita um **KeyCode** ou um nome representando a tecla.

#### GetKey

Retorna verdadeiro se a tecla passada for pressionada. Aceita um **KeyCode** ou um nome representando a tecla.


### AddForce

Aplica uma força no **RigidBody** em uma determinada direção.

```
RB2D.AddForce(new Vector2(0, JumpForce));
```

A direção é o primeiro parâmetro. Esse método também aceita um segundo parâmetro, determinando o modo como a força deve ser aplicado: aceleração, impulso ou velocidade.

### Invertendo um elemento

```
transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.y);
```

### Transform.eulerAngles

Define a rotação como ângulos de Euler em graus. Somente a use para ler e definir ângulos para valores absolutos. Não incremente nunca, pois ele irá falhar caso o ângulo exceda 360 graus.

```
cloneAttack.transform.eulerAngles = new Vector3(180, 0, 180);
``` 

### Quaternion.identity

Este **quaterion** corresponde a "sem rotação".

### Limitar uma ação

```
NextAttack = Time.time + AttackRate;

if (Input.GetButton("Fire1") && Grounded && Time.time > NextAttack)
```

### Invoke

```
Invoke("ReloadLevel", 3f);
```

Invoca o método passado em tantos segundos.

### Reload Level

```
SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
```

O modo **Single** fecha todas as outras cenas. O modo **Additive** adiciona a tal cena para as cenas atualmente carregadas.


### Mexer um objeto sozinho

Através do **velocity**:

```
RGBD2D.velocity = new Vector2(Speed, RGBD2D.velocity.y);

//invertendo o objeto acima
transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y);
Speed *= -1;
```

### Singleton

```
public static SoundManager instance = null;

void Awake () {
    if (instance == null)
    {
        instance = this;
    }
    else
    {
        Destroy(gameObject);
    }

    DontDestroyOnLoad(gameObject);
}
```

### Layer Collision Matrix

Edit -> Project Settings -> Physics2D

Define as layers que podem se colidir ou não.

### Canvas - Render Mode

* Screen Space Camera - A UI segue a câmera. Nesse modo, as configurações da câmera alteram a UI. Qualquer movimentação de câmera, gera uma renderização da UI. Quanto mais elementos na UI, mais processamento é necessário. OBS: Lembre-se de puxar a câmera pro campo **Render Camera**.

* Screen Space Overlay - Elementos UI são renderizados no topo da cena. Se a tela é redimensionada, o canvas irá se adaptar.

* World Space - A UI se comportará como qualquer outro objeto da cena. Isso é bom para UIs que fazem parte do mundo, conhecidas como **diegetic interface**, presente em jogos como **Dead Space**.

### Resolvendo bug de grudar na parede ###

Basta criar um **Physic Material 2D** com fricção **0** e adicionar no colisor da parede.