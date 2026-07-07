using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Assembler.Parser;

/// Define os tipos de instrução da arquitetura Hack.
public enum CommandType
{
    A_INSTRUCTION,
    C_INSTRUCTION,
    LABEL
}

/// Lê o arquivo fonte .asm, remove espaços e comentários e isola cada componente das instruções
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

        _lines = File.ReadLines(filename)
            .Select(CleanLine)
            .Where(line => !string.IsNullOrEmpty(line))
            .ToList();

        _currentIndex = 0;
    }

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

    public CommandType InstructionType()
    {
        if (_currentLine.StartsWith('@'))
        {
            return CommandType.A_INSTRUCTION;
        }
        
        if (_currentLine.StartsWith('(') && _currentLine.EndsWith(')'))
        {
            return CommandType.LABEL;
        }
        
        return CommandType.C_INSTRUCTION;
    }

    public string Symbol()
    {
        var type = InstructionType();
        
        return type switch
        {
            CommandType.A_INSTRUCTION => _currentLine[1..],       
            CommandType.LABEL         => _currentLine[1..^1],     
            _ => throw new InvalidOperationException("Symbol só pode ser chamado para instruções A ou LABEL.")
        };
    }

    public string Dest()
    {
        if (InstructionType() != CommandType.C_INSTRUCTION)
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
        if (InstructionType() != CommandType.C_INSTRUCTION)
        {
            throw new InvalidOperationException("Comp só é válido para instruções do tipo C.");
        }

        string remaining = _currentLine;

        if (remaining.Contains('='))
        {
            remaining = remaining.Split('=')[1];
        }

        if (remaining.Contains(';'))
        {
            remaining = remaining.Split(';')[0];
        }

        return remaining;
    }

    public string Jump()
    {
        if (InstructionType() != CommandType.C_INSTRUCTION)
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