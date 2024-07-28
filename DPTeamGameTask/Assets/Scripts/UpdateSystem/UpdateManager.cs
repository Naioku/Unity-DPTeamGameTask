using System;
using System.Collections.Generic;
using UnityEngine;

namespace DPTeam.UpdateSystem
{
    public class UpdateManager
    {
        public UpdaterActions UpdateActions { get; private set; } = new();
        public UpdaterActions FixedUpdateActions { get; private set; } = new();
        public UpdaterActions LateUpdateActions { get; private set; } = new();
    }

    public class UpdaterActions
    {
        private readonly List<ActionItem> actionItems = new();
        private readonly List<ActionItem> actionItemsToAdd = new();

        public void AddAction(Action action, int priority = 0)
        {
            if (DoesAlreadyExist(action)) return;

            actionItemsToAdd.Add(new ActionItem(action, priority));
        }

        public void RemoveAction(Action action)
        {
            bool foundAction = false;
            int actionItemsCount = actionItems.Count;
            for (int i = 0; i < actionItemsCount; i++)
            {
                ActionItem actionItem = actionItems[i];
                if (actionItem.Action.Equals(action))
                {
                    foundAction = true;
                    actionItem.Active = false;
                    break;
                }
            }
            
            if (!foundAction)
            {
                Debug.LogWarning("You are trying to remove action not added yet.");
            }
        }

        public void InvokeActions()
        {
            foreach (ActionItem actionItem in actionItems)
            {
                if (!actionItem.Active) continue;
                
                actionItem.InvokeAction();
            }
            
            RemoveActionsInternal();
            AddActionsInternal();
        }

        private void RemoveActionsInternal()
        {
            for (int i = actionItems.Count - 1; i >= 0; i--)
            {
                if (actionItems[i].Active) continue;
                
                actionItems.RemoveAt(i);
            }
        }

        private void AddActionsInternal()
        {
            actionItems.AddRange(actionItemsToAdd);
            actionItems.Sort();
            
            actionItemsToAdd.Clear();
        }

        private bool DoesAlreadyExist(Action action)
        {
            foreach (ActionItem existingAction in actionItems)
            {
                if (!existingAction.Equals(action)) continue;
                
                Debug.LogWarning("Action already added to the updater! Can't add a duplicate.");
                return true;
            }

            return false;
        }

        private class ActionItem : IComparable<ActionItem>
        {
            private readonly int priority;
                
            public Action Action { get; }
            public bool Active { get; set; } = true;
            
            public ActionItem(Action action, int priority)
            {
                Action = action;
                this.priority = priority;
            }

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