using CommunityToolkit.Mvvm.Input;
using Fiszki.Models;

namespace Fiszki.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}