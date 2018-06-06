namespace Ataoge.BackgroundJobs
{
    public interface ITask
    {
        int Progresss {get; set;}

        string CurrentTaskId {get; set;}
    }
}