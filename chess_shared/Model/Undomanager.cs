using System.Collections.Generic;
using chess_shared.Model;

namespace Common.Util.Undo
{
    public class UndoManager
    {
        public readonly List<IUndoableCommand> commands = new();

        public int Size => commands.Count;
        
        public int cursor;

        public readonly Holder<bool> undoAvailable = new();
        // public bool IsUndoAvailable => cursor > 0 && CurrentCommand == null;
        public bool IsUndoAvailable => cursor > 0;
        
        public bool IsRedoAvailable => cursor < Size;
        
        public readonly Holder<bool> redoAvailable = new();
        
        public IUndoableCommand CurrentCommand { get; private set; }
        
        
        public void Clear()
        {
            commands.Clear();
            cursor = 0;
            CurrentCommand = null;
            Update();
        }
        
        private void Update()
        {
            undoAvailable.Value = IsUndoAvailable;
            redoAvailable.Value = IsRedoAvailable;
        }

        public void AddCommand(IUndoableCommand cmd, bool apply = true)
        {
            if (cursor < Size)
            {
                commands.RemoveRange(cursor, Size - cursor);
            }
            commands.Add(cmd);
            cursor++;
            Update();
            if (!apply) return;
            CurrentCommand = cmd;
            cmd.Apply();
        }

        public void Undo()
        {
            CurrentCommand = commands[cursor - 1];
            cursor--;
            Update();
            CurrentCommand.Revert();
        }
        
        public void Redo()
        {
            CurrentCommand = commands[cursor];
            cursor++;
            Update();
            CurrentCommand.Apply();
        }
    }

    public interface IUndoableCommand
    {
        void Apply();
        
        void Revert();
    }
}
