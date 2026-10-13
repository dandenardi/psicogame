# Tarefas — 001 Estrutura inicial do projeto Unity

## Bloco A — preparação (sem depender do Unity Editor)

- [x] Inicializar repositório git na raiz do projeto
- [x] Adicionar `.gitignore` padrão Unity
- [x] Commit inicial com `docs/`, `specs/` e `.gitignore`

## Bloco B — manual, Daniel (bloqueia o resto)

- [x] Instalar Unity Hub
- [x] Instalar Unity Editor (6000.6.5f1, com módulos Android, iOS e WebGL Build Support)
- [x] Criar projeto 2D (Built-In Render Pipeline — Universal 2D/URP tem bug de compilação
      nessa versão do Editor) em `C:\programming\freudinho`

## Bloco C — depois que o projeto Unity existir

- [x] Criar estrutura de pastas `Assets/` conforme `ARQUITETURA.md`
- [x] Criar cena `Boot.unity`
- [x] Criar cena `Office.unity`
- [x] `Boot.unity` carrega e troca para `Office.unity` (via `BootLoader.cs`)
- [x] Criar scripts-base (stubs) em `Scripts/Systems` e `Scripts/Core`
- [x] Criar ScriptableObjects-base em `Scripts/Data`
- [ ] Commit da estrutura
