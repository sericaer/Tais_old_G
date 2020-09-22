using Godot;
using RunData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaisGodot.Scripts
{
    class TaskContainer : VBoxContainer
    {
        internal void Refresh(IEnumerable<Process> processes)
        {
            var taskItems = this.GetChildren<Task>().ToList();

            var needRemoves = taskItems.FindAll(x => !processes.Any(y => y.name == x.Name));
            needRemoves.ForEach(x =>
            {
                taskItems.Remove(x);
                x.QueueFree();
            });

            foreach (var process in processes)
            {
                var taskItem = taskItems.SingleOrDefault(x => x.Name == process.name);
                if (taskItem == null)
                {
                    taskItem = (Task)ResourceLoader.Load<PackedScene>("res://Scenes/Main/Task/Task.tscn").Instance();
                    taskItem.Name = process.name;

                    AddChild(taskItem);
                }

                taskItem.Refresh(process);
            }
        }
    }
}
