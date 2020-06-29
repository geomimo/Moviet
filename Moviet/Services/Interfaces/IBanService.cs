namespace Moviet.Services.Interfaces
{
    public interface IBanService
    {
        public bool BanUser(string userId);
        public bool BanPost(int postId);
    }
}
