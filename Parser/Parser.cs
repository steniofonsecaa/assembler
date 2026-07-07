using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Assembler.Parser;

/// Define os tipos de instrução da arquitetura Hack.
public enum InstructionType
{
    A_INSTRUCTION,
    C_INSTRUCTION,
    LABEL
}

/// Le o arquivo fonte .asm, remove espaços e comentários e isola casa compomente das instruções
public class Parser
{
    private readonly List<string> _lines;
    private int _currentIndex;
    private string _currentLine = string.Empty;

    public Parser(string filename)
    {
        if (!File.Exists(filename))
        {
            throw new FileNotFoundException($"Arquivo fonte não encontrado: {filename}");
        }

        // Lê todas as linhas, remove comentários/espaços e descarta linhas vazias
        _lines = File.ReadLines(filename)
            .Select(CleanLine)
            .Where(line => !string.IsNullOrEmpty(line))
            .ToList();

        _currentIndex = 0;
    }

    /// Remove comentários de linha (//) e todos os espaços em branco ou quebras.
    private static string CleanLine(string line)
    {
        int commentIndex = line.IndexOf("//");
        if (commentIndex >= 0)
        {
            line = line.Substring(0, commentIndex);
        }
        
        return line.Replace(" ", "")
                   .Replace("\t", "")
                   .Replace("\r", "")
                   .Replace("\n", "");
    }

    public bool HasMoreInstructions()
    {
        return _currentIndex < _lines.Count;
    }

    public void Advance()
    {
        if (!HasMoreInstructions())
        {
            throw new InvalidOperationException("Fim do arquivo alcançado. Não há mais instruções.");
        }

        _currentLine = _lines[_currentIndex];
        _currentIndex++;
    }

    public InstructionType InstructionType()
    {
        if (_currentLine.StartsWith('@'))
        {
            return InstructionType.A_INSTRUCTION;
        }
        
        if (_currentLine.StartsWith('(') && _currentLine.EndsWith(')'))
        {
            return InstructionType.LABEL;
        }
        
        return InstructionType.C_INSTRUCTION;
    }

    public string Symbol()
    {
        var type = InstructionType();
        
        return type switch
        {
            InstructionType.A_INSTRUCTION => _currentLine[1..],       // Remove o '@'
            InstructionType.LABEL         => _currentLine[1..^1],     // Remove '(' e ')'
            _ => throw new InvalidOperationException("Symbol só pode ser chamado para instruções A ou LABEL.")
        };
    }

    public string Dest()
    {
        if (InstructionType() != InstructionType.C_INSTRUCTION)
        {
            throw new InvalidOperationException("Dest só é válido para instruções do tipo C.");
        }

        if (_currentLine.Contains('='))
        {
            return _currentLine.Split('=')[0];
        }

        return string.Empty;
    }

    public string Comp()
    {
        if (InstructionType() != InstructionType.C_INSTRUCTION)
        {
            throw new InvalidOperationException("Comp só é válido para instruções do tipo C.");
        }

        string remaining = _currentLine;

        // Isola a parte após o '=' se houver destino
        if (remaining.Contains('='))
        {
            remaining = remaining.Split('=')[1];
        }

        // Isola a parte antes do ';' se houver jump
        if (remaining.Contains(';'))
        {
            remaining = remaining.Split(';')[0];
        }

        return remaining;
    }

    public string Jump()
    {
        if (InstructionType() != InstructionType.C_INSTRUCTION)
        {
            throw new InvalidOperationException("Jump só é válido para instruções do tipo C.");
        }

        if (_currentLine.Contains(';'))
        {
            return _currentLine.Split(';')[1];
        }

        return string.Empty;
    }
}