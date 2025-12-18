# ☀️ Bright Squad – Builds v1.0

Repositório público com as builds oficiais do jogo **Bright Squad – O Futuro é Claro**, desenvolvido pela organização **The Bright Games** em parceria com iniciativas de impacto social.

## Visão geral do jogo

Bright Squad é um jogo 3D casual focado em **coleta seletiva de lixo, reciclagem e cuidado com a comunidade**.  
O jogador controla um dos integrantes do squad e precisa **limpar o bairro, separar resíduos corretamente e cumprir missões** dentro de um tempo limitado, acumulando pontos e desbloqueando melhorias e recompensas visuais.

## Plataformas e builds

- **Android**: APK para instalação em dispositivos móveis.  
- **WebGL**: versão jogável diretamente no navegador.  
- **Windows**: executável para PCs Windows (x86_64).

A estrutura deste repositório é:

```text
Android/   -> BrightSquad_v1.0.apk
WebGL/     -> build WebGL (index.html, Build/, TemplateData/, StreamingAssets/)
Windows/   -> BrightSquad_v1.0_Win.zip (EXE + pastas de dados)
```

## Mecânicas principais

- **Exploração do bairro**  
  - Movimento livre do personagem em terceira pessoa pelos cenários da comunidade.  
  - Interação com NPCs que explicam objetivos, dicas e contexto social das missões.

- **Coleta e separação de lixo**  
  - Objetos de lixo espalhados pelas ruas e pontos-chave do cenário.  
  - Cada item pertence a uma categoria (papel, plástico, metal, vidro, orgânico etc.).  
  - O jogador deve **carregar o lixo até a lixeira correta**, evitando descartes errados.

- **Sistema de missões**  
  - Missões principais de “limpar áreas” (rua, praça, entorno de equipamentos públicos).  
  - Missões secundárias ligadas a personagens da comunidade e situações específicas de descarte.  
  - Algumas missões usam **checkpoints de progresso** e salvamento rápido para continuidade.

## Pontuação e feedback

- **Pontuação por acertos**  
  - Cada lixo colocado na lixeira correta concede pontos.  
  - Combos de acertos consecutivos geram **bônus de pontuação** e mensagens positivas na HUD.  

- **Penalidades leves**  
  - Descarte incorreto reduz a pontuação do combo ou concede menos pontos.  
  - Feedback visual e textual indica qual seria o descarte correto, reforçando o aprendizado.

- **Progresso de missão**  
  - A barra de progresso e indicadores de objetivos mostram quanto falta para concluir a missão atual.  
  - Ao terminar, o jogador recebe um resumo com **tempo, quantidade de lixo coletada e acertos**.

## Interface e interações

- **HUD dinâmica**  
  - Exibe pontuação, tempo/missão, ícones de tipo de lixo e indicadores de objetivos.  
  - Botão de **pause** com menu de retorno ao jogo, opções e saída para o menu principal.

- **Menus e fluxo de cenas**  
  - **Splash / Logos** das organizações parceiras.  
  - **Main Menu** com opções de jogar, configurações de áudio e créditos.  
  - **Tela de Loading** com personagem animado e dicas rápidas sobre reciclagem.  
  - **Tela de Resultado** ao final das missões, exibindo desempenho do jogador.

- **Áudio**  
  - Música ambiente temática e efeitos para coleta de lixo, acertos e interações.  
  - Controlador de áudio global com salvamento de preferências do jogador (volume geral, música, efeitos).

## Salvamento e continuidade

- Sistema de **quicksave** salva progresso básico da missão e algumas preferências do jogador.  
- Ao entrar de novo na cena, o jogo tenta restaurar posição, estado das missões e configurações de áudio, quando aplicável.

## Créditos e licenças

- Jogo desenvolvido pela **The Bright Games**.  
- Algumas imagens, logos e marcas exibidas no jogo pertencem a seus respectivos proprietários e são usadas com autorização específica.  
- Este repositório contém **apenas builds compiladas**; o código-fonte e assets originais permanecem em repositórios privados da organização.


