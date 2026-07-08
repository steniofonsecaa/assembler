# Assembler Hack - Projeto 6 (Nand2Tetris)

Implementação de um Assembler para a arquitetura Hack, desenvolvido como requisito da disciplina de Compiladores. Este programa traduz instruções em linguagem Assembly (`.asm`) para código de máquina em binário (`.hack`).

## 👥 Integrantes
* **Stenio Moraes Fonseca** - Matrícula: 20250013686


## 💻 Linguagem e Versão Utilizada
* **Linguagem:** C#
* **Framework:** .NET (Console Application)

## 🚀 Instruções de Build e Execução

Certifique-se de ter o SDK do .NET instalado em sua máquina.

**1. Para compilar o projeto:**
Abra o terminal na raiz do projeto e execute:
```bash
dotnet build
```

**2. Para executar o tradutor:**
Execute o programa passando o caminho do arquivo ```.asm``` como argumento:
```bash
dotnet run -- caminho/para/o/arquivo.asm
```

## 🧪 Validação e Testes

Para garantir a exatidão da tradução bit a bit, a validação é feita comparando o arquivo ```.hack``` gerado pelo nosso compilador com o gabarito oficial fornecido pela arquitetura, utilizando a ferramenta de comparação do Git.

**Passo 1: Gere o arquivo binário a partir do código fonte de teste.**
```bash
dotnet run -- Tests/Add.asm
```

(Isso irá gerar um arquivo Add.hack dentro da pasta Tests)

**Passo 2: Compare a saída com o arquivo de referência oficial.**
```bash
git diff --no-index Tests/Add.hack Gabarito/Add.hack
```

Nota: Se a comparação for bem-sucedida e os arquivos forem 100% idênticos, o terminal permanecerá em "silêncio". Qualquer divergência de bits será impressa na tela.

Este mesmo fluxo foi utilizado para validar com sucesso arquivos mais complexos contendo variáveis e loops, como Max.asm, Rect.asm e o teste de estresse Pong.asm.

## 📌 Justificativa da Escolha da Linguagem

A escolha do C# se deu pela sua forte tipagem e excelente suporte à orientação a objetos. Isso facilitou a implementação da arquitetura modular sugerida pelo livro (separando as responsabilidades em ```Parser```, ```Code``` e ```SymbolTable```), além de oferecer manipulação nativa de strings e arquivos de forma muito eficiente, o que é o núcleo do funcionamento de um Assembler.

## 🎥 Vídeo de Apresentação
No vídeo abaixo, é feita uma explicação detalhada sobre a arquitetura do código, o fluxo de dados em Duas Passagens (Two-Pass), a alocação de memória e a demonstração prática da execução, incluindo a renderização do jogo Pong no CPU Emulator provando o funcionamento da Tabela de Símbolos.

🔗 [Clique aqui para assistir ao Vídeo de Apresentação](https://drive.google.com/file/d/1oeOyrDeHLJZwj-53u6lt_cZvxNPVVMB4/view?usp=sharing)