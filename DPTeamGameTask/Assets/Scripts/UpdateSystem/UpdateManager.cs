using System;
using System.Collections.Generic;
using UnityEngine;

namespace DPTeam.UpdateSystem
{
    public class UpdateManager
    {
        public UpdaterActions UpdateActions { get; private set; } = new UpdaterActions();
        public UpdaterActions FixedUpdateActions { get; private set; } = new UpdaterActions();
        public UpdaterActions LateUpdateActions { get; private set; } = new UpdaterActions();
    }

    public class UpdaterActions
    {
        private readonly List<ActionItem> actions = new();
        private readonly List<ActionItem> actionsToAdd = new();
        private readonly HashSet<Action> actionsToRemove = new();

        public void AddAction(Action action, int priority = 0)
        {
            if (DoesAlreadyExist(action)) return;

            actionsToAdd.Add(new ActionItem(action, priority));
        }

        public void RemoveAction(Action action)
        {
            actionsToRemove.Add(action);
        }

        public void InvokeActions()
        {
            foreach (var actionItem in actions)
            {
                actionItem.InvokeAction();
            }
            
            RemoveActionsInternal();
            AddActionsInternal();

        }

        private void RemoveActionsInternal()
        {
            for (int i = actions.Count - 1; i >= 0; i--)
            {
                Action action = actions[i].Action;
                if (actionsToRemove.Contains(action))
                {
                    actions.RemoveAt(i);
                    actionsToRemove.Remove(action);
                }
            }

            if (actionsToRemove.Count > 0)
            {
                Debug.LogWarning("You are trying to remove action not added yet.");
                actionsToRemove.Clear();
            }
        }

        private void AddActionsInternal()
        {
            actions.AddRange(actionsToAdd);
            actions.Sort();
            
            actionsToAdd.Clear();
        }

        private bool DoesAlreadyExist(Action action)
        {
            foreach (ActionItem existingAction in actions)
            {
                if (!existingAction.Equals(action)) continue;
                
                Debug.LogWarning("Action already added to the updater! Can't add a duplicate.");
                return true;
            }

            return false;
        }

        private class ActionItem : IComparable<ActionItem>
        {
            public ActionItem(Action action, int priority)
            {
                Action = action;
                this.priority = priority;
            }
            
            public Action Action { get; }
            private readonly int priority;

            public void InvokeAction() => Action.Invoke();

            public bool Equals(Action action) => action.Equals(Action);

            public int CompareTo(ActionItem other)
            {
                if (ReferenceEquals(this, other)) return 0;
                if (ReferenceEquals(null, other)) return 1;
                return priority.CompareTo(other.priority);
            }
        }
    }
}