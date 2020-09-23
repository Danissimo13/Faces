namespace FacesWebApi.Services.Abstractions
{
    public interface IHashService
    {
        byte[] Key { get; }

        byte[] GetHash(byte[] buffer);
        byte[] GetHash(string str);
    }
}
