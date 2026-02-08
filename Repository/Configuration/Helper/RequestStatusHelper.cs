using tunepool.Repository.ViewModel;

namespace tunepool.Repository.Configuration.Helper
{
    public class RequestStatusHelper
    {
        public object? Success(int status, bool success, string? messsage, object? content, bool? lastpage)
        {
            return new ResponseApiViewModel()
            {
                Status = status,
                Success = success,
                Message =   messsage,
                Content = content,
                lastPage = lastpage
            };
        }
    }
}
