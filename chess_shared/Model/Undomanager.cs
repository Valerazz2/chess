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
         public bool IsUndoAvailable => cursor > 0 && CurrentCommand == null;

         public bool IsRedoAvailable => cursor < Size && CurrentCommand == null;
        
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
        
        private void OnComplete()
        {
            CurrentCommand = null;
            Update();
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
            OnComplete();
        }
        
        public void Redo()
        {
            CurrentCommand = commands[cursor];
            cursor++;
            Update();
            CurrentCommand.Apply();
            OnComplete();
        }
    }

    public interface IUndoableCommand
    {
        void Apply();
        
        void Revert();
    }
}
