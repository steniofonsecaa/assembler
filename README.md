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
Execute o programa passando o caminho do arquivo .asm como argumento:
```bash
dotnet run -- caminho/para/o/arquivo.asm

## 📌 Justificativa da Escolha da Linguagem

A escolha do C# se deu pela sua forte tipagem e excelente suporte à orientação a objetos. Isso facilitou a implementação da arquitetura modular sugerida pelo livro (separando as responsabilidades em Parser, Code e SymbolTable), além de oferecer manipulação nativa de strings e arquivos de forma muito eficiente, o que é o núcleo do funcionamento de um Assembler.