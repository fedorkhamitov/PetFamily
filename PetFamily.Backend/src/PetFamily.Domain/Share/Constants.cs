namespace PetFamily.Domain.Share;

public static class Constants
{
    public const int MAX_LOW_TEXT_LENGHT = 100;
    public const int MAX_HIGH_TEXT_LENGHT = 2000;
    public const string DATABASE = "Database";
    public const string SEPARATOR = "||";
    public const string PHOTOS_BUCKET_NAME = "pictures";
    public const int MINIO_EXPIRED_TIME= 604800;
    public const int MAX_PET_WIGHT = 200;
    public const int MIN_PET_WIGHT = 5;
    public const int MAX_PET_HEIGHT = 300;
    public const int MIN_PET_HEIGHT = 10;
    public const int MAX_PARALLEL_TASK_LENGTH = 5;
}