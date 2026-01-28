namespace tunepool.Repository.Service.Validation.Playlist
{
    public class PlaylistValidation
    {
        public void PlaylistInput(string link, string title, string description, string[] tags)
        {
            if (string.IsNullOrEmpty(link)) throw new Exception("link is required");
            if (string.IsNullOrEmpty(title)) throw new Exception("title is required");
            if (string.IsNullOrEmpty(description)) throw new Exception("description is required");
            if (tags == null || tags.Length == 0) throw new Exception("tags is requried");
        }
    }
}
