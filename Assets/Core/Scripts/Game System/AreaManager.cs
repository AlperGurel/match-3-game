using System;

namespace Match3
{
    public class AreaManager : SingletonGameSystem<AreaManager>
    {
        public override void Initialize()
        {
            base.Initialize();

            
            var taskObjects = Main.Instance.AreaTasks;
            
            
            foreach (var taskData in Player.Instance.PlayerData.AreaTaskData)
            {
                taskObjects[taskData.TaskIndex].Initialize();
                switch (taskData.State)
                {
                    case TaskState.LOCKED:
                        break;
                    case TaskState.UNLOCKED:
                        taskObjects[taskData.TaskIndex].UnlockTask();
                        break;
                    case TaskState.COMPLETED:
                        taskObjects[taskData.TaskIndex].CompleteTask(true);
                        break;
                    case TaskState.SHOWN_COMPLETED_ANIMATION:
                        taskObjects[taskData.TaskIndex].CompleteTask(false);
                        break;
                }
            }
            
        }

        public void OnLevelWin()
        {
            int level = Player.Instance.PlayerData.CurrentLevel;
            if (level == 1) return;
            int unlockedTaskIndex = level - 2;
            Main.Instance.AreaTasks[unlockedTaskIndex].UnlockTask();

        }
    }

    [Serializable]
    public struct AreaTask
    {
        public int TaskIndex;
        public TaskState State;

        public AreaTask(int taskIndex, TaskState state)
        {
            TaskIndex = taskIndex;
            State = state;
        }
    }

    [Serializable]
    public enum TaskState
    {
        LOCKED,
        UNLOCKED,
        COMPLETED,
        SHOWN_COMPLETED_ANIMATION
    }
}