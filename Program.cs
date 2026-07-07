using System;
using System.IO;
using Assembler.Parser;
using Assembler.SymbolTable;
using Assembler.Code; 

namespace Assembler;

class Program
{
    static void Main(string[] args)
    {
        // Validação da entrada
        if (args.Length == 0)
        {
            Console.WriteLine("Erro: Nenhum arquivo de entrada fornecido.");
            Console.WriteLine("Uso: dotnet run -- <caminho_do_arquivo.asm>");
            return;
        }

        string inputFile = args[0];
        
        if (!File.Exists(inputFile))
        {
            Console.WriteLine($"Erro: Arquivo '{inputFile}' não encontrado.");
            return;
        }

        // Gera o nome do arquivo de saída trocando a extensão .asm por .hack
        string outputFile = Path.ChangeExtension(inputFile, ".hack");

        var symbolTable = new SymbolTable.SymbolTable();

        // Mapeia Rótulos (Labels)
        var parserPass1 = new Parser.Parser(inputFile);
        int romAddress = 0;

        while (parserPass1.HasMoreInstructions())
        {
            parserPass1.Advance();
            var type = parserPass1.InstructionType();

            if (type == CommandType.LABEL)
            {
                string symbol = parserPass1.Symbol();
                // Associa o nome do rótulo ao endereço da PRÓXIMA instrução na ROM
                symbolTable.AddEntry(symbol, romAddress);
            }
            else
            {
                // Incrementa o endereço da ROM apenas para instruções reais (A ou C)
                romAddress++;
            }
        }

        // Geração de Código Binário
        var parserPass2 = new Parser.Parser(inputFile);
        
        // StreamWriter para escrever o arquivo linha por linha de forma eficiente
        using var writer = new StreamWriter(outputFile);

        while (parserPass2.HasMoreInstructions())
        {
            parserPass2.Advance();
            var type = parserPass2.InstructionType();

            if (type == CommandType.A_INSTRUCTION)
            {
                string symbol = parserPass2.Symbol();
                int address;

                // Tenta fazer o parse para inteiro (ex: @100)
                if (int.TryParse(symbol, out int numericValue))
                {
                    address = numericValue;
                }
                else
                {
                    // Se não é número, é símbolo. Se não existe na tabela, é variável nova.
                    if (!symbolTable.Contains(symbol))
                    {
                        symbolTable.AddVariable(symbol);
                    }
                    address = symbolTable.GetAddress(symbol);
                }

                // Converte o endereço decimal para binário de 15 bits e adiciona o '0' da instrução A
                string binaryAddress = Convert.ToString(address, 2).PadLeft(15, '0');
                writer.WriteLine("0" + binaryAddress);
            }
            else if (type == CommandType.C_INSTRUCTION)
            {
                string dest = parserPass2.Dest();
                string comp = parserPass2.Comp();
                string jump = parserPass2.Jump();

                // Busca a tradução em binário no módulo Code
                string compBits = Code.Code.Comp(comp);
                string destBits = Code.Code.Dest(dest);
                string jumpBits = Code.Code.Jump(jump);

                // Concatena a estrutura da instrução C (111 + comp + dest + jump)
                writer.WriteLine("111" + compBits + destBits + jumpBits);
            }
            // Instruções do tipo LABEL são ignoradas na segunda passagem, pois já foram processadas.
        }

        Console.WriteLine($"Montagem concluída com sucesso!");
        Console.WriteLine($"Arquivo de máquina gerado em: {outputFile}");
    }
}