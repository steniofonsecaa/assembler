using System;
using System.Collections.Generic;

namespace Assembler.SymbolTable;

/// Gerencia o mapeamento entre símbolos (rótulos e variáveis) e endereços de memória.
public class SymbolTable
{
    private readonly Dictionary<string, int> _symbols;
    
    // As variáveis em Hack começam a ser alocadas no endereço 16 da RAM
    private int _nextVariableAddress = 16; 

    public SymbolTable()
    {
        // Inicializa a tabela com os símbolos predefinidos da arquitetura Hack
        _symbols = new Dictionary<string, int>
        {
            { "SP", 0 }, { "LCL", 1 }, { "ARG", 2 }, { "THIS", 3 }, { "THAT", 4 },
            { "R0", 0 }, { "R1", 1 }, { "R2", 2 }, { "R3", 3 }, { "R4", 4 },
            { "R5", 5 }, { "R6", 6 }, { "R7", 7 }, { "R8", 8 }, { "R9", 9 },
            { "R10", 10 }, { "R11", 11 }, { "R12", 12 }, { "R13", 13 }, { "R14", 14 }, { "R15", 15 },
            { "SCREEN", 16384 },
            { "KBD", 24576 }
        };
    }

    /// Adiciona um par (símbolo, endereço) à tabela.
    public void AddEntry(string symbol, int address)
    {
        if (!_symbols.ContainsKey(symbol))
        {
            _symbols.Add(symbol, address);
        }
    }

    /// Adiciona uma nova variável à tabela, alocando-a no próximo endereço de RAM disponível.
    public int AddVariable(string symbol)
    {
        if (!_symbols.ContainsKey(symbol))
        {
            _symbols.Add(symbol, _nextVariableAddress);
            _nextVariableAddress++;
        }
        return _symbols[symbol];
    }

    /// Verifica se a tabela de símbolos já contém o símbolo especificado.
    public bool Contains(string symbol)
    {
        return _symbols.ContainsKey(symbol);
    }

    /// Retorna o endereço associado ao símbolo.
    public int GetAddress(string symbol)
    {
        if (_symbols.TryGetValue(symbol, out int address))
        {
            return address;
        }
        
        throw new KeyNotFoundException($"O símbolo '{symbol}' não foi encontrado na tabela.");
    }
}